using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackableObjectManager : ObservableObject
{


    private List<TrackableObject> trackableObjects;
    private TrackableObjectFileManager fileManager;

    public TrackableObjectManager()
    {
        this.trackableObjects = new List<TrackableObject>();
        this.fileManager = new TrackableObjectFileManager();
    }

    public void LoadTrackableObject(IFile file, Tracker tracker)
    {
        TrackableObjectMemento memento = fileManager.Load<TrackableObjectMemento>(file);

        Transform parent = GameObject.Instantiate(new GameObject()).transform;
        parent.SetParent(tracker.transform);
        parent.gameObject.AddComponent<TrackableObject>();
        parent.name = file.Name(false);

        TrackableObject.CreateFromMemento(memento, parent, false);


    }
}
