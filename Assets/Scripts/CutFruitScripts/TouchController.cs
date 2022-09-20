using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;
using DG.Tweening;
//using GameAnalyticsSDK;
//using GoogleMobileAds.Api;
using UnityEngine.UI;
public class TouchController : MonoBehaviour
{
   // public Transform ttt;
   // public Text log;
    public Selector selector;
    public bool IsReadyForCut = true;
    public bool IsTouchReady = true;
    public LayerMask MaskForCut;
    public LayerMask MaskForFruit;
    public LayerMask MaskForHumen;
    private Cutter cutter;
    private LineRenderer line;
    private RaycastHit hit;
    private Ray ray;
    [SerializeField] public List<GameObject> SelectedFruits;
    [SerializeField] public List<Material> SelectedInnerMatrials;
    private Vector3 point1;
    private Vector3 point2;
    private Vector3 pos_click;

    private CrossHairControll crossHair;
   // private DragAndDropItem DragItem;
    private ShopperSystemController shopperSystem;

    public GameObject fruit_slice;
    private float precent_fruit;
    private Vector3 FirstPosBeforSelect;
    private Vector3 offsetOfSelect;
    private  Rigidbody rigidbody_selected_fruit;
    private void Awake()
    {


    }
    private void Start()
    {


        crossHair = FindObjectOfType<CrossHairControll>();
        //DragItem = FindObjectOfType<DragAndDropItem>();
        shopperSystem = GetComponent<ShopperSystemController>();
        shopperSystem.OnChangePhase += ShopperSystem_OnChangePhase;
        cutter = GetComponent<Cutter>();
        line = GetComponent<LineRenderer>();
        SelectedFruits = new List<GameObject>();
        SelectedInnerMatrials = new List<Material>();
        line.positionCount = 2;
        
    }
   
    private void Update()
    {
        if (IsTouchReady)
            Touch();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(point1, point2);
        //  DebugExtension.DrawCapsule(point1, point2, Color.green, 0.5f);


    }
    private void ShopperSystem_OnChangePhase(ShopperSystemController.PhaseGame phase)
    {
        if (phase == ShopperSystemController.PhaseGame.Cut)
        {
            IsTouchReady = true;
            IsReadyForCut = true;
        }
        else if (phase == ShopperSystemController.PhaseGame.Pickup)
        {
            IsTouchReady = true;
            IsReadyForCut = false;
        }
        else if (phase == ShopperSystemController.PhaseGame.Wait)
        {
            IsTouchReady = false;
            IsReadyForCut = false;

        }
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
                    if (Physics.Raycast(ray, out hit, 10, MaskForCut))
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
                    if (Physics.Raycast(ray, out hit, 1000, MaskForFruit))
                    {
                        var f = hit.collider.GetComponent<Furit>();
                        var f_p = hit.collider.GetComponent<FruitPiece>();
                        
                        if (f)
                        {

                            fruit_slice = f.gameObject;
                            precent_fruit = f.PercentVolume;
                            offsetOfSelect = f.OffsetCenter;
                            rigidbody_selected_fruit = f.rigidbody;
                            shopperSystem.SetPickedUpFruitData(f.PercentVolume);
                            crossHair.SetVisible(true);
                            // Debug.Log("Fruit Data:" + f.Volume);
                        }
                        else if (f_p)
                        {
                            fruit_slice = f_p.gameObject;
                            precent_fruit = f_p.PercentVolume;
                            offsetOfSelect = f_p.OffsetCenter;
                            rigidbody_selected_fruit = f_p.rigidbody;
                            shopperSystem.SetPickedUpFruitData(f_p.PercentVolume);
                            crossHair.SetVisible(true);
                            //   Debug.Log("Fruit Piece Data:" + f_p.PercentVolume);
                        }
                        
                        FirstPosBeforSelect = fruit_slice.transform.position;
                        rigidbody_selected_fruit.isKinematic = true;


                    }
                }


            }
            else if (touch.phase == TouchPhase.Moved)
            {
                ray = Camera.main.ScreenPointToRay(touch.position);

                if (IsReadyForCut)
                {
                    if (Physics.Raycast(ray, out hit, 10, MaskForCut))
                    {

                        point2 = hit.point;
                        line.SetPosition(0, point1);
                        line.SetPosition(1, point2);
                        //    Debug.Log(hit.collider.name);
                    }
                }
                else
                {
                    if (Physics.Raycast(ray, out hit, 1000, MaskForHumen))
                    {
                        if (fruit_slice)
                        {
                            fruit_slice.transform.position = hit.point + offsetOfSelect;
                            
                        }

                    }
                }

            }
            else if (touch.phase == TouchPhase.Ended)
            {
                ray = Camera.main.ScreenPointToRay(touch.position);

                if (IsReadyForCut)
                {
                    if (Physics.Raycast(ray, out hit, 10, MaskForCut))
                    {

                        // pos_click.z = 10.0f;
                        point2 = hit.point;
                        selector.RayFire(point1, point2);



                        cutter.SetCutPlane(point1, point2, 1f);




                        //   CutFruits();
                        line.positionCount = 0;
                    }

                }
                else
                {
                    if (Physics.Raycast(ray, out hit, 1000, MaskForHumen))
                    {
                        if (hit.collider.tag == "shopper")
                        {
                         //   var pos = FindObjectOfType<DestroyPlace>().transform.position;
                            var char_agent_temp = hit.collider.GetComponent<Char_Agent>();
                           // var id = char_agent_temp.ID;
                            char_agent_temp.PrograssbarAndPointSet( precent_fruit);
                            
                            Destroy(fruit_slice);

                          //  DOVirtual.DelayedCall(1, () => { char_agent_temp.AgentMoveToDestroy(pos); });
                           
                            //   Debug.Log("END" + hit.collider.name);
                        }
                        else
                        {
                            if (fruit_slice)
                            {
                                //fruit_slice.transform.position = FirstPosBeforSelect;
                                rigidbody_selected_fruit.isKinematic = false;
                              
                            }
                        }
                        fruit_slice = null;
                        rigidbody_selected_fruit = null;
                        FirstPosBeforSelect = Vector3.zero;
                        shopperSystem.SetPickedUpFruitData(0);
                        
                    }
                    crossHair.SetVisible(false);
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

}
