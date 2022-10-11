using System;
using System.Linq;
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

    private NavMeshAgent agent;
    private Animator animator;
    private ShopperSystemController shopperSystem;


    private float Timeresponse;
    private float H;
    private float M;
    private float S;

   [SerializeField] private bool IsReadyToGiveFruit = false;
    void Start()
    {

        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        shopperSystem = FindObjectOfType<ShopperSystemController>();
      //  shopperSystem.OnAgentMove += ShopperSystem_OnAgentMove;
    }
    private void LateUpdate()
    {
        UI.forward = -(Camera.main.transform.position - transform.position);
    }
    private void OnDestroy()
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
        IsReadyToGiveFruit = false;
        //shopperSystem.QueueCapacity++;
    }
    public void AngryMotion()
    {
        this.tag = "destroy";
        animator.SetBool("Angry", true);
        DOVirtual.DelayedCall(5, () => {
            var pos = FindObjectOfType<DestroyPlace>().transform.position;
            AgentMoveToDestroy(pos);
        });
        IsReadyToGiveFruit = false;
      //  shopperSystem.QueueCapacity++;
    }

    public void ForWhatMotion()
    {
        animator.SetBool("Sad", true);
        DOVirtual.DelayedCall(2, () => {
            var pos = FindObjectOfType<DestroyPlace>().transform.position;
            AgentMoveToDestroy(pos);
        });
        IsReadyToGiveFruit = false;
        Debug.Log("For What Motion");
    }
    public void AgentMoveToDestroy(Vector3 pos)
    {
        this.tag = "destroy";
        animator.SetBool("Angry", false);
        animator.SetBool("Happy", false);
        animator.SetBool("Walk", true);
        agent.isStopped = false;
        agent.destination = pos;
        IsReadyToGiveFruit = false;
        // shopperSystem.CustomerInWave--;
    }
   
 
 

    public void SetDestination(Vector3 pos)
    {
       animator.SetBool("Walk", true);
        agent.destination = pos;
        IsReadyToGiveFruit = false;
        //Debug.Log("POS SET");
    }



    public void SetUI(Sprite profile, Sprite fruitIcon, float percent , float timereponse)
    {
        this.Character_image.sprite = profile;
        this.FuritIcon_image.sprite = fruitIcon;
        this.Percent_text.text = percent.ToString("0") + "%";
        this.NeedPercentValue = percent;
        this.Timeresponse = timereponse;
        this.Prograssbar_time.fillAmount = 1;
        if (IDPlace > 1)
            UI.DOScale(5, 0.1f);
        else
            UI.DOScale(4, 0.1f);
        // CalculateTime(timereponse);
    }

    public void PrograssbarAndPointSet(float amount , string fruitname)
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
                var point_offset = (NeedPercentValue - amount);
                /// nagative offset mean  Customer is happy
                ///  positive offset mean  Customer is angery
                if (point_offset <= 5)
                {
                    HappyMotion();
                }
                else if (point_offset >= 6)
                {
                    AngryMotion();
                }

                SpawnDollerCash(point_offset);
            }
            else
            {
                ForWhatMotion();
            }
        }
    }

    private void SpawnDollerCash(float offset)
    {
        var cash_obj = Instantiate(CashPrefab, transform.position, CashPrefab.transform.rotation);
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
        IsReadyToGiveFruit = true;
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
            IsReadyToGiveFruit = false;
            CancelInvoke("RunTimer");
          //  var pos = FindObjectOfType<DestroyPlace>().transform.position;
            AngryMotion();


        }
    }


    
}
