using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneMeasure : Measure
{
    [SerializeField] private GameObject quad = null;
    [SerializeField] private GameObject quad2 = null;

    private void LateUpdate()
    {
        QuadFacePoint();
    }

    void QuadFacePoint()
    {
        this.quad.transform.LookAt(positionB.transform);
        this.quad.transform.position = (positionA.transform.position + positionB.transform.position) / 2;
    }

    public override void SetColor(Color color)
    {
        base.SetColor(color);
        this.quad .GetComponent<Renderer>().material.color = color;
        this.quad2.GetComponent<Renderer>().material.color = color;
    }

}
