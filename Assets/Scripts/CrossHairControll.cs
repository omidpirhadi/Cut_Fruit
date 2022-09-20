using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using DG.Tweening;
public class CrossHairControll : MonoBehaviour, IPointerClickHandler,IDragHandler
{

    public  RectTransform Parent_rect;
    public RectTransform cross_rect;
    public CanvasGroup canvasGroup;
    public Vector2 Offset;
    void Start()
    {
        //cross_rect = this.GetComponent<RectTransform>();
    }
    public void SetVisible(bool show)
    {
        if (show && canvasGroup.alpha < 0.99f)
            DOVirtual.Float(0, 1, 0.5f, x => { canvasGroup.alpha = x; });
        else if (!show && canvasGroup.alpha > 0.99f)
            DOVirtual.Float(1, 0, 0.5f, x => { canvasGroup.alpha = x; });

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
            cross_rect.anchoredPosition = pos + Offset;

        }
    }
}
