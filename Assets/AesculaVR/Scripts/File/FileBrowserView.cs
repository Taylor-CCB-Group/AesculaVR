﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An abstract class that displays a list of views.
/// </summary>
public abstract class FileBrowserView : LateObserver
{
#pragma warning disable 0649
    [SerializeField] private Transform contentRoot;
    [SerializeField] private FileView fileViewPrefab;
    [SerializeField] private Color fileColor1, fileColor2;
    [SerializeField] protected ErrorDialog errorDialog;

    //sorting
    [SerializeField] private SortByDropdown sortByDropdown = null;
    [SerializeField] private OrderByDropdown orderByDropdown = null;
#pragma warning restore 0649

    //re-using memory
    private FileViewPool filePool;
    private Stack<FileView> activeViews;

    /// <summary>
    /// We need a file manager to display files.
    /// </summary>
    /// <returns>The file manager</returns>
    protected abstract FileManager GetFileManager();

    protected virtual void Awake()
    {
        filePool = new FileViewPool(fileViewPrefab);
        activeViews = new Stack<FileView>();
    }

    protected virtual void Start()
    {
        sortByDropdown.AddObserver(this);
        orderByDropdown.AddObserver(this);

        GetFileManager().SetSortBy(sortByDropdown.Value());
        GetFileManager().SetSortDirection(orderByDropdown.Value());

        GetFileManager().AddObserver(this);

        this.Notify(this, null);
    }

    public override void LateNotify(object Sender, EventArgs args)
    {
        try
        {
            GenerateFiles(Sender, args);
        }
        catch(Exception e)
        {
            Debug.Log(e.StackTrace);
            errorDialog.Show(e.Message);
        }
    }

    public virtual void SetupFileView(FileView view, IFile file, Color color)
    {
        view.SetUp(file, color,this.errorDialog);
    }


    /// <summary>
    /// Create the files views.
    /// </summary>
    /// <param name="Sender"></param>
    /// <param name="args"></param>
    private void GenerateFiles(object Sender, EventArgs args)
    {
        while (activeViews.Count > 0)
            filePool.Push(activeViews.Pop());

        if (Sender is SortByDropdown)
            GetFileManager().SetSortBy(sortByDropdown.Value());

        if (Sender is OrderByDropdown)
            GetFileManager().SetSortDirection(orderByDropdown.Value());


        List<IFile> files = GetFileManager().FilesReference;
        for (int i = 0; i < files.Count; i++)
        {
            FileView view = filePool.Pop() as FileView;
            view.transform.SetParent(contentRoot);

            view.transform.localPosition = Vector3.zero;
            view.transform.localScale = Vector3.one;

            SetupFileView(view, files[i], ((i % 2 == 0) ? fileColor1 : fileColor2));

            activeViews.Push(view);

        }
    }
}
