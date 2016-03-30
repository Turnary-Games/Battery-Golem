// Copyright (c) <2015> <Playdead>
// This file is subject to the MIT License as seen in the root of this folder structure (LICENSE.TXT)
// AUTHOR: Lasse Jon Fuglsang Pedersen <lasse@playdead.com>

using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Playdead/VelocityBufferTag")]
public class VelocityBufferTag : MonoBehaviour
{
    public static List<VelocityBufferTag> activeObjects = new List<VelocityBufferTag>(128);

    [NonSerialized, HideInInspector] public Mesh mesh;
    [NonSerialized, HideInInspector] public Matrix4x4 localToWorldPrev;
    [NonSerialized, HideInInspector] public Matrix4x4 localToWorldCurr;

    private SkinnedMeshRenderer skinnedMesh = null;
    public bool useSkinnedMesh = false;

    public const int framesNotRenderedThreshold = 60;
    private int framesNotRendered = framesNotRenderedThreshold;

    [NonSerialized] public bool sleeping = false;

    void Start()
    {
        if (useSkinnedMesh)
        {
            var smr = this.GetComponent<SkinnedMeshRenderer>();
            if (smr != null)
            {
                mesh = new Mesh();
                skinnedMesh = smr;
                skinnedMesh.BakeMesh(mesh);
            }
        }
        else
        {
            var mf = this.GetComponent<MeshFilter>();
            if (mf != null)
            {
                mesh = mf.sharedMesh;
            }
        }

        localToWorldCurr = transform.localToWorldMatrix;
        localToWorldPrev = localToWorldCurr;
    }

    void VelocityUpdate()
    {
        if (useSkinnedMesh)
        {
            if (skinnedMesh == null)
            {
                Debug.LogWarning("vbuf skinnedMesh not set", this);
                return;
            }

            if (sleeping)
            {
                skinnedMesh.BakeMesh(mesh);
                mesh.normals = mesh.vertices;// garbage ahoy
            }
            else
            {
                Vector3[] vs = mesh.vertices;// garbage ahoy
                skinnedMesh.BakeMesh(mesh);
                mesh.normals = vs;
            }
        }

        if (sleeping)
        {
            localToWorldCurr = transform.localToWorldMatrix;
            localToWorldPrev = localToWorldCurr;
        }
        else
        {
            localToWorldPrev = localToWorldCurr;
            localToWorldCurr = transform.localToWorldMatrix;
        }

        sleeping = false;
    }

    void LateUpdate()
    {
        if (framesNotRendered < framesNotRenderedThreshold)
        {
            framesNotRendered++;
        }
        else
        {
            sleeping = true;// sleep until next OnWillRenderObject
            return;
        }

        VelocityUpdate();
    }

    void OnWillRenderObject()
    {
        if (Camera.current != Camera.main)
            return;// ignore anything but main cam

        if (sleeping)
        {
            VelocityUpdate();
        }

        framesNotRendered = 0;
    }

    void OnEnable()
    {
        activeObjects.Add(this);
    }

    void OnDisable()
    {
        activeObjects.Remove(this);
    }
}
