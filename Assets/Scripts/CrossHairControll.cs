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
    public float Min_Y = 0;
    public float Max_Y = 0;
    public Vector2 Offset;
    private Tweener tweenr;
    void Start()
    {
        //cross_rect = this.GetComponent<RectTransform>();
    }
    public void SetVisible(bool show)
    {
        tweenr.Kill();
        if (show && canvasGroup.alpha < 1.0f)
            DOVirtual.Float(0, 1, 0.5f, x => { canvasGroup.alpha = x; });
        else if (!show && canvasGroup.alpha > 0.0f)
            DOVirtual.Float(1, 0, 0.5f, x => { canvasGroup.alpha = x; });

    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Vector2 pos = new Vector2();
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(Parent_rect, eventData.position, eventData.enterEventCamera, out pos))
        {
            var temp = pos + Offset;
            cross_rect.anchoredPosition = new Vector2(temp.x, Mathf.Clamp(temp.y, Min_Y, Max_Y));
          

        }

    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos = new Vector2();
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(Parent_rect, eventData.position, eventData.enterEventCamera, out pos))
        {
            var temp = pos + Offset;
            cross_rect.anchoredPosition = new Vector2(temp.x, Mathf.Clamp(temp.y, Min_Y, Max_Y));

        }
    }
}
