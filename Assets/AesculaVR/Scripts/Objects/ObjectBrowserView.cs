using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A view that allows us to browse all the .obj files.
/// </summary>
public class ObjectBrowserView : FileBrowserView
{
    private ObjectManager objectManager;

    protected override void Awake()
    {
        base.Awake();
        objectManager = MasterManager.GetManager().ObjectManager;
    }

    protected override FileManager GetFileManager() => objectManager.FileManager;
}
