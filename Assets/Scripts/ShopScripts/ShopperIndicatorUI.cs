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
    public float PercentValue;
    public void OnDrop(PointerEventData eventData)
    {
        var item = FindObjectOfType<DragAndDropItem>();
        var percent_furit = item.FuritPercent;

        if (percent_furit>PercentValue)
        {
            Destroy(item.FruitSliceRefrence);
            FindObjectOfType<ShopperSystemController>().CalculateScore(10, 10);
            this.gameObject.SetActive(false);
            
            Debug.Log("WellDown");
        }
        else
        {
            Debug.Log("No");
        }
    }

    public void Set(Sprite profile, Sprite fruitIcon, float percent  )
    {
        this.Character_image.sprite = profile;
        this.FuritIcon_image.sprite = fruitIcon;
        this.Percent_text.text = "%" + percent;
        this.PercentValue = percent;
       
    }
}
