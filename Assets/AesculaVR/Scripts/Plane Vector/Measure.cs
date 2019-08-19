using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// For objects that require two points.
/// </summary>
public abstract class Measure : MonoBehaviour
{
    [SerializeField] protected Manipulatable positionA, positionB;
    public Vector3 Value => (positionA.transform.position - positionB.transform.position);

    #region IMementoOriginator

    public class LineMemento : IMemento
    {
        public readonly Vector3 a, b;

        public LineMemento(Measure measure)
        {
            this.a = measure.positionA.transform.localPosition;
            this.b = measure.positionB.transform.localPosition;
        }

    }

    public IMemento SaveMemento() => new LineMemento(this);

    public void RestoreMemento(IMemento memento)
    {
        LineMemento m = (LineMemento)memento;
        this.positionA.transform.position = m.a;
        this.positionB.transform.position = m.b;
    }
    #endregion
}
