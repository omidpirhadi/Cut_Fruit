using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Selector : MonoBehaviour
{

    public TouchController controller;
    public void RayFire(Vector3 start, Vector3 end)
    {
        transform.position = start;
        transform.DOMove(end, 0.2f).OnComplete(() => { controller.CutFruits(); });
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "furit")
        {
            controller.FruitSelect(other.gameObject);
            Debug.Log(other.name);
        }
       
    }
}
