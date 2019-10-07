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
    [SerializeField] protected List<Manipulatable> points;
#pragma warning restore 0649

    protected Color color;
    public Color Color => color;

    public virtual Manipulatable PointA => points[0];
    public abstract Manipulatable PointB { get; }
    public abstract Vector3 Value { get; }
    

    /// <summary>
    /// Set weather or not the manlipulatable components on each point are enabled.
    /// </summary>
    /// <param name="value"> Are the manlipulatables enabled? </param>
    public virtual void SetManipulatablesEnabled(bool value)
    {
        int count = points.Count;
        for(int i = 0; i < count;i++)
            points[i].enabled = value;
    }

    /// <summary>
    /// Set the color of the two manlipulatable points.
    /// </summary>
    /// <param name="color">The color to set them to. </param>
    public virtual void SetColor(Color color)
    {
        this.color = color;
        int count = points.Count;
        for (int i = 0; i < count; i++)
            points[i].GetComponent<Renderer>().material.color = color;

    }
    

    #region IMementoOriginator

    [System.Serializable]
    public class Memento : IMemento
    {

        [SerializeField] public List<Vector3> positions;
        [SerializeField] public Vector3 position, rotation;
        [SerializeField] public int type;

        public Memento(Measure measure)
        {
            this.position = measure.transform.localPosition;
            this.rotation = measure.transform.localRotation.eulerAngles;

            int count = measure.points.Count;
            this.positions = new List<Vector3>(count);
            for (int i = 0; i < count; i++)
                positions.Add(measure.points[i].transform.localPosition);

            this.type = -1;
        }
             
    }

    public virtual IMemento SaveMemento() => new Memento(this);

    public virtual void RestoreMemento(IMemento memento)
    {
        Memento m = (Memento)memento;
        this.transform.localPosition = m.position;
        this.transform.localRotation = Quaternion.Euler(m.rotation);

        int count = m.positions.Count;
        for(int i = 0; i < count; i++)
        {
            this.points[i].transform.localPosition = m.positions[i];
        }
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
