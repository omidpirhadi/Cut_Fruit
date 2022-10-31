using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScreen : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public void SetAlpha(float a)
    {
        if(canvasGroup  == null)
            canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = a;
    }
}
