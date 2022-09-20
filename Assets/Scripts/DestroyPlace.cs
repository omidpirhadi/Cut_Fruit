using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPlace : MonoBehaviour
{
    private ShopperSystemController shopperSystem;
    private void Start()
    {
        shopperSystem = FindObjectOfType<ShopperSystemController>();
        
    }
    private void OnTriggerEnter(Collider obj)
    {
        if (obj.tag == "destroy")
        {
            //shopperSystem.ShopperInPlaceCount--;
            Destroy(obj.gameObject);
            //obj.transform.DOLookAt(cam_forward, 2);
         //  Debug.Log("AAAAAAAA");
        }
        
    }
}
