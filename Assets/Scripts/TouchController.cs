using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;
public class TouchController : MonoBehaviour
{

   // public GameObject PointerObject;
    public GameObject Plane;
    public Cutter cutter;

    private Vector3 point1;
    private Vector3 point2;

    //  private Vector3 point;
    /* public LayerMask maskBackground;
     public LineRenderer line;
     private RaycastHit hit;

     private Ray ray;
     private Vector3 currentPos;
     private Vector3 prev_pos;

     public Vector3 GuidLine;*/
    private void Update()
    {
        Touch();
    }
    private void Touch()
    {
        if(Input.touchCount>0)
        {
            var touch = Input.GetTouch(0);
            Vector3 pos_click;
            if(touch.phase == TouchPhase.Began)
            {
                pos_click = Camera.main.ScreenToWorldPoint(touch.position);
                pos_click.z = 5.0f;
                point1 = pos_click;
            }
            else if(touch.phase == TouchPhase.Moved)
            {

            }
            else if (touch.phase == TouchPhase.Ended)
            {
                pos_click = Camera.main.ScreenToWorldPoint(touch.position);
                pos_click.z = 5.0f;
                point2 = pos_click;
                SetCutPlane(point1, point2, Plane.transform);
                cutter.Cut();
            }
        }
        
    }
    private void SetCutPlane(Vector3 point1, Vector3 point2, Transform plane)
    {
        var dir = point2 - point1;
        plane.position = point1 + (dir / 2.0f);
        plane.localScale = new Vector3(1, 1, 1);
        plane.forward = dir;
    }
}
