using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An Object that was created from an .Obj file.
/// </summary>
public class GeneratedObject : MonoBehaviour
{
    private IFile orginalFile;
    public IFile File { get { return orginalFile; } }

    public void Setup(IFile orginalFile)
    {
        this.orginalFile = orginalFile;
        this.gameObject.name = orginalFile.Name(false);
    }
}
