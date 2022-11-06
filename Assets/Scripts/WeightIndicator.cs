using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using Sirenix.OdinInspector;
public class WeightIndicator : MonoBehaviour
{
   public  TextMeshPro Context;
    public Ease ease;
    public LoopType loop;
    public float Duration;
    public float TimeDestroy;
   // public Transform Direction;
    [Button("MoveTest")]
    public void SetForFruit(string precent)
    {
        Context.text = precent+"gr";

        this.transform.DOMoveY(5 , Duration).SetEase(ease).SetLoops(-1, loop);
        Context.DOFade(0, 1).SetEase(ease).SetLoops(-1, loop);

        this.transform.localEulerAngles = Vector3.zero;
        this.transform.eulerAngles = Vector3.zero;
        Destroy(this.gameObject, TimeDestroy);
    }
    public void SetForCash(string amountcash)
    {
        Context.text = amountcash + "$";
        this.transform.DOMove((transform.position), Duration).SetEase(ease).SetLoops(-1, loop);
        this.transform.localEulerAngles = Vector3.zero;
        this.transform.eulerAngles = Vector3.zero;
        Destroy(this.gameObject, TimeDestroy);
    }
}
