using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Managers all the files around trackable objects.
/// </summary>
public class TrackableObjectFileManager : ObservableObject, IObserver, IFileSortable
{
    /*
     * Todo:
     * 1.) This class should also save trackable objects to a file.
    */

    private FileModel fileModel;
    
    #region IFileSortable
    public FileSorter.SortBy GetSortBy() => ((IFileSortable)fileModel).GetSortBy(); 

    public FileSorter.SortDirection GetSortDirection() => ((IFileSortable)fileModel).GetSortDirection(); 


    public void SetSortBy(FileSorter.SortBy value) => ((IFileSortable)fileModel).SetSortBy(value);

    public void SetSortDirection(FileSorter.SortDirection value) => ((IFileSortable)fileModel).SetSortDirection(value);
    #endregion

    #region File Model methods

    /// <summary>
    /// Get a copy of all the Files from the File model.
    /// </summary>
    public List<IFile> Files          => fileModel.Files;
    /// <summary>
    /// Get a refrence of all the files in the file model.
    /// </summary>
    public List<IFile> FilesReference => fileModel.FilesReference;

    /// <summary>
    /// update the file model.
    /// </summary>
    public void Refresh() => fileModel.Refresh();

    #endregion

    public TrackableObjectFileManager()
    {
        fileModel = new FileModel(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"/AesculaVR/TrackableObjects",
            ".trackedObj"
            );

        fileModel.AddObserver(this);
    }

    public void Notify(object Sender, EventArgs args)
    {
        this.NotifyObservers();
    }
}
