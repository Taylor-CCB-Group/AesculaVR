using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Managers all the files around trackable objects.
/// </summary>
public class TrackableObjectFileManager : SavableFileManager, IObserver, IFileSortable
{
    public TrackableObjectFileManager() :
        base(
        Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"/AesculaVR/TrackableObjects", 
        ".trackedObj"
        )
    {

    }

}
