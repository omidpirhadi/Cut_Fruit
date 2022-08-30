using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Char_Agent : MonoBehaviour
{
   // public Transform Target;
    private NavMeshAgent agent;
    public Animator animator;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }
    private void LateUpdate()
    {
        var dis = Vector3.Distance(transform.position, agent.destination);
        
        if (dis<1)
        {
            animator.SetBool("Walk", false);
           
            //   Debug.Log("as");
            Debug.Log(dis);
        }
    }
    public void SetDestination(Vector3 pos)
    {
       animator.SetBool("Walk", true);
        agent.stoppingDistance = 1;
        agent.destination = pos;
    }
}
