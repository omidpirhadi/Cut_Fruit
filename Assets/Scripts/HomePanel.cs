using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HomePanel : MonoBehaviour
{
    public TMPro.TextMeshProUGUI Gold_text;
    public Button Play_button;
    private ShopperSystemController shopperSystem;
    private void OnEnable()
    {
        shopperSystem = FindObjectOfType<ShopperSystemController>();
        Play_button.gameObject.SetActive(false);
        DOVirtual.DelayedCall(2, () => {
            
            Play_button.gameObject.SetActive(true);
            Play_button.interactable = true;
            Play_button.GetComponent<Image>().DOFade(1, 0.5f);
            Play_button.onClick.AddListener(() =>
            {

                StartCoroutine(shopperSystem.StartGame());
                Handler_OnPlay();


            });


        });
        
      
        Gold_text.text = shopperSystem.TotalCash.ToString();
        
    }
    private void OnDisable()
    {
        Play_button.onClick.RemoveAllListeners();
        
    }

    private Action onplay;
    public event Action OnPlay
    {
        add { onplay += value; }
        remove { onplay -= value; }
    }
    protected void Handler_OnPlay()
    {
        if (onplay != null)
        {
            onplay();
        }
    }
}
