using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ReplayView : FileBrowserView
{
#pragma warning disable 0649
    [SerializeField] private GameObject stopRecording, startRecording;
#pragma warning restore 0649
    private MainManager mainManager;

    protected override void Awake()
    {
        mainManager = MainManager.GetManager();
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        replayManager.AddObserver(this);

        stopRecording.GetComponentInChildren<Button>().onClick.AddListener(replayManager.StopRecording);
        startRecording.GetComponentInChildren<Button>().onClick.AddListener(replayManager.StartRecording);
    }

    protected override FileManager GetFileManager() => mainManager.ReplayManager.FileManager();
    private ReplayManager replayManager => mainManager.ReplayManager;

    public override void LateNotify(object Sender, EventArgs args)
    {
        base.LateNotify(Sender, args);

        stopRecording .SetActive( replayManager.IsRecording());
        startRecording.SetActive(!replayManager.IsRecording());
    }


}
