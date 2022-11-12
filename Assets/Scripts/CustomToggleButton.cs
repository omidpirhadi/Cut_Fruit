using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Button))]
public class CustomToggleButton : MonoBehaviour
{
    public Image TargetGraphic;
    public Sprite ToggleOn_sprite;
    public Sprite ToggleOff_sprite;

    public bool isOn = true;
    private Button toggleButton;
    private ShopperSystemController shopperSystem;
    public void OnEnable()
    {
        shopperSystem = FindObjectOfType<ShopperSystemController>();
        toggleButton = GetComponent<Button>();
        toggleButton.onClick.AddListener(() => {

            if (isOn == true)
            {
                isOn = false;
                shopperSystem.Mute(false);
                TargetGraphic.sprite = ToggleOff_sprite;
                
            }
            else if(isOn == false)
            {
                
                shopperSystem.Mute(true);
                TargetGraphic.sprite = ToggleOn_sprite;
                isOn = true;
            }
        });
    }
    public void OnDisable()
    {
        toggleButton.onClick.RemoveAllListeners();
    }
}
