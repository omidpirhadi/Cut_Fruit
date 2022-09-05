using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
public class CrossHairControll : MonoBehaviour, IPointerClickHandler,IDragHandler
{

    public  RectTransform Parent_rect;
    public RectTransform cross_rect;
    void Start()
    {
        //cross_rect = this.GetComponent<RectTransform>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Vector2 pos = new Vector2();
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(Parent_rect, eventData.position, eventData.enterEventCamera, out pos))
        {
            cross_rect.anchoredPosition = pos;

        }

    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos = new Vector2();
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(Parent_rect, eventData.position, eventData.enterEventCamera, out pos))
        {
            cross_rect.anchoredPosition = pos;

        }
    }
}
