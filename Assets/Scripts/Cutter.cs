
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;
using DG.Tweening;
using Sirenix.OdinInspector;
public class Cutter : MonoBehaviour
{

    public GameObject objectToSlice; // non-null
    public Material crossSectionMaterial; // non-null
    public PhysicMaterial physicMaterial;
    public Material hightlight_matrial;
    public Transform Plane;
    // public GameObject p1, p2;
    //public Vector3 vec;
    /**
     * Example on how to slice a GameObject in world coordinates.
     * Uses a custom TextureRegion to offset the UV coordinates of the cross-section
     * Uses a custom Material
     */
    // [Button("Cut", ButtonSizes.Medium)]

    public void Cut(GameObject furit)
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

        ////****************************************************************Create Lower
        var lower = hull.CreateLowerHull(objectToSlice, crossSectionMaterial);
        lower.AddComponent<MeshCollider>().convex = true;
        var colliderlower = lower.AddComponent<MeshCollider>();
        colliderlower.material = physicMaterial;
        colliderlower.convex = true;

        var lowerbody = lower.AddComponent<Rigidbody>();
        lowerbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        lowerbody.interpolation = RigidbodyInterpolation.Interpolate;
        lowerbody.AddForce(Vector3.forward * 5, ForceMode.Impulse);

        var piece_lower = lower.AddComponent<FruitPiece>();
        piece_lower.FuritTag = tag_furit;
        //piece_lower.hightlighter = hightlight_matrial;

        lower.layer = LayerMask.NameToLayer("Furit");
        lower.name = furit.name;
        DOVirtual.DelayedCall(2, () => {
            lowerbody.isKinematic = true;
        });
        ////****************************************************************Create Upper
        var upper = hull.CreateUpperHull(objectToSlice, crossSectionMaterial);
        var colliderupper = upper.AddComponent<MeshCollider>();
        colliderupper.material = physicMaterial;
        colliderupper.convex = true;

        var upperbody = upper.AddComponent<Rigidbody>();

        upperbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        upperbody.interpolation = RigidbodyInterpolation.Interpolate;
        upperbody.AddForce(Vector3.forward * 5, ForceMode.Impulse);

        var piece_upper= upper.AddComponent<FruitPiece>();
        piece_upper.FuritTag = tag_furit;
       // piece_upper.hightlighter = hightlight_matrial;

        upper.layer = LayerMask.NameToLayer("Furit");
        upper.name = furit.name;

        DOVirtual.DelayedCall(2, () => {
            upperbody.isKinematic = true;
        });
        // objectToSlice.SetActive(false);
        Destroy(objectToSlice);
        Handler_Cut();

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
