using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// For objects that require two points.
/// </summary>
public abstract class Measure : MonoBehaviour, IMementoOriginator, IPoolable
{
    [SerializeField] protected Manipulatable positionA, positionB;
    protected Color color;
    public Color Color => color;

    public Manipulatable PointA => positionA;
    public Manipulatable PointB => positionB;

    public Vector3 Value => (positionA.transform.position - positionB.transform.position);

    public void SetManipulatablesEnabled(bool value)
    {
        positionA.enabled = value;
        positionB.enabled = value;
    }

    public virtual void SetColor(Color color)
    {
        this.color = color;
        positionA.GetComponent<Renderer>().material.color = color;
        positionB.GetComponent<Renderer>().material.color = color;
    }
    

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

    #region IPoolable

    public void OnPoppedFromPool()
    {
        //assuming A,B are children, [ and concrete gameobjects ].

        this.gameObject.SetActive(false);
        this.gameObject.hideFlags = HideFlags.HideInHierarchy;
    }

    public void OnPushedToPool()
    {
        this.gameObject.hideFlags = HideFlags.None;
        this.gameObject.SetActive(true);      
    }
    
    #endregion
}
