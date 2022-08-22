using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;
//using GameAnalyticsSDK;
//using GoogleMobileAds.Api;
using UnityEngine.UI;
public class TouchController : MonoBehaviour
{
    public Text log;
    public Selector selector;
    public bool IsReadyForCut = true;
    public LayerMask MaskFruit;
    private Cutter cutter;
    private LineRenderer line;

  [SerializeField]  public List<GameObject> SelectedFruits;
   [SerializeField] public List<Material> SelectedInnerMatrials;
    private Vector3 point1;
    private Vector3 point2;
    private Vector3 pos_click;
    private void Awake()
    {
     

    }
    private void Start()
    {
       

        

        FindObjectOfType<UI>().ChangeMode += FuritSliceManager_ChangeMode;
        cutter = GetComponent<Cutter>();
        line = GetComponent<LineRenderer>();
        SelectedFruits = new List<GameObject>();
        SelectedInnerMatrials = new List<Material>();
        line.positionCount = 2;
    }
    private void FuritSliceManager_ChangeMode(bool cut ,  bool pick)
    {
        IsReadyForCut = cut;
    }
    private void Update()
    {
        Touch();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(point1, point2);
      //  DebugExtension.DrawCapsule(point1, point2, Color.green, 0.5f);

       
    }
    private void Touch()
    {
        if(Input.touchCount>0)
        {

           
            var touch = Input.GetTouch(0);
            
            if(touch.phase == TouchPhase.Began)
            {
                if (IsReadyForCut)
                {
                    
                    line.positionCount = 2;

                    SelectedFruits.Clear();
                    SelectedInnerMatrials.Clear();
                    pos_click = Camera.main.ScreenToWorldPoint(touch.position);
                   // pos_click.z = 10.0f;
                    point1 = pos_click;
                    line.SetPosition(0, point1);
                    line.SetPosition(1, point1);
                }
            }
            else if(touch.phase == TouchPhase.Moved)
            {

                if (IsReadyForCut)
                {
                    pos_click = Camera.main.ScreenToWorldPoint(touch.position);
                    pos_click.z = 11.0f;
                    point2 = pos_click;

                  
                    line.SetPosition(1, point2);
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                if (IsReadyForCut)
                {
                    
                    pos_click = Camera.main.ScreenToWorldPoint(touch.position);
                   // pos_click.z = 10.0f;
                    point2 = pos_click;
                    selector.RayFire(point1, point2);



                    cutter.SetCutPlane(point1, point2, 1f);
                //    CutFruits();
                    line.positionCount = 0;
                   

                }
            }
        }
        
    }
    public void CutFruits()
    {
        for (int i = 0; i < SelectedFruits.Count; i++)
        {
            cutter.Cut(SelectedFruits[i], SelectedInnerMatrials[i]);
        }
        SelectedFruits.Clear();
        SelectedInnerMatrials.Clear();
    }
    public void FruitSelect(GameObject fruit)
    {
        if (!SelectedFruits.Contains(fruit) )
        {
            SelectedFruits.Add(fruit);
            if (fruit.GetComponent<Furit>())
            {
                SelectedInnerMatrials.Add(fruit.GetComponent<Furit>().InnerMatrialAfterCut);
            }
            else
            {
                SelectedInnerMatrials.Add(fruit.GetComponent<FruitPiece>().InnerMatrialAfterCut);
            }
            Debug.Log($"Add Furit :{fruit} ");
        }
       /* else
        {
            SelectedFruits.Remove(fruit);
            SelectedInnerMatrials.Remove(innerMatrialAfterCut);
            Debug.Log($"Removed Furit :{fruit} Matrial{ innerMatrialAfterCut.name}");

        }*/
    }

   // 

}
