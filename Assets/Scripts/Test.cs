using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Transform P1, P2;
    public Vector3 dir;
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
     //   DD();
    }
    private void SetCutPlane(Vector3 point1, Vector3 point2, Transform plane)
    {
        var dir = point2 - point1;
        plane.position = point1 + (dir / 2.0f);
        plane.localScale = new Vector3(2, 2, 2);
        plane.forward = dir;
    }
}