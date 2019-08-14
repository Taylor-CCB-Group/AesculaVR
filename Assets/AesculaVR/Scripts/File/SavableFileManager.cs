using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A File Manager that can save and load IMemento.
/// <remark>
/// The IMemento must follow the rules for unity serialisation.
/// </remark>
/// </summary>
public abstract class SavableFileManager : FileManager
{

    private string directory => this.fileModel.Directory;

    public SavableFileManager(string directory, string fileExtension) : base(directory, fileExtension)
    {
        
    }

    /// <summary>
    /// Save a Memento to a file.
    /// </summary>
    /// <param name="fileName">The name of the file, without the extention.</param>
    /// <param name="data">The data to save.</param>
    public void Save<T>(string fileName, IMemento data) where T : IMemento
    {
        string path = directory + "/" + fileName + fileModel.Extension;
        T saveData = (T)data;
        System.IO.File.WriteAllText(path, JsonUtility.ToJson(saveData,true));

        fileModel.Refresh();
        NotifyObservers();
    }

    /// <summary>
    /// Loads in a file. 
    /// <remark>
    /// The File must be a JSON file that matches type T.
    /// </remark>
    /// </summary>
    /// <typeparam name="T">The format of the Json in the file, must be an IMemento</typeparam>
    /// <param name="file">The file to load</param>
    /// <returns>The IMemento from the file</returns>
    public T Load<T> (IFile file) where T : IMemento
    {
        string jsonStr = System.IO.File.ReadAllText(file.Path());
        IMemento data = JsonUtility.FromJson<T>(jsonStr) as IMemento;

        return (T)data;
    }


    /// <summary>
    /// Delete a file.
    /// </summary>
    /// <param name="file"></param>
    public void Delete(IFile file)
    {
        Debug.Log("deleting " + file.Path());
        System.IO.File.Delete(file.Path());
        this.Refresh();
    }
}
