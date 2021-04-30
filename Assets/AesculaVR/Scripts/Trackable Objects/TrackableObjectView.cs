using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackableObjectView : FileBrowserView
{
    public FileManager FileManager => mainManager.TrackableObjectManager.FileManager;

#pragma warning disable 0649
    [SerializeField] private TrackersView trackersView;
#pragma warning restore 0649
    private MainManager mainManager;

    protected override void Awake()
    {
        base.Awake();
        this.mainManager = MainManager.GetManager();
    }


    protected override FileManager GetFileManager() => mainManager.TrackableObjectManager.FileManager;

    public override void SetupFileView(FileView view, IFile file, Color color)
    {
        base.SetupFileView(view, file, color);
        ((TrackableObjectFileView)view).Setup(file, color, this);
    }

    /// <summary>
    /// Load a file.
    /// </summary>
    /// <param name="file">The file to load.</param>
    public void Load(IFile file)
    {
        mainManager.TrackableObjectManager.LoadTrackableObject(file, trackersView.Tracker);
    }
}
