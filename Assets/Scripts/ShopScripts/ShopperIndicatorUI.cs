using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.EventSystems;
using TMPro;
public class ShopperIndicatorUI : MonoBehaviour,IDropHandler
{
    public Image Character_image;
    public TMPro.TMP_Text Percent_text;
    public Image FuritIcon_image;

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("HELLO");
    }

    public void Set(Sprite profile, Sprite fruitIcon, int percent  )
    {
        this.Character_image.sprite = profile;
        this.FuritIcon_image.sprite = fruitIcon;
        this.Percent_text.text = "%" + percent;
       
    }
}
