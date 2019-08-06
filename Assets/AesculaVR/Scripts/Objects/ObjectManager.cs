using Dummiesman;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : ObservableObject, IObserver
{

    private static  Vector3 GeneratedObjectScale { get { return Vector3.one * 0.1f; } }

    #region Object Manager Actions
    /// <summary>
    /// This Action generates an object from an File
    /// </summary>
    private class GenerateObjectAction : IActionDereferenceable
    {

        private readonly ObjectManager objectManager;
        private readonly GeneratedObject generatedObject;
        public GeneratedObject GeneratedObject { get { return generatedObject; } }

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


            gameObject.transform.localScale = GeneratedObjectScale;
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

        public string Description() => "Generate an object from the '"+ generatedObject.File.Name(false)+ "' file.";

        public void DoAction()
        {
            generatedObject.gameObject.SetActive(true);
            objectManager.activeObjects.Add(generatedObject);
        }

        public void OnDereferenced()
        {
            GameObject.Destroy(generatedObject.gameObject);
        }

        public void UndoAction()
        {
            generatedObject.gameObject.SetActive(false);
            objectManager.activeObjects.Remove(generatedObject);
        }
    }

    /// <summary>
    /// This Action destroys an generated object.
    /// </summary>
    private class DestroyGeneratedObjectAction : IAction
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

        public string Description() => "Destroy the object that was generated from the "+generatedObject.File.Name(false)+ "' file.";

        public void DoAction()
        {
            generatedObject.gameObject.SetActive(false);
            objectManager.activeObjects.Remove(generatedObject);
        }

        public void UndoAction()
        {
            generatedObject.gameObject.SetActive(true);
            objectManager.activeObjects.Add(generatedObject);
        }
    }
    #endregion

    private readonly ObjectFileManager fileManager;
    private readonly HashSet<GeneratedObject> activeObjects;
       
    public FileManager FileManager { get { return fileManager; } }


    public ObjectManager()
    {
        this.fileManager = new ObjectFileManager();
        this.fileManager.AddObserver(this);

        this.activeObjects = new HashSet<GeneratedObject>();
    }

    /// <summary>
    /// Create a Generate Object action
    /// </summary>
    /// <param name="file">The file that contains the object we want to make. </param>
    /// <returns>The new Generate Object action</returns>
    public IAction GenerateObject(IFile file) => new GenerateObjectAction(this, file);

    /// <summary>
    /// Create a Destroy Object action
    /// </summary>
    /// <param name="generatedObject"> The generated object we want to destroy. </param>
    /// <returns>The new Destroy Object action</returns>
    public IAction DestroyObject(GeneratedObject generatedObject) => new DestroyGeneratedObjectAction(this, generatedObject);

    public void Notify(object Sender, EventArgs args) => NotifyObservers(Sender, args);
}