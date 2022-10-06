using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Money : MonoBehaviour
{
    [SerializeField] float SpeedRotate = 0.5f;
    [SerializeField] Ease easeType;
    [SerializeField] LoopType loopType;
    void Start()
    {
        transform.DORotate(Vector3.up, SpeedRotate).SetEase(easeType).SetLoops(-1, loopType);
    }


}
