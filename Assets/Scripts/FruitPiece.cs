using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitPiece : MonoBehaviour
{
    public string FuritTag;

    public Material InnerMatrialAfterCut;

    public float Volume;
    public bool IsReadyPickedUp = false;

    private TouchController controller;
    private Mesh meshFilter;
    private  new MeshRenderer  renderer;
    
    //private Material default_matrial;
   // private Material default_uv;

    private float Get_Z;
    private UI ui;
    void Start()
    {

        ui = FindObjectOfType<UI>();
        ui.ChangeMode += Furit_ChangeMode;
        meshFilter = GetComponent<MeshFilter>().sharedMesh;
        Volume = VolumeOfMesh(meshFilter);
        controller = FindObjectOfType<TouchController>();
        renderer = GetComponent<MeshRenderer>();
       // default_matrial = GetComponent<MeshRenderer>().materials[0];
       // default_uv = GetComponent<MeshRenderer>().materials[1];
    }

    private void Furit_ChangeMode(bool cut, bool pick)
    {
        IsReadyPickedUp = pick;
        if (pick)
        {
         
        }
        else
        {

            
        }
    }
    private void OnMouseDrag()
    {
        if (IsReadyPickedUp)
        {

            var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var offset = transform.position - transform.TransformPoint(GetComponent<MeshFilter>().mesh.bounds.center);
            var tt = new Vector3(pos.x, pos.y, transform.position.z) + offset;
            tt.z = Get_Z;
            this.transform.position = tt;
        }
    }

    private void OnMouseDown()
    {
        if (IsReadyPickedUp)
        {
            Get_Z = transform.position.z;
            renderer.materials[0].SetFloat("_OutlineSize", 10);
            Debug.Log("PICK");
        }
    }
    private void OnMouseUp()
    {
        if (IsReadyPickedUp)
        {
            /// Get_Z = transform.position.z;
            renderer.materials[0].SetFloat("_OutlineSize", 0);
            Debug.Log("Drop");
        }
    }
    void OnMouseEnter()
    {
        controller.FruitSelect(this.gameObject, InnerMatrialAfterCut);
    }

    void OnMouseExit()
    {
        // controller.FruitSelect(this.gameObject);
    }
    private void OnDestroy()
    {
        ui.ChangeMode -= Furit_ChangeMode;
    }
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
            /// volume *= this.transform.localScale.x * this.transform.localScale.y * this.transform.localScale.z;
        }
        return Mathf.Abs(volume);
    }
}

