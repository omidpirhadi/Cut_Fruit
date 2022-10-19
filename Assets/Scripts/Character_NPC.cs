using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using DG.Tweening;

public class Character_NPC : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform DestroyPlace;
    private Animator animator;
    void Start()
    {
        
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        FindObjectOfType<HomePanel>().OnPlay += Character_NPC_OnPlay;

        //  shopperSystem.OnAgentMove += ShopperSystem_OnAgentMove;
    }

    private void Character_NPC_OnPlay()
    {
        transform.DOLookAt(DestroyPlace.position, 0.5f);
        SetDestination(DestroyPlace.position);
    }

    public void SetDestination(Vector3 pos)
    {
        animator.SetBool("Walk", true);

        agent.destination = pos;
        
        //Debug.Log("POS SET");
    }
}
