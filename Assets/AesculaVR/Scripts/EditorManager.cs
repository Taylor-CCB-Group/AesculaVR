﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorManager : MonoBehaviour
{

    private static EditorManager manager;

    /// <summary>
    /// Get the EditorManager from within the current scene.
    /// </summary>
    /// <returns></returns>
    public static EditorManager GetManager()
    {
        if (manager == null)
        {
            manager = GameObject.FindObjectOfType<EditorManager>();
            manager?.Setup();
        }
        return manager;
    }

    private bool isSetup = false;

    private ObjectManager          objectManager;
    private ActionManager          actionManager;
    private TrackableObjectManager trackableObjectManager;
    private TrackerManager         trackerManager;
    private MeasureManager         measureManager;
    private ToolManager            toolManager;
    private ManipulatableManager   manipulatableManager;

    public ObjectManager            ObjectManager   => objectManager;
    public ActionManager            ActionManager   => actionManager;
    public TrackerManager           TrackerManager  => trackerManager;
    public MeasureManager           MeasureManager  => measureManager;
    public ToolManager              ToolManager     => toolManager;
    public TrackableObjectManager   TrackableObjectManager  => trackableObjectManager;
    public ManipulatableManager     ManipulatableManager    => manipulatableManager;

    private void Setup()
    {

        ///Make sure setup can only be called once, otherwise we'll get a recursive problem.
        if (isSetup)
            return;
        isSetup = true;

        objectManager = new ObjectManager();
        actionManager = new ActionManager();
        trackableObjectManager = new TrackableObjectManager(this);
        trackerManager = new TrackerManager();
        measureManager = new MeasureManager();

        toolManager          = GameObject.FindObjectOfType<ToolManager>();
        manipulatableManager = GameObject.FindObjectOfType<ManipulatableManager>();
        
    }

}