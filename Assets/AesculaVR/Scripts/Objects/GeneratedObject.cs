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
        GenerateObjectAction action = (GenerateObjectAction)editorManager.ObjectManager.GenerateObject(new File(generatedObjectMemento.objPath));
        GeneratedObject go = action.GeneratedObject;
        //set go to be child of tracker.
        go.transform.SetParent(parent);

        go.transform.localPosition = generatedObjectMemento.localPosition;
        go.transform.localRotation = Quaternion.Euler(generatedObjectMemento.rotation);
        go.transform.localScale = generatedObjectMemento.scale;

        return action;
    }
}
