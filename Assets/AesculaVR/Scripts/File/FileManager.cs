using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An abstract class used to wrap a file model.
/// </summary>
public abstract class FileManager : ObservableObject, IObserver, IFileSortable
{
    protected FileModel fileModel;

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
    public List<IFile> Files => fileModel.Files;
    /// <summary>
    /// Get a refrence of all the files in the file model.
    /// </summary>
    public List<IFile> FilesReference => fileModel.FilesReference;

    /// <summary>
    /// update the file model.
    /// </summary>
    public void Refresh() => fileModel.Refresh();
    #endregion


    public FileManager(string directory, string fileExtension) : base()
    {
        fileModel = new FileModel(
            directory,
            fileExtension
            );

        fileModel.AddObserver(this);
    }

    public void Notify(object Sender, EventArgs args)
    {
        this.NotifyObservers(Sender, args);
    }
}
