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
    public LayerMask RayCastLayar;
    public LayerMask MaskFruit;
    private Cutter cutter;
    private LineRenderer line;
    private RaycastHit hit;
    private Ray ray;
    [SerializeField] public List<GameObject> SelectedFruits;
    [SerializeField] public List<Material> SelectedInnerMatrials;
    private Vector3 point1;
    private Vector3 point2;
    private Vector3 pos_click;
    private DragAndDropItem DragItem;
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
        DragItem = FindObjectOfType<DragAndDropItem>();
    }
    private void FuritSliceManager_ChangeMode(bool cut, bool pick)
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
        if (Input.touchCount > 0)
        {


            var touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                ray = Camera.main.ScreenPointToRay(touch.position);

                if (IsReadyForCut)
                {
                    if (Physics.Raycast(ray, out hit, 10, RayCastLayar))
                    {
                        SelectedFruits.Clear();
                        SelectedInnerMatrials.Clear();
                        line.positionCount = 2;
                        point1 = hit.point;
                        line.SetPosition(0, point1);
                        line.SetPosition(1, point1);
                      //  Debug.Log(hit.collider.name);
                    }
                }
                else
                {
                    if (Physics.Raycast(ray, out hit, 10, MaskFruit))
                    {
                        var f = hit.collider.GetComponent<Furit>();
                        var f_p = hit.collider.GetComponent<FruitPiece>();
                        if (f)
                        {
                            DragItem.FuritPercent = f.PercentVolume;
                            DragItem.FruitSliceRefrence = f.gameObject;
                          //  Debug.Log("Fruit Data:" + f.Volume);
                        }
                        else if (f_p)
                        {
                            DragItem.FuritPercent = f_p.PercentVolume;
                            DragItem.FruitSliceRefrence = f_p.gameObject;
                            // Debug.Log("Fruit Piece Data:" + f_p.PercentVolume);
                        }
                    }
                }


            }
            else if (touch.phase == TouchPhase.Moved)
            {
                ray = Camera.main.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray, out hit, 10, RayCastLayar))
                {
                    if (IsReadyForCut)
                    {


                        point2 = hit.point;
                        line.SetPosition(0, point1);
                        line.SetPosition(1, point2);
                      //    Debug.Log(hit.collider.name);

                    }
                    else
                    {

                    }
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                ray = Camera.main.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray, out hit, 10, RayCastLayar))
                {
                    if (IsReadyForCut)
                    {


                        // pos_click.z = 10.0f;
                        point2 = hit.point;
                        selector.RayFire(point1, point2);



                        cutter.SetCutPlane(point1, point2, 1f);
                        CutFruits();
                        line.positionCount = 0;


                    }
                    else
                    {

                    }
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
        if (!SelectedFruits.Contains(fruit))
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
        //    Debug.Log($"Add Furit :{fruit} ");
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
