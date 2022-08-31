using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPlace : MonoBehaviour
{

    private void OnTriggerEnter(Collider obj)
    {
        if (obj.tag == "destroy")
        {
            Destroy(obj.gameObject);
            //obj.transform.DOLookAt(cam_forward, 2);
            // Debug.Log("AAAAAAAA");
        }
    }
}
