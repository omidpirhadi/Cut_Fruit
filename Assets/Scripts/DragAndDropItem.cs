using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class DragAndDropItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{


    public GameObject FruitSliceRefrence;
    public float FuritPercent = 0;
    public Image Icon_image;
    public TMPro.TMP_Text Percent_text;
    public void OnBeginDrag(PointerEventData eventData)
    {

       // Debug.Log("Begin" +"%"+ FuritPercent);
    }

    public void OnDrag(PointerEventData eventData)
    {
       
       // Debug.Log("Drag");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
    //   Debug.Log("End");
    }




}
