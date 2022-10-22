using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using DG.Tweening;


[RequireComponent(typeof(CapsuleCollider), typeof(Rigidbody), typeof(NavMeshAgent))]
public class Char_Agent : MonoBehaviour
{
    // public Transform Target;
    public int IDPlace;
    public Money CashPrefab;
   


    //public Transform Canvas;
    public Image Character_image;
    public TMPro.TMP_Text Percent_text;
    public Image FuritIcon_image;
    public Image Prograssbar_time;
    public Image Prograssbar_satisfaction;
    public RectTransform Canvans;

    public RectTransform UI;
    public float NeedPercentValue;
    public string fruitname;

    public NavMeshAgent agent;
    private Animator animator;
    private ShopperSystemController shopperSystem;


    private float Timeresponse;
    private float H;
    private float M;
    private float S;
    private float point_offset = -1000;
    private Vector3 des_pos;
    private bool ToDestroy = false;
    [SerializeField] private bool IsReadyToGiveFruit = false;



    void Start()
    {
        point_offset = -1000;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        shopperSystem = FindObjectOfType<ShopperSystemController>();
       
        //  shopperSystem.OnAgentMove += ShopperSystem_OnAgentMove;
    }
    private void LateUpdate()
    {

        UI.forward = -(Camera.main.transform.position - transform.position);
        var a = animator.GetCurrentAnimatorStateInfo(0);
        if(ToDestroy && a.IsName("Idel"))
        {
            AgentMoveToDestroy();
            ToDestroy = false;
        }
        
            
    }

    public void CustomerInPlace()
    {

        agent.isStopped = true;
        animator.SetBool("Destroy", false);
        animator.SetBool("Walk", false);

        transform.DORotate(new Vector3(0, -180, 0), 2);

        var dis = Vector3.Distance(this.transform.position, Camera.main.transform.position);
        if (dis > 5)
            UI.DOScale(6, 0.1f);
        else
            UI.DOScale(4, 0.1f);
        CalculateTime(shopperSystem.TimeResponseCustomer);
    }
    public void HappyMotion()
    {
        this.tag = "destroy";
        animator.SetBool("Happy", true);
        
        DOVirtual.DelayedCall(2f, () => {
            animator.SetBool("Happy", false);
            
            ToDestroy = true;
        });

      
        // animator.SetBool("Happy", false);
        IsReadyToGiveFruit = false;
        Debug.Log("HAPPY MOTIONNNNNNNNNN");
        //shopperSystem.QueueCapacity++;
    }
    /* public void AngryMotion()
     {
         this.tag = "destroy";
         animator.SetBool("Angry", true);
         DOVirtual.DelayedCall(5, () => {

             AgentMoveToDestroy();

         });
         IsReadyToGiveFruit = false;
       //  shopperSystem.QueueCapacity++;
     }*/
    public void AngryMotionForTime()
    {

        animator.SetBool("Angry", true);
        DOVirtual.DelayedCall(1, () => { animator.SetBool("Angry", false);  });
        Debug.Log("Angry MOTIONNNNNNNNNN");

        //  shopperSystem.QueueCapacity++;
    }
    public void SadMotion()
    {
        animator.SetBool("Sad", true);
        DOVirtual.DelayedCall(2.0f, () => {
            animator.SetBool("Sad", false);
            //animator.SetBool("Destroy", true);
            ToDestroy = true;
        });
        IsReadyToGiveFruit = false;
        Debug.Log("Sad Motion");
    }
    public void ForWhatMotion()
    {
        animator.SetBool("ForWhat", true);
        DOVirtual.DelayedCall(1.20f, () =>
        {
            animator.SetBool("ForWhat", false);
        });

        Debug.Log("For What Motion");
    }
    public void AgentMoveToDestroy()
    {


        var pos = FindObjectOfType<DestroyPlace>().transform.position;
        var pos_spawn_doller = transform.position;
        transform.DOLookAt(pos, 0.5f);
        IsReadyToGiveFruit = false;
        this.tag = "destroy";
        animator.SetBool("Destroy", true);
        animator.SetBool("Walk", true);
        animator.SetBool("Angry", false);
        animator.SetBool("Happy", false);
        animator.SetBool("Sad", false);
        animator.SetBool("ForWhat", false);
        
        agent.isStopped = false;
        agent.destination = pos;


        if (point_offset != -1000)
        {
            DOVirtual.DelayedCall(0.5f, () => { SpawnDollerCash(point_offset, pos_spawn_doller); });
            

        }
        else if (point_offset == -1000)
        {
            shopperSystem.SetHealth();
            Debug.Log("SETT HEALTH:" + point_offset);

        }

        // shopperSystem.CustomerInWave--;
    }




    public void SetDestination(Vector3 pos)
    {
        animator.SetBool("Walk", true);
        animator.SetBool("Destroy", true);
        if (pos.Equals(Vector3.zero))
        {
            pos = shopperSystem.ShopperServicePlace[IDPlace].transform.position;
            // Debug.Log("..........................ZEROOOOOOOOOOOOOOO");
        }
        agent.destination = pos;
        IsReadyToGiveFruit = false;
        //Debug.Log("POS SET");
    }



    public void SetUI(Sprite profile, Sprite fruitIcon, float percent, float timereponse)
    {
        this.Character_image.sprite = profile;
        this.FuritIcon_image.sprite = fruitIcon;
        this.Percent_text.text = percent.ToString("0") + "gr";
        this.NeedPercentValue = percent;
        this.Timeresponse = timereponse;
        this.Prograssbar_time.fillAmount = 1;

        // CalculateTime(timereponse);
    }

    public void PrograssbarAndPointSet(float amount, string fruitname)
    {
        if (IsReadyToGiveFruit == true)
        {
            if (this.fruitname == fruitname)
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
                point_offset = (NeedPercentValue - amount);
                Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA:" + point_offset);
                /// nagative offset mean  Customer is happy
                ///  positive offset mean  Customer is angery
                if (point_offset <= 5)
                {
                    HappyMotion();
                }
                else if (point_offset >= 6)
                {
                    SadMotion();
                }



            }
            else if (this.fruitname != fruitname)
            {
                ForWhatMotion();
            }
        }
    }

    private void SpawnDollerCash(float offset, Vector3 pos)
    {
        var cash_obj = Instantiate(CashPrefab, pos, CashPrefab.transform.rotation);
        var amount = CalculateScore(offset);
        cash_obj.AmountCash = amount;
        Debug.Log("CASH SPAWN :" + amount);
    }


    private float CalculateScore(float point_offset)
    {
        float tempscore = 0.0f;

        if (point_offset <= 5)
        {
            tempscore = shopperSystem.SliceCash;
        }
        else if (point_offset >= 6)
        {
            tempscore = shopperSystem.SliceCash / 2;
        }
        return tempscore;
    }
    private void CalculateTime(float time)
    {
        H = 0;
        M = 0;
        S = 0;
        CancelInvoke("RunTimer");


        H = (float)Math.Floor(time / 3600);
        M = (float)Math.Floor(time / 60 % 60);
        S = (float)Math.Floor(time % 60);
        InvokeRepeating("RunTimer", 0, 1.0f);
        IsReadyToGiveFruit = true;
    }
    /// <summary>
    /// INVOKE IN Calculate
    /// </summary>
    int intergate = 0;
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
            if (intergate == 0)
            {
                AngryMotionForTime();
                intergate = 1;
            }
        }
        if (S == 0 && M == 0 && H == 0)
        {
            ToDestroy = true;
            IsReadyToGiveFruit = false;
            CancelInvoke("RunTimer");
            //  var pos = FindObjectOfType<DestroyPlace>().transform.position;



        }
    }



}
