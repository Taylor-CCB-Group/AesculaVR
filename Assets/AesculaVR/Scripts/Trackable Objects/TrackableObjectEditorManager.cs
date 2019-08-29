using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackableObjectEditorManager : IMementoOriginator
{
    /// <summary>
    /// The file manager for trackable objects.
    /// </summary>
    public FileManager FileManager { get { return fileManager; } }
    private TrackableObjectFileManager fileManager;

    private EditorManager editorManager;

    public TrackableObjectEditorManager(EditorManager editorManager)
    {
        fileManager = new TrackableObjectFileManager();
        this.editorManager = editorManager;
    }


    /// <summary>
    /// Save the trackable object to a file
    /// </summary>
    /// <param name="fileName">the name of the file, without an Extenstion.</param>
    public void Save(string fileName) => fileManager.Save<TrackableObjectMemento>(fileName, SaveMemento());

    /// <summary>
    /// Load a trackable object from a file
    /// </summary>
    /// <param name="file">the file to load from.</param>
    public void Load(IFile file) => RestoreMemento(fileManager.Load<TrackableObjectMemento>(file));

    /// <summary>
    /// Deletes a trackable object file.
    /// </summary>
    /// <param name="file">The file to delete.</param>
    public void Delete(IFile file) => fileManager.Delete(file);

    /// <summary>
    /// Clear all the generated objects in the scene.
    /// </summary>
    public void Clear()
    {
        List<GeneratedObject> objects = editorManager.ObjectManager.GeneratedObjects;
        List<IAction> actions = new List<IAction>(objects.Count);

        while (objects.Count > 0)
        {
            IAction action = editorManager.ObjectManager.DestroyObject(objects[objects.Count - 1]);
            objects.RemoveAt(objects.Count - 1);
            actions.Add(action);
        }

        actions.Add(editorManager.MeasureManager.ClearMeasures());
        
        editorManager.ActionManager.DoAction(new CompoundAction(actions, "Clear all the objects in the scene"));

    }



    #region IMementoOriginator


    public IMemento SaveMemento()
    {
        return new TrackableObjectMemento
        (
            editorManager.ObjectManager.GeneratedObjectsReference,
            editorManager.MeasureManager.Measures
        ); 
    }

    public void RestoreMemento(IMemento memento)
    {
        TrackableObjectMemento trackableObjectsMemento = (TrackableObjectMemento)memento;
        IAction action = TrackableObject.CreateFromMemento(trackableObjectsMemento, editorManager.TrackerManager.Main.transform, true);
        editorManager.ActionManager.DoAction(action);
    }



   

    #endregion





}
