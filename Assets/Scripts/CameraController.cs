using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Sirenix.OdinInspector;
public class CameraController : MonoBehaviour
{


    public GameObject CamShop;
    public GameObject CamCut;
    public Image Fade_image;
    public float FadeDuration = 1;
    [Button("Switch")]
    public void SwitchCamera(int index)
    {
        var color = new Color();
        color = Fade_image.color;
        DOVirtual.Float(0, 1, FadeDuration, (alpha) =>
        {

            color.a = alpha;
            Fade_image.color = color;
        }).OnComplete(() =>
        {

            DOVirtual.Float(1, 0, FadeDuration, (alpha) =>
            {

                color.a = alpha;
                Fade_image.color = color;
            });
            if (index == 0)
            {

                CamCut.tag = "Untagged";
                CamShop.tag = "MainCamera";
                
                CamShop.SetActive(true);
                CamCut.SetActive(false);
            }
            else
            {
                CamShop.tag = "Untagged";
                CamCut.tag = "MainCamera";
               
                CamShop.SetActive(false);
                CamCut.SetActive(true);
            }
        });
      

    }
}
