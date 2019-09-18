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
    [SerializeField] protected Manipulatable positionB;
    [SerializeField] protected Manipulatable positionA;
#pragma warning restore 0649

    protected Color color;
    public Color Color => color;



    public virtual Manipulatable PointB => positionB;
    public virtual Manipulatable PointA => positionA;
    public virtual Vector3 Value => (positionA.transform.position - positionB.transform.position);
    

    /// <summary>
    /// Set weather or not the manlipulatable components on each point are enabled.
    /// </summary>
    /// <param name="value"> Are the manlipulatables enabled? </param>
    public virtual void SetManipulatablesEnabled(bool value)
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
        [SerializeField] public Vector3 position, rotation;
        [SerializeField] public int type;

        public Memento(Measure measure)
        {
            this.position = measure.transform.localPosition;
            this.rotation = measure.transform.localRotation.eulerAngles;

            this.a = measure.PointA.transform.localPosition;
            this.b = measure.PointB.transform.localPosition;
            this.type = -1;
        }
             
    }

    public virtual IMemento SaveMemento() => new Memento(this);

    public void RestoreMemento(IMemento memento)
    {
        Memento m = (Memento)memento;
        this.transform.localPosition = m.position;
        this.transform.localRotation = Quaternion.Euler(m.rotation);

        this.PointA.transform.localPosition = m.a;
        this.PointB.transform.localPosition = m.b;        
    }


    #endregion

    #region IPoolable
    public void OnPoppedFromPool()
    {
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
