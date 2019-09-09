using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackableObjectManager : ObservableObject
{

    private class LoadTrackableObjectAction : IAction
    {

        private CreateTrackableObjectFromMementoAction createAction;
        private TrackableObjectManager tManager;
        private TrackableObject trackableObject;

        public LoadTrackableObjectAction(TrackableObjectManager tManager, TrackableObjectMemento memento, IFile file, Tracker tracker)
        {

            this.tManager = tManager;
            Transform parent = GameObject.Instantiate(new GameObject()).transform;
            parent.SetParent(tracker.transform);

            parent.transform.localPosition = Vector3.zero;
            parent.transform.localScale = Vector3.one;
            parent.transform.localRotation = Quaternion.Euler(0, 0, 0);

            trackableObject = parent.gameObject.AddComponent<TrackableObject>();
            parent.name = file.Name(false);

            createAction = (CreateTrackableObjectFromMementoAction)TrackableObject.CreateFromMemento(memento, parent, false);
            trackableObject.Setup(file, createAction.Measures);

        }

        public string Description() => createAction.Description();


        public void DoAction()
        {
            createAction.DoAction();
            trackableObject.gameObject.SetActive(true);
            tManager.AddTrackableObject(trackableObject);
        }

        public void UndoAction()
        {
            createAction.UndoAction();
            trackableObject.gameObject.SetActive(false);
            tManager.RemoveTrackableObject(trackableObject);
        }
    }

    private MainManager mainManager;
    private List<TrackableObject> trackableObjects;

    public List<TrackableObject> TrackableObjects => new List<TrackableObject>(trackableObjects);
    public List<TrackableObject> TrackableObjectsReference => (trackableObjects);



    public FileManager FileManager { get { return fileManager; } }
    private TrackableObjectFileManager fileManager;

    /// <summary>
    /// Add a trackable object to the manager.
    /// </summary>
    /// <param name="trackableObject">The trackable object to add.</param>
    private void AddTrackableObject(TrackableObject trackableObject)
    {
        trackableObjects.Add(trackableObject);
        NotifyObservers();
    }

    /// <summary>
    /// remove a trackable object from the manager.
    /// </summary>
    /// <param name="trackableObject">The trackable object to remove.</param>
    private void RemoveTrackableObject(TrackableObject trackableObject)
    {
        trackableObjects.Remove(trackableObject);
        NotifyObservers();
    }

    public TrackableObjectManager()
    {
        this.mainManager = MainManager.GetManager();
        this.trackableObjects = new List<TrackableObject>();
        this.fileManager = new TrackableObjectFileManager();
    }

    /// <summary>
    /// Load a trackable object from a file, and set it to be a child to a tracker.
    /// </summary>
    /// <param name="file">The file to load from.</param>
    /// <param name="tracker">The tracker to attack the trackable object to.</param>
    public void LoadTrackableObject(IFile file, Tracker tracker)
    {
        TrackableObjectMemento memento = fileManager.Load<TrackableObjectMemento>(file);
        IAction a = new LoadTrackableObjectAction(this, memento, file, tracker);
        mainManager.ActionManager.DoAction(a);
    }
}
