using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
public class PlaceShopper : MonoBehaviour
{

    private ShopperSystemController shopperSystem;
    public bool HaveShopper = false;
    private void Start()
    {
        shopperSystem = FindObjectOfType<ShopperSystemController>();
        shopperSystem.OnResetWave += ShopperSystem_OnResetWave;
    }

    private void ShopperSystem_OnResetWave()
    {
        HaveShopper = false;
    }

    private void OnTriggerEnter(Collider obj)
    {
        if (obj.tag == "shopper" && HaveShopper== false)
        {
            HaveShopper = true;
            Vector3 cam_forward = Camera.main.transform.position;
            var agent = obj.GetComponent<NavMeshAgent>();
            var animator = obj.GetComponent<Animator>();
            agent.isStopped = true;
            animator.SetBool("Walk", false);

            shopperSystem.ShopperInPlaceCount++;
            //obj.transform.DOLookAt(cam_forward, 2);
           // Debug.Log("AAAAAAAA");
        }
    }
}
