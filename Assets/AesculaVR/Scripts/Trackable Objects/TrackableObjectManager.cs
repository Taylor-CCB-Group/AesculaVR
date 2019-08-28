using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackableObjectManager : IMementoOriginator
{
    /// <summary>
    /// The file manager for trackable objects.
    /// </summary>
    public FileManager FileManager { get { return fileManager; } }
    private TrackableObjectFileManager fileManager;

    private EditorManager editorManager;

    public TrackableObjectManager(EditorManager editorManager)
    {
        fileManager = new TrackableObjectFileManager();
        this.editorManager = editorManager;
    }


    /// <summary>
    /// Save the trackable object to a file
    /// </summary>
    /// <param name="fileName">the name of the file, without an Extenstion.</param>
    public void Save(string fileName) => fileManager.Save<TrackableObjectsMemento>(fileName, SaveMemento());

    /// <summary>
    /// Load a trackable object from a file
    /// </summary>
    /// <param name="file">the file to load from.</param>
    public void Load(IFile file) => RestoreMemento(fileManager.Load<TrackableObjectsMemento>(file));

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
        Debug.Log("Hello");

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

    /// <summary>
    /// Generates an object from an TrackableObjectsMemento.GeneratedObjectMemento and sets its parent.
    /// </summary>
    /// <param name="generatedObjectMemento">The object we want to generate.</param>
    /// <param name="parent">the parent of the generated object. </param>
    /// <returns>returns the action for generating the object. The action has not yet been done. </returns>
    public static ObjectManager.GenerateObjectAction GenerateObjectFomMemento(TrackableObjectsMemento.GeneratedObjectMemento generatedObjectMemento, Transform parent)
    {
        EditorManager editorManager = EditorManager.GetManager();
        ObjectManager.GenerateObjectAction action = (ObjectManager.GenerateObjectAction)editorManager.ObjectManager.GenerateObject(new File(generatedObjectMemento.objPath));
        GeneratedObject go = action.GeneratedObject;
        //set go to be child of tracker.
        go.transform.SetParent(parent);

        go.transform.localPosition = generatedObjectMemento.localPosition;
        go.transform.localRotation = Quaternion.Euler(generatedObjectMemento.rotation);
        go.transform.localScale = generatedObjectMemento.scale;

        return action;
    }

    #region IMementoOriginator


    public IMemento SaveMemento()
    {
        return new TrackableObjectsMemento
        (
            editorManager.ObjectManager.GeneratedObjectsReference,
            editorManager.MeasureManager.Measures
        ); 
    }

    public void RestoreMemento(IMemento memento)
    {
        TrackableObjectsMemento trackableObjectsMemento = (TrackableObjectsMemento)memento;
        List<IAction> actions = new List<IAction>(trackableObjectsMemento.objects.Count + 1);

        //objects
        for(int i = 0; i < trackableObjectsMemento.objects.Count; i++)
        {

            ObjectManager.GenerateObjectAction action = GenerateObjectFomMemento(trackableObjectsMemento.objects[i], editorManager.TrackerManager.Main?.transform);
            actions.Add(action);
        }

        //measures.
        for(int i = 0; i < trackableObjectsMemento.measures.Count; i++)
        {
            MeasureManager.MeasureType type = (MeasureManager.MeasureType)trackableObjectsMemento.measures[i].type;
            CreateMeasureAction createMeasureAction = new CreateMeasureAction(type);
            createMeasureAction.Measure.RestoreMemento(trackableObjectsMemento.measures[i]);
            actions.Add(createMeasureAction);
        }


        IAction compound = new CompoundAction(actions, "Loaded in a trackable Object");
        editorManager.ActionManager.DoAction(compound);
    }



    [System.Serializable]
    public class TrackableObjectsMemento : IMemento
    {
        [System.Serializable]
        public class GeneratedObjectMemento : IMemento
        {
            [SerializeField] public Vector3 localPosition, rotation, scale;
            [SerializeField] public string objPath;

            public GeneratedObjectMemento(Vector3 localPosition, Vector3 rotation, Vector3 scale, string objPath)
            {
                this.localPosition = localPosition;
                this.rotation = rotation;
                this.scale = scale;
                this.objPath = objPath;
            }
        }

        [SerializeField] public List<GeneratedObjectMemento> objects;
        [SerializeField] public List<Measure.Memento> measures;

        public TrackableObjectsMemento(List<GeneratedObject> generatedObjects, List<Measure> measures)
        {
            //generated objects
            this.objects = new List<GeneratedObjectMemento>(generatedObjects.Count);
            for (int i = 0; i < generatedObjects.Count; i++)
            {
                objects.Add(new GeneratedObjectMemento(
                    generatedObjects[i].transform.localPosition,
                    generatedObjects[i].transform.localRotation.eulerAngles,
                    generatedObjects[i].transform.localScale,
                    generatedObjects[i].File.Path()
                    ));
            }

            //measures
            this.measures = new List<Measure.Memento>(measures.Count);
            for(int i = 0; i < measures.Count; i++)
            {
                this.measures.Add((Measure.Memento)measures[i].SaveMemento());
            }

        }
    }

    #endregion


}
