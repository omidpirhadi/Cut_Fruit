using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;
public class TouchController : MonoBehaviour
{

    // public GameObject PointerObject;
    //public GameObject Plane;
    public bool IsReadyForCut = true;
    public LayerMask MaskFruit;
    private Cutter cutter;
    private LineRenderer line;

    private List<GameObject> SelectedFruits;
    private List<Material> SelectedInnerMatrials;
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
                    pos_click.z = 10.0f;
                    point1 = pos_click;
                    line.SetPosition(0, point1);
                    line.SetPosition(1, point1);
                }
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
                if (IsReadyForCut)
                {
                    pos_click = Camera.main.ScreenToWorldPoint(touch.position);
                    pos_click.z = 10.0f;
                    point2 = pos_click;
                    line.SetPosition(1, point2);
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                if (IsReadyForCut)
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
        
    }
    private void CutFruits()
    {
        for (int i = 0; i < SelectedFruits.Count; i++)
        {
            cutter.Cut(SelectedFruits[i], SelectedInnerMatrials[i]);
        }
    }
    public void FruitSelect(GameObject fruit , Material innerMatrialAfterCut)
    {
        if (!SelectedFruits.Contains(fruit) && !SelectedInnerMatrials.Contains(innerMatrialAfterCut))
        {
            SelectedFruits.Add(fruit);
            SelectedInnerMatrials.Add(innerMatrialAfterCut);
          //  Debug.Log($"Add Furit :{fruit}");
        }
        else
        {
            SelectedFruits.Remove(fruit);
            SelectedInnerMatrials.Remove(innerMatrialAfterCut);
            //  Debug.Log($"Removed Furit :{fruit}");

        }
    }
}
