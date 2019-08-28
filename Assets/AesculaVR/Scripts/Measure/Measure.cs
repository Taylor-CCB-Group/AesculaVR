using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// For objects that require two points.
/// </summary>
public abstract class Measure : MonoBehaviour, IMementoOriginator, IPoolable
{

    public static MeasureManager.MeasureType Type => throw new System.NotSupportedException();

#pragma warning disable 0649
    [SerializeField] protected Manipulatable positionA, positionB;
#pragma warning restore 0649

    protected Color color;
    public Color Color => color;

    public Manipulatable PointA => positionA;
    public Manipulatable PointB => positionB;

    public Vector3 Value => (positionA.transform.position - positionB.transform.position);

    /// <summary>
    /// Set weather or not the manlipulatable components on each point are enabled.
    /// </summary>
    /// <param name="value"> Are the manlipulatables enabled? </param>
    public void SetManipulatablesEnabled(bool value)
    {
        positionA.enabled = value;
        positionB.enabled = value;
    }

    /// <summary>
    /// Set the color of the two manlipulatable points.
    /// </summary>
    /// <param name="color">The color to set them to. </param>
    public virtual void SetColor(Color color)
    {
        this.color = color;
        positionA.GetComponent<Renderer>().material.color = color;
        positionB.GetComponent<Renderer>().material.color = color;
    }
    

    #region IMementoOriginator

    [System.Serializable]
    public class Memento : IMemento
    {

        [SerializeField] public Vector3 a, b;
        [SerializeField] public int type;

        public Memento(Measure measure)
        {
            this.a = measure.positionA.transform.localPosition;
            this.b = measure.positionB.transform.localPosition;
            this.type = -1;
        }

    }

    public abstract IMemento SaveMemento();

    public void RestoreMemento(IMemento memento)
    {
        Memento m = (Memento)memento;
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
