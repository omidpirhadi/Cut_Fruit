using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;
public class TouchController : MonoBehaviour
{

   // public GameObject PointerObject;
    public GameObject Plane;
    public Cutter cutter;
  //  private Vector3 point;
    public LayerMask maskBackground;
    public LineRenderer line;
    private RaycastHit hit;
    
    private Ray ray;
    private Vector3 currentPos;
    private Vector3 prev_pos;
    private Vector3 point1;
    private Vector3 point2;
    public Vector3 GuidLine;
    public void OnMouseDown()
    {
        ray =  Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, maskBackground))
        {
            Plane.transform.position = hit.point;
         //   currentPos = new Vector3(hit.point.x, hit.point.y, -4.0f);
           // Debug.Log("AAAA" + currentPos);
            //Debug.Log("Wall");
        }
    }
    public void OnMouseDrag()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, maskBackground))
        {
            prev_pos = currentPos;
            //Plane.transform.LookAt(new Vector3(hit.point.x, hit.point.y, 0));
            Plane.transform.position = new Vector3(hit.point.x, hit.point.y, -4.0f);
            
           currentPos = new Vector3(hit.point.x, hit.point.y, -4.0f);
          //  Debug.Log($"Vec : {prev_pos}, Current:{currentPos}");
        }
    }
    public void OnMouseUp()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, maskBackground))
        {

            prev_pos = new Vector3(hit.point.x, hit.point.y, -4.0f);
          //  Debug.Log("BBBB" + prev_pos);
            var dir = prev_pos - currentPos;
          ///  Debug.Log("Dir" + dir);
            float angle = Vector3.Angle(dir, GuidLine);

            Plane.transform.eulerAngles = new Vector3(0, 0, -angle);
            Debug.Log("angle:" + angle);
        }

        line.SetPosition(0, Vector3.zero);
        line.SetPosition(1, Vector3.zero);
        //cutter.Cut();
       // Debug.Log("Cut");
    }
    /* public void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            PlanePlace();

        }
        if(Input.GetKey(KeyCode.Mouse0))
        {
            PlaneRotate();

        }

    }
   public void PlanePlace()
    {
        
      ///  var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float x = Input.mousePosition.x;
        float y =  Input.mousePosition.y;
        point = Camera.main.ScreenToWorldPoint(new Vector3(x, y, Camera.main.nearClipPlane));
        Plane.transform.position = point;
    }
    public void PlaneRotate()
    {
        float x = Input.mousePosition.x;
        float y = Input.mousePosition.y;
        point = Camera.main.ScreenToWorldPoint(new Vector3(x, y, Camera.main.nearClipPlane));

        Plane.transform.LookAt(point);
    }*/
}
