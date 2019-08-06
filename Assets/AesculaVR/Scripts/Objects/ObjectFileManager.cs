using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dummiesman;

public class ObjectFileManager : FileManager
{

    public ObjectFileManager() : 
        base(
        Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"/AesculaVR/Objects",
        ".Obj"
        )
    {
    }



}
