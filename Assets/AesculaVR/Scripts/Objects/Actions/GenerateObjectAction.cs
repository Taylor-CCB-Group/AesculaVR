using Dummiesman;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This Action generates an object from an File
/// </summary>
public class GenerateObjectAction : IActionDereferenceable
{


    private readonly GeneratedObject generatedObject;
    public GeneratedObject GeneratedObject { get { return generatedObject; } }

    public EditorManager editorManager;



    /// <summary>
    /// The Constructor for GenerateObjectAction.
    /// </summary>
    /// <param name="objectManager"> The Object Manager trying to create the object </param>
    /// <param name="file"> The .obj file. </param>
    public GenerateObjectAction(IFile file, Transform parent, bool editable)
    {
        editorManager = EditorManager.GetManager();
        generatedObject = GeneratedObject.GenerateObjectFromFile(file, parent, editable);
        generatedObject.gameObject.SetActive(false);
    }

    public string Description() => "Generate an object from the '" + generatedObject.File.Name(false) + "' file.";

    public void DoAction()
    {
        Debug.Log("GenerateObjectAction : DoAction");
        generatedObject.gameObject.SetActive(true);
        editorManager?.ObjectManager.AddGeneratedObject(generatedObject);
    }

    public void OnDereferenced()
    {
        GameObject.Destroy(generatedObject.gameObject);
    }

    public void UndoAction()
    {
        Debug.Log("GenerateObjectAction : UndoAction");
        generatedObject.gameObject.SetActive(false);
        editorManager?.ObjectManager.RemoveGeneratedObject(generatedObject);
    }
}
