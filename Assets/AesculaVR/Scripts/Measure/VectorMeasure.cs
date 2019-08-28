﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorMeasure : Measure, IMementoOriginator
{

    public new static MeasureManager.MeasureType Type => MeasureManager.MeasureType.Vector;

#pragma warning disable 0649
    [SerializeField] private GameObject link;
#pragma warning restore 0649

    private const float linkScale = 0.25f;

    private void LateUpdate()
    {
        DrawLinkBetweenPoints();
    }

    /// <summary>
    /// rotate, scale and move the link gameobject so it connects the two points.
    /// </summary>
    private void DrawLinkBetweenPoints()
    {
        //this was ripped from bable VR.

        //get the positions.
        Vector3 pa = positionA.transform.position;
        Vector3 pb = positionB.transform.position;

        //get the position between the two points.
        Vector3 pos = Vector3.zero;
        pos[0] = (pa[0] + pb[0]) * 0.5f;
        pos[1] = (pa[1] + pb[1]) * 0.5f;
        pos[2] = (pa[2] + pb[2]) * 0.5f;
        link.transform.position = pos;

        //set the scale to be reletive to the links hierarchy.
        Transform tsfm = link.transform.parent;
        if (tsfm)
        {
            pa = tsfm.InverseTransformPoint(pa);
            pb = tsfm.InverseTransformPoint(pb);
        }

        //set pos[2] to be the distance bwteen the two points.
        Vector3 smallestPoint = pa;
        smallestPoint[0] -= pb[0]; smallestPoint[1] -= pb[1]; smallestPoint[2] -= pb[2];
        pos[2] = (smallestPoint).magnitude;

        //get the smallest point
        pa = positionA.transform.lossyScale;
        pb = positionB.transform.lossyScale;
        smallestPoint = (pa.sqrMagnitude < pb.sqrMagnitude) ? pa : pb;

        //set the cross section of the link to be propotional to the smallest point.
        pos[0] = smallestPoint[0] * linkScale;
        pos[1] = smallestPoint[1] * linkScale;

        link.transform.localScale = pos;
        link.transform.LookAt(positionB.transform);
    }

    public override void SetColor(Color color)
    {
        base.SetColor(color);
        this.link.GetComponent<Renderer>().material.color = color;
    }

    public override IMemento SaveMemento() => new VectorMemento(this);

    public class VectorMemento : Memento
    {
        public VectorMemento(Measure measure) : base(measure)
        {
            this.type = (int)VectorMeasure.Type;
        }
    }
}