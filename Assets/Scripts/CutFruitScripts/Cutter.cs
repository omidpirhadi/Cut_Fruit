﻿
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
   // public Material crossSectionMaterial; // non-null
    public PhysicMaterial physicMaterial;
  //  public Material hightlight_matrial;
    public Transform Plane;


    public void Cut(GameObject furit , Material inner)
    {
        string tag_furit = "";
        float totalvolume = 0;
        if (furit.GetComponent<Furit>())
        {
            tag_furit = furit.GetComponent<Furit>().FuritTag;
            totalvolume = furit.GetComponent<Furit>().Volume;
        }
        else
        {
            tag_furit = furit.GetComponent<FruitPiece>().FuritTag;
            totalvolume = furit.GetComponent<FruitPiece>().TotalVolume;

        }
        if (furit != null)
        {
            var hull = Slice(furit, Plane.transform.position, Plane.transform.up, inner);
            if (objectToSlice != null && hull !=null)
            {
        
                ////****************************************************************Create Lower
                var lower = hull.CreateLowerHull(objectToSlice, inner);
               
                var colliderlower = lower.AddComponent<MeshCollider>();
                colliderlower.convex = true;
                colliderlower.material = physicMaterial;
                colliderlower.convex = true;
                
                var lowerbody = lower.AddComponent<Rigidbody>();
                lowerbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
                lowerbody.interpolation = RigidbodyInterpolation.Interpolate;
                lowerbody.AddForce(-Plane.transform.up*0.1f , ForceMode.Impulse);

                var piece_lower = lower.AddComponent<FruitPiece>();
                piece_lower.FuritTag = tag_furit;
                piece_lower.InnerMatrialAfterCut = inner;
                piece_lower.TotalVolume = totalvolume;
                //piece_lower.hightlighter = hightlight_matrial;

                lower.layer = LayerMask.NameToLayer("Furit");
                lower.name = furit.name;
                lower.tag = "furit";
                DOVirtual.DelayedCall(2, () =>
                {
                    lowerbody.isKinematic = true;
                  //  colliderlower.isTrigger = true;
 
                });
                ////****************************************************************Create Upper
                var upper = hull.CreateUpperHull(objectToSlice, inner);
                var colliderupper = upper.AddComponent<MeshCollider>();
                colliderupper.material = physicMaterial;
                colliderupper.convex = true;

                var upperbody = upper.AddComponent<Rigidbody>();

                upperbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
                upperbody.interpolation = RigidbodyInterpolation.Interpolate;
                upperbody.AddForce(Plane.transform.up*0.1f , ForceMode.Impulse);

                var piece_upper = upper.AddComponent<FruitPiece>();
                piece_upper.FuritTag = tag_furit;
                piece_upper.InnerMatrialAfterCut = inner;
                piece_upper.TotalVolume = totalvolume;
              
                // piece_upper.hightlighter = hightlight_matrial;

                upper.layer = LayerMask.NameToLayer("Furit");
                upper.name = furit.name;
                upper.tag = "furit";
                DOVirtual.DelayedCall(2, () =>
                {
                    upperbody.isKinematic = true;
                   // colliderupper.isTrigger = true;

                });
                // objectToSlice.SetActive(false);
                Destroy(objectToSlice);
             //   Handler_Cut();
            }
        }
        // Debug.Log("Cut");
    }
    public void SetCutPlane(Vector3 point1, Vector3 point2, float muliply = 1)
    {
        var dir = point2 - point1;
        var dis = Vector3.Distance(point1, point2);
        var initscale = this.Plane.localScale;
        var newscale = (initscale * dis) * muliply;
        this.Plane.position = point1 + (dir / 2.0f);
        this.Plane.localScale = Vector3.one;
        this.Plane.forward = dir;
        this.Plane.eulerAngles = new Vector3(this.Plane.eulerAngles.x, this.Plane.eulerAngles.y, 90);
    }

    private SlicedHull Slice( GameObject furit, Vector3 planeWorldPosition, Vector3 planeWorldDirection, Material mat)
    {

        objectToSlice = furit;
      
        return objectToSlice.Slice(planeWorldPosition, planeWorldDirection, mat);
    }


}