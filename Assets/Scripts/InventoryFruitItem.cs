﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryFruitItem : MonoBehaviour
{
    public string FruitName;
    public float Price;
    public float CashSlice;
    public TMPro.TextMeshProUGUI price_text;
    private ShopperSystemController shopperSystem;
    private UnityEngine.UI.Button slot_btn;
    private void OnEnable()
    {
        if (!shopperSystem)
            shopperSystem = FindObjectOfType<ShopperSystemController>();
        slot_btn = GetComponent<UnityEngine.UI.Button>();
        slot_btn.onClick.AddListener(() => {
            Debug.Log("Fruit Button Click");
            StartCoroutine(shopperSystem.SpawFruit(FruitName, Price, CashSlice));

        });
        price_text.text = Price.ToString();
    }
    private void OnDisable()
    {
        slot_btn.onClick.RemoveAllListeners();
    }
}
