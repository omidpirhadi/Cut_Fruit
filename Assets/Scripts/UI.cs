
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UI : MonoBehaviour
{
    public Button Cut_btn;
    public Button PickUp_btn;
    private void Start()
    {
        Cut_btn.onClick.AddListener(() => {

            Handler_ChangeMode(true,false);
        });
        PickUp_btn.onClick.AddListener(() => {

            Handler_ChangeMode(false,true);
        });
    }

    private Action<bool, bool> changemode;
    public event Action<bool , bool> ChangeMode
    {
        add { changemode += value; }
        remove { changemode -= value; }
    }
    protected void Handler_ChangeMode(bool cut , bool pick)
    {
        if(changemode != null)
        {
            changemode(cut, pick);
        }
    }
}
