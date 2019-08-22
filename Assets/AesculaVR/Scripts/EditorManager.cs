    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorManager : MonoBehaviour
{

    private static EditorManager manager;
    public static EditorManager GetManager()
    {
        if (manager == null)
        {
            manager = GameObject.FindObjectOfType<EditorManager>();
            manager.Setup();
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

    public ObjectManager          ObjectManager { get { return objectManager; } }
    public ActionManager          ActionManager { get { return actionManager; } }
    public TrackableObjectManager TrackableObjectManager { get { return trackableObjectManager; } }
    public TrackerManager         TrackerManager { get { return trackerManager; } }
    public MeasureManager         MeasureManager { get { return measureManager; } }
    public ToolManager            ToolManager => toolManager;
    public ManipulatableManager   ManipulatableManager => manipulatableManager;

    private void Setup()
    {
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
