using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class InventoryFruitItem : MonoBehaviour
{
    public string FruitName;
    public float Price;
    public float CashSlice;
    public TMPro.TextMeshProUGUI price_text;
    private ShopperSystemController shopperSystem;
    private UnityEngine.UI.Button slot_btn;
  //  [SerializeField] private GameObject ParentUI;
    private void OnEnable()
    {
        if (!shopperSystem)
            shopperSystem = FindObjectOfType<ShopperSystemController>();


        if (shopperSystem.TutorialMode && FruitName != "Apple")
        {
            GetComponentInParent<CanvasGroup>().DOFade(0, 0.2f);
        }
        else
        {
            GetComponentInParent<CanvasGroup>().DOFade(1, 0.2f);
        }


        slot_btn = GetComponent<UnityEngine.UI.Button>();
        slot_btn.onClick.AddListener(() =>
        {
            if (shopperSystem.TutorialMode )
            {
                shopperSystem.HandTutorial.StepTutorial = 5;
                
            }
            StartCoroutine(shopperSystem.SpawFruit(FruitName, Price, CashSlice));
          //  Debug.Log("Fruit Button Click");
        });
        if (Price <= shopperSystem.TotalCash)
            price_text.color = Color.green;
        else
            price_text.color = Color.red;
       
        price_text.text = Price.ToString();
    }
    private void OnDisable()
    {
        slot_btn.onClick.RemoveAllListeners();
    }
}
