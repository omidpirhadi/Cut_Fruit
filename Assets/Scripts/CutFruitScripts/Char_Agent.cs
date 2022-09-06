using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Char_Agent : MonoBehaviour
{
    // public Transform Target;
    public int ID;
    private NavMeshAgent agent;
    public Animator animator;
    private ShopperSystemController shopperSystem;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        shopperSystem = FindObjectOfType<ShopperSystemController>();
        shopperSystem.OnAgentMove += ShopperSystem_OnAgentMove;
    }

    private void ShopperSystem_OnAgentMove(Vector3 pos)
    {
        this.tag = "destroy";
        animator.SetBool("Walk", true);
        agent.isStopped = false;
        agent.destination = pos;
        
    }
    private void OnDestroy()
    {
        shopperSystem.OnAgentMove -= ShopperSystem_OnAgentMove;
    }
 
 

    public void SetDestination(Vector3 pos)
    {
       animator.SetBool("Walk", true);
        agent.destination = pos;
    }
}
