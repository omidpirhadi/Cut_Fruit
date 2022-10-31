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

    public Transform DestroyPlace;
    public float SpeedTurn = 1.5f;
    private NavMeshAgent agent;
    
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
        TurnAgent(DestroyPlace.position);
        SetDestination(DestroyPlace.position);


    }
    public Tweener TurnAgent(Vector3 Destroyplace)
    {
        var dir = (Destroyplace - transform.position);
        var angel = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
        Quaternion a = Quaternion.Euler(new Vector3(0, -angel + 90, 0));
        var tweener = transform.DORotateQuaternion(a, SpeedTurn);
        Debug.Log("Angel:" + angel + "AA" + gameObject.name);
        return tweener;

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


