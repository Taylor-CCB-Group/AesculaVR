using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// A measure for getting a position.
/// </summary>
public class PointMeasure : Measure
{

    public new static MeasureManager.MeasureType Type => MeasureManager.MeasureType.Point;

    public override Vector3 Value => PointA.transform.position;

    public override Manipulatable PointB => points[0];


    private void Awake()
    {
        Debug.Assert(points.Count == 1);
    }


    public class PointMemento : Memento
    {
        public PointMemento(Measure measure) : base(measure)
        {
            this.type = (int)PointMeasure.Type;
        }
    }

    public override IMemento SaveMemento() => new PointMemento(this);
}
