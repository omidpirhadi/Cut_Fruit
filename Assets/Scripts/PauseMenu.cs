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
        shopperSystem.GamePlayed = false;
        Home_button.onClick.AddListener(() => {
            Time.timeScale = 1;
            StartCoroutine(shopperSystem.EndGame());
            
        });
        Resume_button.onClick.AddListener(() => {
            Time.timeScale = 1;
            shopperSystem.GamePlayed = true;
            shopperSystem.HUDPanel.SetActive(true);
            this.gameObject.SetActive(false);
        });
        Gold_text.text = shopperSystem.TotalCash.ToString();
        DOVirtual.DelayedCall(0.2f, () => { Time.timeScale = 0; });
    }
    private void OnDisable()
    {
        Time.timeScale = 1;
        Home_button.onClick.RemoveAllListeners();
        Resume_button.onClick.RemoveAllListeners();
    }
}
