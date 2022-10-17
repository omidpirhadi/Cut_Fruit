using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class PauseMenu : MonoBehaviour
{
    public TMPro.TextMeshProUGUI Gold_text;
    public Button Home_button;
    public Button Resume_button;


    private ShopperSystemController shopperSystem;
    private void OnEnable()
    {
        shopperSystem = FindObjectOfType<ShopperSystemController>();
       
        Home_button.onClick.AddListener(() => {
            Time.timeScale = 1;
            StartCoroutine(shopperSystem.EndGame());
            
        });
        Resume_button.onClick.AddListener(() => {
            Time.timeScale = 1;
            this.gameObject.SetActive(false);
        });
        Gold_text.text = shopperSystem.TotalCash.ToString();
        DOVirtual.DelayedCall(1, () => { Time.timeScale = 0; });
    }
    private void OnDisable()
    {
        Home_button.onClick.RemoveAllListeners();
        Resume_button.onClick.RemoveAllListeners();
    }
}
