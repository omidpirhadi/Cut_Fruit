using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class TutorialHand : MonoBehaviour
{


    public int StepTutorial = -1;
    
    public RectTransform CutButton_pos;
    public RectTransform PickupButton_pos;
    public RectTransform ShopButton_pos;

    private Image HandImage;
    private new Animation animation;

    private ShopperSystemController shopperSystem;
    private DialogBox dialog;
    private void Start()
    {
      //  StartCoroutine(Tutorial());
    }
    public IEnumerator Tutorial()
    {
        if (animation == null)
            animation = GetComponent<Animation>();
        if (HandImage == null)
            HandImage = GetComponent<Image>();
        if (dialog == null)
            dialog = FindObjectOfType<DialogBox>();
        if (shopperSystem == null)
            shopperSystem = FindObjectOfType<ShopperSystemController>();

        var hand_rect = HandImage.GetComponent<RectTransform>();
        hand_rect.DOMove(CutButton_pos.position, 1.0f);
        HandImage.DOFade(1, 0.5f);
        
        animation.Play("ShockOnButton", PlayMode.StopAll);
        dialog.SetPositionWithAnimation("Cut_POS_Button");
        dialog.Set("Click On Cut Button ", 10);
       
        Debug.Log("Step1");

        yield return new WaitUntil(() => StepTutorial == 0);

        animation.Play("DoCut", PlayMode.StopAll);
        dialog.SetPositionWithAnimation("CutDrag");
        dialog.Set("Drag Finger On Fruit For Cut ", 10);
        Debug.Log("Setp 2");

        yield return new WaitUntil(() => StepTutorial == 1);

        hand_rect.DOMove(PickupButton_pos.position, 1.0f);
        animation.Play("ShockOnButton", PlayMode.StopAll);
        dialog.SetPositionWithAnimation("Pickup_POS_Button");
        dialog.Set("Click On PickUp Button ", 10);
        Debug.Log("Setp 3");

        yield return new WaitUntil(() => StepTutorial == 2);

        animation.Play("DoDrag", PlayMode.StopAll);
        dialog.SetPositionWithAnimation("CutDrag");
        dialog.Set("Drag Fruit On Customer", 10);
        Debug.Log("Setp 4");

        yield return new WaitUntil(() => StepTutorial == 3);

        hand_rect.DOMove(ShopButton_pos.position, 1.0f);
        animation.Play("ShockOnButton", PlayMode.StopAll);
        dialog.SetPositionWithAnimation("Shop_POS_Button");
        dialog.Set("Click Shop Button", 10);
        Debug.Log("Setp 5");

        yield return new WaitUntil(() => StepTutorial == 4);


        animation.Play("SellFruit", PlayMode.StopAll);
        DOVirtual.DelayedCall(1, () => { animation.Play("ShockOnButton", PlayMode.StopAll); });
        dialog.SetPositionWithAnimation("Inventory_POS");
        dialog.Set("Buy Fruit", 10);
        Debug.Log("Setp 5");

        yield return new WaitUntil(() => StepTutorial == 5);


        //yield return new WaitUntil(() => StepTutorial == 6);
        dialog.SetPositionWithAnimation("CashMony_POS");
        dialog.Set("Excellent And DontForget Pickup Cash Item", 5);

        HandImage.DOFade(0, 0.5f);
       
        DOVirtual.DelayedCall(5, () => {
            shopperSystem.SetLeaderboard();

            shopperSystem.SaveLeaderboard("fruitshop");
            shopperSystem.Pause_Button.interactable = true;
            this.gameObject.SetActive(false);

        });
        Debug.Log("Setp 6");
    }
}
