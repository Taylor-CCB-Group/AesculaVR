using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An Object that was created from an .Obj file.
/// </summary>
public class GeneratedObject : MonoBehaviour, IMementoOriginator
{
    private IFile orginalFile;
    public IFile File { get { return orginalFile; } }



    public void Setup(IFile orginalFile)
    {
        this.orginalFile = orginalFile;
        this.gameObject.name = orginalFile.Name(false);
    }

    #region IMementoOriginator 

    [System.Serializable]
    public class Memento : IMemento
    {
        [SerializeField] public Vector3 localPosition, rotation, scale;
        [SerializeField] public string objPath;

        public Memento(GeneratedObject generatedObject)
        {
            this.localPosition = generatedObject.transform.localPosition;
            this.rotation = generatedObject.transform.localRotation.eulerAngles;
            this.scale = generatedObject.transform.localScale;
            this.objPath = generatedObject.File.Path();
        }
    }

    /// <summary>
    /// Look at using the GenerateObjectFomMemento method instead of this.
    /// </summary>
    void IMementoOriginator.RestoreMemento(IMemento memento) => new System.NotImplementedException();

    public IMemento SaveMemento() => new Memento(this);

    #endregion

    /// <summary>
    /// Generates an object from an GeneratedObject.Memento and sets its parent.
    /// </summary>
    /// <param name="generatedObjectMemento">The object we want to generate.</param>
    /// <param name="parent">the parent of the generated object. </param>
    /// <returns>returns the action for generating the object. The action has not yet been done. </returns>
    public static GenerateObjectAction GenerateObjectFomMemento(GeneratedObject.Memento generatedObjectMemento, Transform parent)
    {
        EditorManager editorManager = EditorManager.GetManager();
        GenerateObjectAction action = new GenerateObjectAction(new File(generatedObjectMemento.objPath), parent, editorManager != null);
        GeneratedObject go = action.GeneratedObject;
       
        //set go to be child of tracker.
        go.transform.SetParent(parent);

        go.transform.localPosition = generatedObjectMemento.localPosition;
        go.transform.localRotation = Quaternion.Euler(generatedObjectMemento.rotation);
        go.transform.localScale = generatedObjectMemento.scale;

        return action;
    }

    /// <summary>
    /// The method that wraps 'Dummiesmans' object loader.
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    public static GeneratedObject GenerateObjectFromFile(IFile file, Transform parent, bool editable)
    {
        GameObject gameObject = new Dummiesman.OBJLoader().Load(file.Path());
        GeneratedObject generatedObject = gameObject.AddComponent<GeneratedObject>();
        if (editable)
        {
            Manipulatable manipulatable = gameObject.AddComponent<Manipulatable>();
        }

        BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();

        Vector3 boxSize = gameObject.GetComponentInChildren<MeshRenderer>().bounds.size;
        boxCollider.size = boxSize;
        boxSize.x = 0; boxSize.z = 0;
        boxCollider.center = boxSize / 3;

        generatedObject.Setup(file);
        generatedObject.transform.SetParent(parent);

        file.SetLastAccessTime();



        gameObject.transform.localScale = ObjectManager.GeneratedObjectScale;
        return generatedObject;
    }
}
