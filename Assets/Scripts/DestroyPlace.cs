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
            shopperSystem.QueueCapacity++;
            var id = obj.GetComponent<Char_Agent>().IDPlace;
            shopperSystem.FreePlaceAfterDestroyCustomer(id);

            StartCoroutine(shopperSystem.FlowSpwanCustomer());

            //  StartCoroutine(shopperSystem.SpawnCustomer(1));
            Destroy(obj.gameObject);

        }
        else if(obj.tag == "npc")
        {
            Destroy(obj.gameObject);
        }
    }
}
