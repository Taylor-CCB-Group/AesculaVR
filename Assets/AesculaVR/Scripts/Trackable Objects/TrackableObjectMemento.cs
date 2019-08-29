using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Memento for a trackable object. 
/// </summary>
[System.Serializable]
public class TrackableObjectMemento : IMemento
{
    /// <summary>
    /// The Memento for a generated object. 
    /// </summary>


    [SerializeField] public List<GeneratedObject.Memento> objects;
    [SerializeField] public List<Measure.Memento> measures;

    public TrackableObjectMemento(List<GeneratedObject> generatedObjects, List<Measure> measures)
    {
        //generated objects
        this.objects = new List<GeneratedObject.Memento>(generatedObjects.Count);
        for (int i = 0; i < generatedObjects.Count; i++)
        {
            objects.Add((GeneratedObject.Memento)generatedObjects[i].SaveMemento());
        }

        //measures
        this.measures = new List<Measure.Memento>(measures.Count);
        for (int i = 0; i < measures.Count; i++)
        {
            this.measures.Add((Measure.Memento)measures[i].SaveMemento());
        }

    }
}
