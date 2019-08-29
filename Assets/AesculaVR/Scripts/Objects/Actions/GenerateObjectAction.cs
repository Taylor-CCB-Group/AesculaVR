using Dummiesman;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This Action generates an object from an File
/// </summary>
public class GenerateObjectAction : IActionDereferenceable
{

    private readonly ObjectManager objectManager;
    private readonly GeneratedObject generatedObject;
    public GeneratedObject GeneratedObject { get { return generatedObject; } }

    public EditorManager editorManager;

    /// <summary>
    /// The method that wraps 'Dummiesmans' object loader.
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    private GeneratedObject GenerateObject(IFile file)
    {
        GameObject gameObject = new OBJLoader().Load(file.Path());
        GeneratedObject generatedObject = gameObject.AddComponent<GeneratedObject>();
        Manipulatable manipulatable = gameObject.AddComponent<Manipulatable>();
        BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();

        Vector3 boxSize = gameObject.GetComponentInChildren<MeshRenderer>().bounds.size;
        boxCollider.size = boxSize;
        boxSize.x = 0; boxSize.z = 0;
        boxCollider.center = boxSize / 3;

        generatedObject.Setup(file);
        generatedObject.transform.SetParent(EditorManager.GetManager().TrackerManager.Main?.transform);

        file.SetLastAccessTime();



        gameObject.transform.localScale = ObjectManager.GeneratedObjectScale;
        return generatedObject;
    }

    /// <summary>
    /// The Constructor for GenerateObjectAction.
    /// </summary>
    /// <param name="objectManager"> The Object Manager trying to create the object </param>
    /// <param name="file"> The .obj file. </param>
    public GenerateObjectAction(ObjectManager objectManager, IFile file)
    {
        this.objectManager = objectManager;

        generatedObject = GenerateObject(file);
        generatedObject.gameObject.SetActive(false);
    }

    public string Description() => "Generate an object from the '" + generatedObject.File.Name(false) + "' file.";

    public void DoAction()
    {
        Debug.Log("GenerateObjectAction : DoAction");
        generatedObject.gameObject.SetActive(true);
        objectManager.AddGeneratedObject(generatedObject);
    }

    public void OnDereferenced()
    {
        GameObject.Destroy(generatedObject.gameObject);
    }

    public void UndoAction()
    {
        Debug.Log("GenerateObjectAction : UndoAction");
        generatedObject.gameObject.SetActive(false);
        objectManager.RemoveGeneratedObject(generatedObject);
    }
}
