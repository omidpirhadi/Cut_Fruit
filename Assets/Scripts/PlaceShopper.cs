using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
public class PlaceShopper : MonoBehaviour
{

    public Transform TargetViweCharacter;
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
           
            var agent = obj.GetComponent<NavMeshAgent>();
            var animator = obj.GetComponent<Animator>();
            agent.isStopped = true;
            animator.SetBool("Walk", false);
            obj.transform.DOLookAt(TargetViweCharacter.position, 2);
            shopperSystem.ShopperInPlaceCount++;
        
           // Debug.Log("AAAAAAAA");
        }
    }
}
