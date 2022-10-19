using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;

public class GlowButton : MonoBehaviour
{
    public Ease ease;
    public LoopType loop;
    public float duration;
    private new Animation animation;
    private void Start()
    {
        animation = GetComponent<Animation>();
        transform.DORotate(new Vector3(0, 0, 180f), duration).SetEase(ease).SetLoops(-1, loop);
    }
    public void GlowPos(string name)
    {
        animation = GetComponent<Animation>();
        animation.Play(name, PlayMode.StopAll);
    }
}
