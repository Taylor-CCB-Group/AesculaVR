﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TrackableObjectView : FileBrowserView
{

    public enum FilingMode { Load, Delete }
    public FilingMode Mode { get { return filesToggle.Value == 0 ? FilingMode.Load : FilingMode.Delete; } }

    private TrackableObjectManager trackableObjectManager;
    [SerializeField] private RadioToggle filesToggle;
    [SerializeField] private Dialog dialog;
    [SerializeField] private Keyboard keyboard;

    [SerializeField] private Button clearBtn, saveBtn;
    


    private MasterManager masterManager;

    protected override void Awake()
    {
        base.Awake();
        trackableObjectManager = MasterManager.GetManager().TrackableObjectManager;
        masterManager = MasterManager.GetManager();

        clearBtn.onClick.AddListener(Clear);
        saveBtn.onClick.AddListener(Save);
    }

    protected override FileManager GetFileManager() => trackableObjectManager.FileManager;

    public override void SetupFileView(FileView view, IFile file, Color color)
    {        
        ((TrackableObjectFileView)view).Setup(file, color,this);
    }

    /// <summary>
    /// Loads a file.
    /// </summary>
    /// <param name="file">The file to load.</param>
    public void Load(IFile file) => trackableObjectManager.Load(file);
    
    /// <summary>
    /// Deletes a file.
    /// </summary>
    /// <param name="file">The file to delete.</param>
    public void Delete(IFile file)
    {
        dialog.Show(DeleteAction(file), "Delete File?", "are you sure you want to delete the file ''?, this action cannot be undone.");
    }

    /// <summary>
    /// Remove every generated object in the scene
    /// </summary>
    public void Clear() => trackableObjectManager.Clear();

    /// <summary>
    /// show the keyboard, get text, save a file.
    /// </summary>
    public void Save() => keyboard.Show(SaveAction());

    private UnityAction DeleteAction(IFile file)
    {
        return new UnityAction(() => { trackableObjectManager.Delete(file); });
    }
     
    private UnityAction<string> SaveAction()
    {
        return new UnityAction<string>((string value) => { trackableObjectManager.Save(value); });
    }

}
