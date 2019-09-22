using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ReplayManager : ObservableComponent
{
    

  

    private FramePool framePool;
    private Recording recording;
    private ReplayFileManager fileManager;

    public FileManager FileManager() => fileManager;

    private void Awake()
    {
        framePool = new FramePool();
        fileManager = new ReplayFileManager();
    }

    private void LateUpdate()
    {
        if(IsRecording())
            recording.AddFrame((KeyFrame)framePool.Pop());
    }

    private bool isRecording;
    public bool IsRecording() => isRecording;
    
    /// <summary>
    /// Create a new recording.
    /// </summary>
    public void StartRecording()
    {
        Debug.Assert(recording == null);
        recording = new Recording();
        isRecording = true;
        NotifyObservers();
    }

    /// <summary>
    /// Stop recording and save the data to a file.
    /// </summary>
    public void StopRecording()
    {
        Debug.Assert(recording != null);
        isRecording = false;
       
        SaveRecording(DateTime.Now.ToString("yyyyddmmhhmmssf"), recording);
        recording.Clear(this.framePool);

        recording = null;
        NotifyObservers();
    }

    public void SaveRecording(string filename, Recording recording) => fileManager.Save<Recording.Memento>(filename, recording.SaveMemento());

}
