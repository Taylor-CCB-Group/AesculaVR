using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// concreate pool for the file object views.   
/// </summary>
public class FileViewPool : ObjectPool
{
    private FileView viewPrefab;

    public FileViewPool(FileView viewPrefab) : base()
    {
        this.viewPrefab = viewPrefab;
    }

    public override void Fill(int amount = 1)
    {
        FileView obj = GameObject.Instantiate(viewPrefab).GetComponent<FileView>();
        this.Push(obj);
    }
}
