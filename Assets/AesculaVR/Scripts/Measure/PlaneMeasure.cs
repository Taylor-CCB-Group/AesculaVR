using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneMeasure : Measure
{

    public new static MeasureManager.MeasureType Type => MeasureManager.MeasureType.Plane;

#pragma warning disable 0649
    [SerializeField] private GameObject quad = null;
    [SerializeField] private GameObject quad2 = null;
#pragma warning restore 0649

    private void LateUpdate()
    {
        QuadFacePoint();
    }

    /// <summary>
    /// Rotate the quad so its perpendicular to the two points, and move it bewtween the two.
    /// </summary>
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

    public override IMemento SaveMemento() => new PlaneMemento(this);

    public class PlaneMemento : Memento
    {
        public PlaneMemento(Measure measure) : base(measure)
        {
            this.type = (int)PlaneMeasure.Type;
        }
    }

}
