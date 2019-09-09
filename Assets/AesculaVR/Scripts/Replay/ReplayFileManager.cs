using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplayFileManager : SavableFileManager, IObserver, IFileSortable
{
    public ReplayFileManager() :
          base(
          Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"/AesculaVR/Recordings",
          ".recording"
          )
    {

    }
}
