using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class DialogBox : MonoBehaviour
{
    public TMPro.TMP_Text Context;
    
    public void Set(string context , float duration = 5)
    {
      
        this.Context.text = context;
        this.gameObject.SetActive(true);
      DOVirtual.DelayedCall(duration, () => { Hide(); });
    }
    public void Hide()
    {

        this.gameObject.SetActive(false);
    }
}
