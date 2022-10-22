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
    private HomePanel homePanel;
    private ShopperSystemController shopperSystem;
    private Vector3 standPos;
    private Vector3 standrotate;
    void Start()
    {
        
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        homePanel = FindObjectOfType<HomePanel>();
        shopperSystem = FindObjectOfType<ShopperSystemController>();
        homePanel.OnPlay += Character_NPC_OnPlay;
        shopperSystem.OnEndGame += ShopperSystem_OnEndGame;
        standPos = transform.position;
        standrotate = transform.eulerAngles;
    }

    private void ShopperSystem_OnEndGame()
    {
        this.tag = "null";
        DOVirtual.DelayedCall(2,()=>{ this.tag = "npc"; });
        
        transform.position = standPos;
        transform.eulerAngles = standrotate;
        this.gameObject.SetActive(true);
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
    private void OnDestroy()
    {
        homePanel.OnPlay -= Character_NPC_OnPlay;
    }
    
  
  
}


