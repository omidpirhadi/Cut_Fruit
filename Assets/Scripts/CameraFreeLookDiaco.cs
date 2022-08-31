using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
public class CameraFreeLookDiaco : MonoBehaviour
{

    private Transform Camera;
    public bool InvertY = false;
    public float Sensivity = 0.5f;
 
    public float Min_X;
   
    public float Max_X;
 
    public float Min_Y;
 
    public float Max_Y;

    private float rot_x;
    private float rot_y;
    public void Start()
    {
        
    }
    private void Update()
    {
        Touch();
    }
    private void Touch()
    {
        if (Input.touchCount > 0)
        {


            var touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {

               

            }
            else if (touch.phase == TouchPhase.Moved)
            {

                Vector3 deltamove = touch.deltaPosition.normalized;

                if (InvertY)
                {
                    rot_x += deltamove.y * Sensivity;
                    rot_y += deltamove.x * Sensivity;
                    rot_x = Mathf.Clamp(rot_x, Min_X, Max_X);
                    rot_y = Mathf.Clamp(rot_y, Min_Y, Max_Y);
                }
                else
                {
                    rot_x += -deltamove.y * Sensivity;
                    rot_y += deltamove.x * Sensivity;
                    rot_x = Mathf.Clamp(rot_x, Min_X, Max_X);
                    rot_y = Mathf.Clamp(rot_y, Min_Y, Max_Y);
                }
                transform.eulerAngles = new Vector3(rot_x, rot_y, 0);
           
  

               // Debug.Log(touch.deltaPosition);
            }
            else if (touch.phase == TouchPhase.Ended)
            {
 
                
            }
        }

    }
}
