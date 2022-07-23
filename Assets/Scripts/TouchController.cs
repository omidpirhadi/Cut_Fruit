using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;
public class TouchController : MonoBehaviour
{

    // public GameObject PointerObject;
    //public GameObject Plane;
    public LayerMask MaskFruit;
    private Cutter cutter;
    private LineRenderer line;

    private List<GameObject> SelectedFruits;
    private Vector3 point1;
    private Vector3 point2;
    private Vector3 pos_click;
   // private Ray ray;
   // private RaycastHit hit;

    //  private Vector3 point;
    /* 
     
     

     
     private Vector3 currentPos;
     private Vector3 prev_pos;

     public Vector3 GuidLine;*/
    private void Start()
    {
        cutter = GetComponent<Cutter>();
        line = GetComponent<LineRenderer>();
        SelectedFruits = new List<GameObject>();
        line.positionCount = 2;
    }
    private void Update()
    {
        Touch();
    }
    private void Touch()
    {
        if(Input.touchCount>0)
        {

           
            var touch = Input.GetTouch(0);
            
            if(touch.phase == TouchPhase.Began)
            {
                line.positionCount = 2;
                
                SelectedFruits.Clear();
                pos_click = Camera.main.ScreenToWorldPoint(touch.position);
                pos_click.z = 10.0f;
                point1 = pos_click;
                line.SetPosition(0, point1);
                line.SetPosition(1, point1);
            }
            else if(touch.phase == TouchPhase.Moved)
            {
                /* ray = Camera.main.ScreenPointToRay(touch.position); 
                 if(Physics.Raycast(ray,out hit, 100,MaskFruit))
                 {
                     if (hit.collider != null)
                     {
                         FruitSelect(hit.collider.gameObject);
                     }
                 }*/
                pos_click = Camera.main.ScreenToWorldPoint(touch.position);
                pos_click.z = 10.0f;
                point2 = pos_click;
                line.SetPosition(1, point2);
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                pos_click = Camera.main.ScreenToWorldPoint(touch.position);
                pos_click.z = 10.0f;
                point2 = pos_click;
                line.positionCount = 0;
                cutter.SetCutPlane(point1, point2);

                CutFruits();
            }
        }
        
    }
    private void CutFruits()
    {
        for (int i = 0; i < SelectedFruits.Count; i++)
        {
            cutter.Cut(SelectedFruits[i]);
        }
    }
    public void FruitSelect(GameObject fruit)
    {
        if (!SelectedFruits.Contains(fruit))
        {
            SelectedFruits.Add(fruit);
          //  Debug.Log($"Add Furit :{fruit}");
        }
        else
        {
            SelectedFruits.Remove(fruit);
          //  Debug.Log($"Removed Furit :{fruit}");

        }
    }
}
