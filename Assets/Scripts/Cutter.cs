using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;
using Sirenix.OdinInspector;
public class Cutter : MonoBehaviour
{

    public GameObject objectToSlice; // non-null
    public Material crossSectionMaterial; // non-null
    public GameObject Plane;
    public GameObject p1, p2;
    public Vector3 vec;
    /**
     * Example on how to slice a GameObject in world coordinates.
     * Uses a custom TextureRegion to offset the UV coordinates of the cross-section
     * Uses a custom Material
     */
    [Button("Cut", ButtonSizes.Medium)]

    public void Cut()
    {
        // var dir = p2.transform.position - p1.transform.position;
        var hull = Slice(Plane.transform.position, Plane.transform.up, crossSectionMaterial);

        hull.CreateLowerHull(objectToSlice, crossSectionMaterial);
        hull.CreateUpperHull(objectToSlice, crossSectionMaterial);
        objectToSlice.SetActive(false);
        // Debug.Log("Cut");
    }
    [Button("Angle", ButtonSizes.Medium)]

    public void angle()
    {
        var dir = p2.transform.position - p1.transform.position;
        float angle = Vector3.Angle(dir, vec);

        Debug.Log("angle:" + angle);
    }
    public SlicedHull Slice(Vector3 planeWorldPosition, Vector3 planeWorldDirection, Material mat)
    {

        GameObject gameObject = new GameObject();
      
        return objectToSlice.Slice(planeWorldPosition, planeWorldDirection, mat);
    }

}
