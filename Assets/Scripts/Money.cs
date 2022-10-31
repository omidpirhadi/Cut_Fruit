using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
public class Money : MonoBehaviour
{
    public float AmountCash;
    [SerializeField] float GrowSize = 2;
    [SerializeField] float SpeedGrow = 0.5f;
    [SerializeField] float SpeedRotate = 0.5f;
    [SerializeField]  Vector3 Angel;
    [SerializeField] Ease easeType;
    [SerializeField] LoopType loopType;
    private ShopperSystemController shopperSystem;
    private MoneyParticleController particelMoney;
    void Start()
    {
        Angel = new Vector3();
       // easeType = new Ease();
        shopperSystem = FindObjectOfType<ShopperSystemController>();
        particelMoney = shopperSystem.GetComponent<MoneyParticleController>();
        transform.DORotate(Angel, SpeedRotate).SetEase(easeType).SetLoops(-1, loopType);
        DOVirtual.DelayedCall(1f, () => { transform.DOScale(GrowSize, SpeedGrow).SetEase(easeType).SetLoops(1, loopType); });

    }

    public void ReciveCash()
    {
        shopperSystem.AmountCash(AmountCash);

       particelMoney.StartEmit(transform.position);

        Destroy(this.gameObject);
    }


}
