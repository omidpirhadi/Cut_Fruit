using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;
public class FruitPiece : MonoBehaviour,IFruit
{
    public string FuritTag;

    public Material InnerMatrialAfterCut;
    public float TotalVolume;
    public float Volume;
    public float PercentVolume;
    private Mesh meshFilter;
    public new Rigidbody rigidbody;
    public Vector3 OffsetCenter;
    public ShopperSystemController shopperSystem;
    void Start()
    {

        shopperSystem = FindObjectOfType<ShopperSystemController>();
        meshFilter = GetComponent<MeshFilter>().sharedMesh;
        Volume = VolumeOfMesh(meshFilter);
      //  controller = FindObjectOfType<TouchController>();
       //renderer = GetComponent<MeshRenderer>();
        rigidbody = GetComponent<Rigidbody>();
        PercentVolume = (Volume / TotalVolume) * 100;
        OffsetCenter = transform.position - transform.TransformPoint(meshFilter.bounds.center);
        InstantiatWeightTextEffct();
        
    }
    private void LateUpdate()
    {
        if (rigidbody.velocity.magnitude > 20)
        {
            rigidbody.drag = 20;
            rigidbody.angularDrag = 20;
        }
        else
        {
            rigidbody.drag = 1;
            rigidbody.angularDrag = 0;
        }
        if(Mathf.RoundToInt(PercentVolume) <= 0)
        {
            Debug.Log("Destroy Because This Volume 0");
            Destroy(this.gameObject);

        }
     //   transform.position = new Vector3(transform.position.x, 1.5f, transform.position.z);
    }

    private void OnCollisionEnter(Collision collision)
    {


        if (collision.gameObject.tag == "floor")
        {
            DOVirtual.DelayedCall(2f, () => {
                this.transform.DOScale(0, 1f).OnComplete(() =>
                {
                    Destroy(this.gameObject,0.1f);
                    Debug.Log("Fruit droped On Floor And Destroyed");
                });
            });



        }

    }


    //[Button("Set Pivot")]
    public void CalculatePivot()
    {
        var vertices = meshFilter.vertices;
        var center = transform.TransformPoint(meshFilter.bounds.center);
        var transformloc = transform.position;
        var dir = transformloc - center;

        var mesh = Instantiate(meshFilter); // Create a copy
        meshFilter = mesh;

        var vertices22 = mesh.vertices;
        for (int i = 0; i < vertices22.Length; i++)
        {
            vertices22[i] += dir;
        }
        meshFilter.RecalculateBounds();
    }
  /*  private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        var center = transform.TransformPoint(meshFilter.bounds.center);
        var transformloc = transform.position;
        var dir = transformloc - center;
        Debug.Log(dir);
        Gizmos.DrawSphere(center, 0.01f);
    }*/
    public float SignedVolumeOfTriangle(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float v321 = p3.x * p2.y * p1.z;
        float v231 = p2.x * p3.y * p1.z;
        float v312 = p3.x * p1.y * p2.z;
        float v132 = p1.x * p3.y * p2.z;
        float v213 = p2.x * p1.y * p3.z;
        float v123 = p1.x * p2.y * p3.z;

        return (1.0f / 6.0f) * (-v321 + v231 + v312 - v132 - v213 + v123);
    }

    public float VolumeOfMesh(Mesh mesh)
    {
        float volume = 0;

        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;

        for (int i = 0; i < triangles.Length; i += 3)
        {
            Vector3 p1 = vertices[triangles[i + 0]];
            Vector3 p2 = vertices[triangles[i + 1]];
            Vector3 p3 = vertices[triangles[i + 2]];
            volume += SignedVolumeOfTriangle(p1, p2, p3);
            
        }
        return Mathf.Abs(volume*1000);
    }

    private void InstantiatWeightTextEffct()
    {
        var indicator = shopperSystem.weightSliceFruitIndicator;
        DOVirtual.DelayedCall(0.5f, () =>
        {
            
            var pos = transform.TransformPoint(meshFilter.bounds.center);
            var t = Instantiate(indicator, pos, Quaternion.identity);

            t.SetForFruit(PercentVolume.ToString("0"));
        });

    }
}

