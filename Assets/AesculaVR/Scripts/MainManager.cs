using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    private static MainManager manager;

    private ActionManager           actionManager;
    private TrackableObjectManager  trackableObjectManager;
    private TrackerManager          trackerManager;
    private ReplayManager           replayManager;

    public ActionManager            ActionManager           => actionManager;
    public TrackableObjectManager   TrackableObjectManager  => trackableObjectManager;
    public TrackerManager           TrackerManager          => trackerManager;
    public ReplayManager            ReplayManager => replayManager;


    /// <summary>
    /// Get the MainManager from within the current scene.
    /// </summary>
    /// <returns></returns>
    public static MainManager GetManager()
    {
        if (manager == null)
        {
            manager = GameObject.FindObjectOfType<MainManager>();
            manager?.Setup();
        }
        return manager;
    }


    private bool isSetup = false;
    private void Setup()
    {
        ///Make sure setup can only be called once, otherwise we'll get a recursive problem.
        if (isSetup)
            return;
        isSetup = true;

        actionManager = new ActionManager();
        trackableObjectManager = new TrackableObjectManager();
        trackerManager = new TrackerManager();
        replayManager = FindObjectOfType<ReplayManager>();

    }

}
