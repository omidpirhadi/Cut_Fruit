using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Sirenix.OdinInspector;
public class MoneyParticleController : MonoBehaviour
{

    public Transform ParentCanvas;
    public RectTransform MoneyInCanvas_Prefab;
  //  public Vector3 worldPositionMoneyBox;
    public Transform EndTargetInCanvas;

    public MinMax SizeByLifeTime;
    public int ParticleCount = 10;
    public float DistanceBetweenEmit = 0.1f;
    public float SpeedMove = 0.1f;
    public Ease MoveEase;
    
    public void StartEmit( Vector3 startpos)
    {
        StartCoroutine(Emit(startpos));
    }
    private Sequence sequence;
    private IEnumerator Emit(Vector3 worldpos)
    {
       
        for (int i = 0; i < ParticleCount; i++)
        {
            var screen_pos = Camera.main.WorldToScreenPoint(worldpos);
            var money = Instantiate(MoneyInCanvas_Prefab, ParentCanvas);
            money.position = screen_pos;
            money.DOMove(EndTargetInCanvas.position, SpeedMove).OnComplete(() => {

                Destroy(money.gameObject);
            }).SetEase(MoveEase);
            money.DOScale(SizeByLifeTime.Max, SpeedMove).SetEase(MoveEase);
            yield return new WaitForSecondsRealtime(DistanceBetweenEmit);
        }
       
    }

}

[Serializable]
public struct MinMax
{
  
    public float Min;
    
    public float Max;
}
