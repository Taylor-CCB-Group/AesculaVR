﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
