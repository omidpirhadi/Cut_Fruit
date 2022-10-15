using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
public class Money : MonoBehaviour
{
    public float AmountCash;
    [SerializeField] float SpeedRotate = 0.5f;
    [SerializeField] Vector3 Angel;
    [SerializeField] Ease easeType;
    [SerializeField] LoopType loopType;
    private ShopperSystemController shopperSystem;
    void Start()
    {
        shopperSystem = FindObjectOfType<ShopperSystemController>();
        transform.DORotate(Angel, SpeedRotate).SetEase(easeType).SetLoops(-1, loopType);
    }

    public void ReciveCash()
    {
        shopperSystem.AmountCash(AmountCash);
        Destroy(this.gameObject);
    }
    [Button("Set",ButtonSizes.Medium)]
    private void Set()
    {
        transform.DORotate(Angel, SpeedRotate).SetEase(easeType).SetLoops(-1, loopType);
    }

}
