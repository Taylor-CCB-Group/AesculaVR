using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneLine : Measure
{
    [SerializeField] private GameObject quad;
       
    private void LateUpdate()
    {
        QuadFacePoint();
    }

    void QuadFacePoint()
    {
        this.quad.transform.LookAt(positionB.transform);
    }

}
