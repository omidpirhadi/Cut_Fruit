﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitPiece : MonoBehaviour
{
    public string FuritTag;

    public Material InnerMatrialAfterCut;
    public float TotalVolume;
    public float Volume;
    public float PercentVolume;
   

    private TouchController controller;
    private Mesh meshFilter;
    private  new MeshRenderer  renderer;
    


    private float Get_Z;
    private UI ui;
    private int index = 0;
    private new Rigidbody rigidbody;
    void Start()
    {


        meshFilter = GetComponent<MeshFilter>().sharedMesh;
        Volume = VolumeOfMesh(meshFilter);
        controller = FindObjectOfType<TouchController>();
        renderer = GetComponent<MeshRenderer>();
        rigidbody = GetComponent<Rigidbody>();
        PercentVolume = (Volume / TotalVolume) * 100;
    }
    private void LateUpdate()
    {
        if (rigidbody.velocity.magnitude > 20)
        {
            rigidbody.drag = 20;
            rigidbody.angularDrag = 20;
        }
        else
        {
            rigidbody.drag = 1;
            rigidbody.angularDrag = 0;
        }
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
            
        }
        return Mathf.Abs(volume*1000);
    }
}
