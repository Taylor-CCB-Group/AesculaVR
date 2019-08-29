using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This Action destroys an generated object.
/// </summary>
public class DestroyGeneratedObjectAction : IAction
{
    private readonly ObjectManager objectManager;
    private readonly GeneratedObject generatedObject;
    public GeneratedObject GeneratedObject { get { return generatedObject; } }

    /// <summary>
    /// The Constructor for DestroyGeneratedObjectAction.
    /// </summary>
    /// <param name="objectManager">The object manager trying to destroy the object.</param>
    /// <param name="generatedObject">The Generated object we're trying to destroy.</param>
    public DestroyGeneratedObjectAction(ObjectManager objectManager, GeneratedObject generatedObject)
    {
        this.objectManager = objectManager;
        this.generatedObject = generatedObject;
    }

    public string Description() => "Destroy the object that was generated from the " + generatedObject.File.Name(false) + "' file.";

    public void DoAction()
    {
        generatedObject.gameObject.SetActive(false);
        objectManager.RemoveGeneratedObject(generatedObject);
    }

    public void UndoAction()
    {
        generatedObject.gameObject.SetActive(true);
        objectManager.AddGeneratedObject(generatedObject);
    }
}

