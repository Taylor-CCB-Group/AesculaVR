using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A way to track files in a directory.
/// </summary>
public class FileModel : ObservableObject
{

    #region private variables

    private readonly string directory  = "";
    private readonly string extension = "";
    
    private FileSorter.SortDirection sortDirection;
    private FileSorter.SortField sortField;

    private List<IFile> files;

    #endregion

    /// <summary>
    ///  The Directory this file model is looking at.
    /// </summary>
    public string Directory { get { return directory; } }
    /// <summary>
    /// The Extension we're looking for.
    /// </summary>
    public string Extension { get { return extension; } }

    /// <summary>
    /// The Direction the files will be return in.
    /// </summary>
    public FileSorter.SortDirection SortDirection
    {
        get { return this.sortDirection; }
        set { this.sortDirection = value; Refresh(); }
    }
    /// <summary>
    /// The field we sort bu.
    /// </summary>
    public FileSorter.SortField SortField
    {
        get { return this.sortField; }
        set { this.sortField = value; Refresh(); }
    }

    /// <summary>
    /// A copy of the files in this model.
    /// </summary>
    public List<IFile> Files { get { return new List<IFile>(files); } }
    /// <summary>
    /// a reference to the files in this model.
    /// </summary>
    public List<IFile> FilesReference { get { return files; } }

    /// <summary>
    /// reload all the files, do this when something in the directory changes or when you want to reorder the list.
    /// </summary>
    public void Refresh()
    {
        files = FileSorter.SortFiles(FileLoader.GetFiles(directory, extension), sortField, sortDirection);
        NotifyObservers();  
    }


    /// <summary>
    /// This model will track all files in the directory
    /// </summary>
    /// <param name="directory"> the directory we want to look at. </param>
    public FileModel(string directory) : base()
    {
        this.directory = directory;
        this.extension = string.Empty;

        sortField = FileSorter.SortField.Created;
        sortDirection = FileSorter.SortDirection.Descending;

        Refresh();
    }

    /// <summary>
    /// This model will track all files in the directory.
    /// </summary>
    /// <param name="directory"> The directory we want to look at</param>
    /// <param name="sortField"> The field we want to sort by.</param>
    /// <param name="sortDirection"> The direction we want to sort by. </param>
    public FileModel(string directory, FileSorter.SortField sortField, FileSorter.SortDirection sortDirection) : base()
    {
        this.directory = directory;
        this.extension = string.Empty;

        this.sortField = sortField;
        this.sortDirection = sortDirection;

        Refresh();
    }

    /// <summary>
    /// A model that will track all the files in a directory with the extension
    /// </summary>
    /// <param name="directory">The directory we want to look at.</param>
    /// <param name="extension">The extension we want to track.</param>
    public FileModel(string directory, string extension) : base()
    {
        this.directory = directory;
        this.extension = extension;

        sortField     = FileSorter.SortField.Created;
        sortDirection = FileSorter.SortDirection.Descending;

        Refresh();
    }

    /// <summary>
    /// A model that will track all the files in a directory with the extension
    /// </summary>
    /// <param name="directory">The directory we want to look at.</param>
    /// <param name="extension">The extension we want to track.</param>
    /// <param name="sortField">The field we want to sort by</param>
    /// <param name="sortDirection">the sort direction</param>
    public FileModel(string directory, string extension, FileSorter.SortField sortField, FileSorter.SortDirection sortDirection) : base()
    {
        this.directory = directory;
        this.extension = extension;

        this.sortField     = sortField;
        this.sortDirection = sortDirection;

        Refresh();
    }

}
