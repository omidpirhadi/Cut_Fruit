using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.EventSystems;
using TMPro;
public class ShopperIndicatorUI : MonoBehaviour
{
    public Image Character_image;
    public TMPro.TMP_Text Percent_text;
    public Image FuritIcon_image;
    public Image Prograssbar;
    public float NeedPercentValue;
   // private ShopperSystemController shopperSystem;
    private void Start()
    {
      //  shopperSystem = FindObjectOfType<ShopperSystemController>();
    }


    public void Set(Sprite profile, Sprite fruitIcon, float percent  )
    {
        this.Character_image.sprite = profile;
        this.FuritIcon_image.sprite = fruitIcon;
        Percent_text.text = percent.ToString("0")+"%";
        this.NeedPercentValue = percent;
       
    }
    public void PrograssbarSet(float amount)
    {
        var unit = amount / NeedPercentValue;

        Prograssbar.fillAmount += unit;
        if(Prograssbar.fillAmount>0.5)
        {
            Prograssbar.color = Color.green;
        }
        else if (Prograssbar.fillAmount >0.25 && Prograssbar.fillAmount < 0.5)
        {
            Prograssbar.color = Color.yellow;
        }
        else if (Prograssbar.fillAmount > 0 && Prograssbar.fillAmount < 0.25)
        {
            Prograssbar.color = Color.red;
        }
      //  this.Percent_text.text = (Mathf.Clamp(amount / NeedPercentValue, 0, 1) * 100).ToString("0");
    }
}
