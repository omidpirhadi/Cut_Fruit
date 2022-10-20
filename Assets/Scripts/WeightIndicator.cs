using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
public class WeightIndicator : MonoBehaviour
{
   public  TextMeshPro Context;
    public Ease ease;
    public LoopType loop;
    public float Duration;
    public float TimeDestroy;
    public Transform Direction;
    public void Set(string precent)
    {
        Context.text = precent+"gr";
        this.transform.DOMove(Direction.position , Duration).SetEase(ease).SetLoops(-1, loop);
        this.transform.localEulerAngles = Vector3.zero;
        this.transform.eulerAngles = Vector3.zero;
        Destroy(this.gameObject, TimeDestroy);
    }
}
