using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using DG.Tweening;
public class Char_Agent : MonoBehaviour
{
    // public Transform Target;
    public int IDPlace;
    
    public Transform Canvas;
    public Image Character_image;
    public TMPro.TMP_Text Percent_text;
    public Image FuritIcon_image;
    public Image Prograssbar_time;
    public Image Prograssbar_satisfaction;
    public float NeedPercentValue;


    private NavMeshAgent agent;
    private Animator animator;
    private ShopperSystemController shopperSystem;


    private float Timeresponse;
    private float H;
    private float M;
    private float S;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        shopperSystem = FindObjectOfType<ShopperSystemController>();
      //  shopperSystem.OnAgentMove += ShopperSystem_OnAgentMove;
    }
    private void LateUpdate()
    {
        
    }
    public void HappyMotion()
    {
        this.tag = "destroy";
        animator.SetBool("Happy", true);
        DOVirtual.DelayedCall(5, () => {
            var pos = FindObjectOfType<DestroyPlace>().transform.position;
            AgentMoveToDestroy(pos);
        });
    }
    public void AngryMotion()
    {
        this.tag = "destroy";
        animator.SetBool("Angry", true);
        DOVirtual.DelayedCall(5, () => {
            var pos = FindObjectOfType<DestroyPlace>().transform.position;
            AgentMoveToDestroy(pos);
        });
    }
    public void AgentMoveToDestroy(Vector3 pos)
    {
        this.tag = "destroy";
        animator.SetBool("Angry", false);
        animator.SetBool("Happy", false);
        animator.SetBool("Walk", true);
        agent.isStopped = false;
        agent.destination = pos;
       // shopperSystem.CustomerInWave--;
    }
    private void OnDestroy()
    {
       
    }
 
 

    public void SetDestination(Vector3 pos)
    {
       animator.SetBool("Walk", true);
        agent.destination = pos;
        Debug.Log("POS SET");
    }



    public void SetUI(Sprite profile, Sprite fruitIcon, float percent , float timereponse)
    {
        this.Character_image.sprite = profile;
        this.FuritIcon_image.sprite = fruitIcon;
        this.Percent_text.text = percent.ToString("0") + "%";
        this.NeedPercentValue = percent;
        this.Timeresponse = timereponse;
        this.Prograssbar_time.fillAmount = 1;
       // CalculateTime(timereponse);
    }

    public void PrograssbarAndPointSet(float amount)
    {
        var unit = amount / NeedPercentValue;

        Prograssbar_satisfaction.fillAmount += unit;
        if (Prograssbar_satisfaction.fillAmount > 0.5)
        {
            Prograssbar_satisfaction.color = Color.green;
         
        }
        else if (Prograssbar_satisfaction.fillAmount > 0.25 && Prograssbar_satisfaction.fillAmount < 0.5)
        {
            Prograssbar_satisfaction.color = Color.yellow;
      
        }
        else if (Prograssbar_satisfaction.fillAmount > 0 && Prograssbar_satisfaction.fillAmount < 0.25)
        {
            Prograssbar_satisfaction.color = Color.red;
           
        }
        //  this.Percent_text.text = (Mathf.Clamp(amount / NeedPercentValue, 0, 1) * 100).ToString("0"); 
        var point_offset = (NeedPercentValue - amount);

        if (point_offset <= 2)
        {
            HappyMotion();
        }
        else if (point_offset > 2 && point_offset <= 5)
        {
            HappyMotion();
        }
        else if (point_offset >= 5 && point_offset < 11)
        {
            AngryMotion();
        }
        else if (point_offset > 15)
        {
            AngryMotion();
        }
       
        shopperSystem.CalculateScore(point_offset);
    }


    public void CalculateTime(float time)
    {
        H = 0;
        M = 0;
        S = 0;
        CancelInvoke("RunTimer");


        H = (float)Math.Floor(time / 3600);
        M = (float)Math.Floor(time / 60 % 60);
        S = (float)Math.Floor(time % 60);
        InvokeRepeating("RunTimer", 0, 1.0f);
    }
    /// <summary>
    /// INVOKE IN Calculate
    /// </summary>
    public void RunTimer()
    {
        S--;
        if (S < 0)
        {
            if (M > 0 || H > 0)
            {
                S = 59;
                M--;
                if (M < 0)
                {
                    if (H > 0)
                    {
                        M = 59;
                        H--;
                    }
                    else
                    {
                        M = 0;
                    }
                }

            }
            else
            {
                S = 0;
            }
        }


        var unit = 1 / Timeresponse;
        this.Prograssbar_time.fillAmount -= unit;
        if (Prograssbar_time.fillAmount > 0.5)
        {
            Prograssbar_time.color = Color.green;
        }
        else if (Prograssbar_time.fillAmount > 0.25 && Prograssbar_time.fillAmount < 0.5)
        {
            Prograssbar_time.color = Color.yellow;
        }
        else if (Prograssbar_time.fillAmount > 0 && Prograssbar_time.fillAmount < 0.25)
        {
            Prograssbar_time.color = Color.red;
        }
        if (S == 0 && M == 0 && H == 0)
        {
            CancelInvoke("RunTimer");
          //  var pos = FindObjectOfType<DestroyPlace>().transform.position;
            AngryMotion();

        }
    }
}
