using Dummiesman;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : ObservableObject, IObserver
{

    public static  Vector3 GeneratedObjectScale { get { return Vector3.one * 0.1f; } }

    private readonly ObjectFileManager fileManager;
    private readonly List<GeneratedObject> activeObjects;
       
    public List<GeneratedObject> GeneratedObjects { get { return new List<GeneratedObject> (activeObjects); } }
    public List<GeneratedObject> GeneratedObjectsReference { get { return activeObjects; } }

    public FileManager FileManager { get { return fileManager; } }


    public void AddGeneratedObject(GeneratedObject generatedObject)
    {
        this.activeObjects.Add(generatedObject);
        NotifyObservers();
    }

    public void RemoveGeneratedObject(GeneratedObject generatedObject)
    {
        this.activeObjects.Remove(generatedObject);
        NotifyObservers();
    }

    public ObjectManager()
    {
        this.fileManager = new ObjectFileManager();
        this.fileManager.AddObserver(this);

        this.activeObjects = new List<GeneratedObject>();
    }

    /// <summary>
    /// Create a Generate Object action
    /// </summary>
    /// <param name="file">The file that contains the object we want to make. </param>
    /// <returns>The new Generate Object action</returns>
    public IAction GenerateObject(IFile file)
    {
        Transform parent = EditorManager.GetManager().TrackerManager.Main?.transform;
        GenerateObjectAction goa = new GenerateObjectAction(file, parent, true);

        if(parent)
            goa.GeneratedObject.transform.position = parent.position;

        return goa;
    }

    /// <summary>
    /// Create a Destroy Object action
    /// </summary>
    /// <param name="generatedObject"> The generated object we want to destroy. </param>
    /// <returns>The new Destroy Object action</returns>
    public IAction DestroyObject(GeneratedObject generatedObject) => new DestroyGeneratedObjectAction(this, generatedObject);

    public void Notify(object Sender, EventArgs args) => NotifyObservers(Sender, args);
}