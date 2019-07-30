using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Managers all the files around trackable objects.
/// </summary>
public class TrackableObjectFileManager : FileManager, IObserver, IFileSortable
{
    /*
     * Todo:
     * 1.) This class should also save trackable objects to a file.
    */
    
    public TrackableObjectFileManager() :
        base(
        Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"/AesculaVR/TrackableObjects", 
        ".trackedObj"
        )
    {

    }

}
