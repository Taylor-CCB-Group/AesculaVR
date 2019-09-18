using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointMeasure : Measure
{

    public new static MeasureManager.MeasureType Type => MeasureManager.MeasureType.Point;

    public override Manipulatable PointB => base.PointA;
    public override Vector3 Value => PointA.transform.position;


    public class PointMemento : Memento
    {
        public PointMemento(Measure measure) : base(measure)
        {
            this.type = (int)PointMeasure.Type;
        }
    }

    public override IMemento SaveMemento() => new PointMemento(this);
}
