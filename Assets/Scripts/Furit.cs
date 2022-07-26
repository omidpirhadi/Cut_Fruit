﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furit : MonoBehaviour
{
    public string FuritTag;   
    public float Volume;
    // public int SliceNumber;
    //  public bool Sliced = false;
    public bool IsReadyPickedUp = false;

    public Material hightlighter;
    private Material default_matrial;
    private TouchController controller;
    private FuritSliceManager furitSliceManager;
    private Mesh meshFilter;

    //private List<Material> matrials_Temp;
    void Start()
    {

       
        FindObjectOfType<UI>().ChangeMode += Furit_ChangeMode;
        meshFilter = GetComponent<MeshFilter>().sharedMesh;
        Volume = VolumeOfMesh(meshFilter);
        controller = FindObjectOfType<TouchController>();
        furitSliceManager = FindObjectOfType<FuritSliceManager>();
        
        
      //  furitSliceManager.AddFuritOnStartGame(FuritTag, Volume);
         
     //  default_matrial = GetComponent<MeshRenderer>().materials[0];

    }

    private void Furit_ChangeMode(bool cut , bool pick)
    {
       IsReadyPickedUp = pick;
         if (pick)
        {
            //GetComponent<MeshRenderer>().materials = new Material[2] { default_matrial, hightlighter };
        }
        else
        {

           // GetComponent<MeshRenderer>().materials = new Material[1] { default_matrial};
        }
    }

    private void OnDestroy()
    {
       // FindObjectOfType<UI>().ChangeMode -= Furit_ChangeMode;
    }
    private void OnMouseDown()
    {
        if(IsReadyPickedUp)
        {
            //hightlighter.SetFloat("_Width", 10);
            Debug.Log("PICK");
        }
    }
    private void OnMouseDrag()
    {
        if (IsReadyPickedUp)
        {
           var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            ///var offset = transform.position - transform.TransformPoint(GetComponent<MeshFilter>().mesh.bounds.center);
            // offset.z = transform.position.z;
            this.transform.position = new Vector3(pos.x, pos.y, transform.position.z) ;
        }
    }
    void OnMouseEnter()
    {
        controller.FruitSelect(this.gameObject);
    }
    
    void OnMouseExit()
    {
       // controller.FruitSelect(this.gameObject);
    }

    public float SignedVolumeOfTriangle(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float v321 = p3.x * p2.y * p1.z;
        float v231 = p2.x * p3.y * p1.z;
        float v312 = p3.x * p1.y * p2.z;
        float v132 = p1.x * p3.y * p2.z;
        float v213 = p2.x * p1.y * p3.z;
        float v123 = p1.x * p2.y * p3.z;

        return (1.0f / 6.0f) * (-v321 + v231 + v312 - v132 - v213 + v123);
    }

    public float VolumeOfMesh(Mesh mesh)
    {
        float volume = 0;

        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;

        for (int i = 0; i < triangles.Length; i += 3)
        {
            Vector3 p1 = vertices[triangles[i + 0]];
            Vector3 p2 = vertices[triangles[i + 1]];
            Vector3 p3 = vertices[triangles[i + 2]];
            volume += SignedVolumeOfTriangle(p1, p2, p3);
            /// volume *= this.transform.localScale.x * this.transform.localScale.y * this.transform.localScale.z;
        }
        return Mathf.Abs(volume);
    }
}
