using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialHand : MonoBehaviour
{


    public int StepTutorial = 0;

    public RectTransform CutButton_pos;
    public RectTransform PickupButton_pos;
    public RectTransform ShopButton_pos;

    private new Animation animation;

    public IEnumerator Tutorial()
    {
        animation = GetComponent<Animation>();
        animation.Play("cut", PlayMode.StopAll);
        yield return new WaitUntil(() => StepTutorial == 0);

        yield return new WaitUntil(() => StepTutorial == 1);

        yield return new WaitUntil(() => StepTutorial == 2);
    }
}
