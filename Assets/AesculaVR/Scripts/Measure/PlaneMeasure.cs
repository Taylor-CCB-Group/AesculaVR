using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A measure for getting a normal to a plane.
/// </summary>
public class PlaneMeasure : Measure
{

    public new static MeasureManager.MeasureType Type => MeasureManager.MeasureType.Plane;

    public override Vector3 Value => (points[0].transform.position - points[1].transform.position).normalized;

    public override  Manipulatable PointB => points[1];

#pragma warning disable 0649
    [SerializeField] private GameObject quad = null;
    [SerializeField] private GameObject quad2 = null;
#pragma warning restore 0649

    private void Awake()
    {
        Debug.Assert(points.Count == 2);
    }

    private void LateUpdate()
    {
        QuadFacePoint();
    }

    /// <summary>
    /// Rotate the quad so its perpendicular to the two points, and move it bewtween the two.
    /// </summary>
    void QuadFacePoint()
    {
        this.quad.transform.LookAt(points[1].transform);
        this.quad.transform.position = (points[0].transform.position + points[1].transform.position) / 2;
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
