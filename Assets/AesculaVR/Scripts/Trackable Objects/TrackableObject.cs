using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TrackableObject : ObservableComponent
{

    /// <summary>
    /// Create a Trackable Object from a TrackableObjectMemento.
    /// </summary>
    /// <param name="memento" > The memento to load. </param>
    /// <param name="parent"  > The parent object for the trackable object. </param>
    /// <param name="editable"> Will components of the trackable objects be editable? </param>
    /// <returns></returns>
    public static IAction CreateFromMemento(TrackableObjectMemento memento, Transform parent, bool editable) => new CreateTrackableObjectFromMementoAction(memento,parent, editable);

    private IFile file;
    private List<Measure> measures;

    public IFile SourceFile => file;

    protected void Awake()
    {
        this.measures = new List<Measure>();
    }

    public void Setup(IFile source, List<Measure> measures)
    {
        this.file = source;
        this.measures.AddRange(measures);
        NotifyObservers();
    }
}
