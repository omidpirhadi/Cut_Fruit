using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class DialogBox : MonoBehaviour
{
    public TMPro.TMP_Text Context;
    private CanvasGroup CanvasGroup;
    private Tween t;
    public void Set(string context, float duration = 5)
    {
        CanvasGroup = GetComponent<CanvasGroup>();
        Show();
        this.Context.text = context;
        this.gameObject.SetActive(true);
        if (t != null && t.IsComplete())
        {
            t = DOVirtual.DelayedCall(duration, () => { Hide(); });
        }
        else
        {
            t.Kill();
            t = DOVirtual.DelayedCall(duration, () => { Hide(); });
        }

    }
    public void Hide()
    {

        CanvasGroup.DOFade(0, 0.5f);
    }
    public void Show()
    {

        CanvasGroup.DOFade(1, 0.5f);
    }
}
