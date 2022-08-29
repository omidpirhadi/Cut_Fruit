using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Selector : MonoBehaviour
{

    public TouchController controller;
    public void RayFire(Vector3 start, Vector3 end)
    {
       // start.z = -4.65f;

        transform.position = start;

       // end.z = -4.65f;

        transform.DOMove(end, 0.2f).OnComplete(() => { controller.CutFruits(); });
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "furit")
        {
            controller.FruitSelect(other.gameObject);
           // Debug.Log(other.name);
        }
       
    }
}
