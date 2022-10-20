using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Btn : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Canvas;

    public void PanelSag()
    {
        if(Canvas != null)
        {
            Canvas.SetActive(true);
        }
    }
}
