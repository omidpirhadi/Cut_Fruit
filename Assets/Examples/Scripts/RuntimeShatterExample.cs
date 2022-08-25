using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;
using Sirenix.OdinInspector;
/**
 * Represents a really badly written shatter script! use for reference purposes only.
 */
public class RuntimeShatterExample : MonoBehaviour
{

    public GameObject objectToShatter;
    public Material crossSectionMaterial;

    public List<GameObject> prevShatters = new List<GameObject>();

    public GameObject[] ShatterObject(GameObject obj, Material crossSectionMaterial = null)
    {
        return obj.SliceInstantiate(GetRandomPlane(obj.transform.position, obj.transform.localScale),
                                                            new TextureRegion(0.0f, 0.0f, 1.0f, 1.0f),
                                                            crossSectionMaterial);
    }

    public EzySlice.Plane GetRandomPlane(Vector3 positionOffset, Vector3 scaleOffset)
    {
        Vector3 randomPosition = Random.insideUnitSphere;

        //randomPosition += positionOffset;

        Vector3 randomDirection = Random.insideUnitSphere.normalized;

        return new EzySlice.Plane(randomPosition, randomDirection);
    }
    [Button("Shatter")]
    public void RandomShatter(int count)
    {

        List<GameObject> shatters = new List<GameObject>();
        // otherwise, shatter the previous shattered objects, randomly picked
        for (int i = 0; i < count; i++)
        {
            shatters = ShatterObject(objectToShatter, crossSectionMaterial).ToList();
        }

      


        

        // add rigidbodies and colliders
        foreach (GameObject shatteredObject in shatters)
        {
            shatteredObject.AddComponent<MeshCollider>().convex = true;
            shatteredObject.AddComponent<Rigidbody>();


        }
       // objectToShatter.SetActive(false);
    }

}
