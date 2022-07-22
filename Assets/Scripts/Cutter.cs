
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;
using Sirenix.OdinInspector;
public class Cutter : MonoBehaviour
{

    public GameObject objectToSlice; // non-null
    public Material crossSectionMaterial; // non-null
    public Transform Plane;
    // public GameObject p1, p2;
    //public Vector3 vec;
    /**
     * Example on how to slice a GameObject in world coordinates.
     * Uses a custom TextureRegion to offset the UV coordinates of the cross-section
     * Uses a custom Material
     */
   // [Button("Cut", ButtonSizes.Medium)]

    public void Cut( GameObject furit)
    {
        string tag_furit = "";
        if (furit.GetComponent<Furit>())
        {
             tag_furit = furit.GetComponent<Furit>().FuritTag;
        }
        else
        {
            tag_furit = furit.GetComponent<FruitPiece>().FuritTag;

        }
        var hull = Slice(furit, Plane.transform.position, Plane.transform.up, crossSectionMaterial);

        var lower = hull.CreateLowerHull(objectToSlice, crossSectionMaterial);
        lower.AddComponent<MeshCollider>().convex = true;
        lower.AddComponent<Rigidbody>();
        lower.AddComponent<FruitPiece>().FuritTag  = tag_furit;       
        lower.layer = LayerMask.NameToLayer("Furit");
        lower.name = furit.name;


        var upper = hull.CreateUpperHull(objectToSlice, crossSectionMaterial);
        upper.AddComponent<MeshCollider>().convex = true;
        upper.AddComponent<Rigidbody>();
        upper.AddComponent<FruitPiece>().FuritTag = tag_furit;
        upper.layer = LayerMask.NameToLayer("Furit");
        upper.name = furit.name;

        objectToSlice.SetActive(false);
        //Handler_Cut();

        // Debug.Log("Cut");
    }
    public void SetCutPlane(Vector3 point1, Vector3 point2)
    {
        var dir = point2 - point1;
        this.Plane.position = point1 + (dir / 2.0f);
        this.Plane.localScale = new Vector3(1, 1, 1);
        this.Plane.forward = dir;
    }

    private SlicedHull Slice( GameObject furit, Vector3 planeWorldPosition, Vector3 planeWorldDirection, Material mat)
    {

        objectToSlice = furit;
      
        return objectToSlice.Slice(planeWorldPosition, planeWorldDirection, mat);
    }

    private Action cut;

    public event Action OnCut
    {
        add { cut += value; }
        remove { cut -= value; }
    }
    protected void Handler_Cut()
    {
        if(cut !=null)
        {
            cut();
        }
    }
}
