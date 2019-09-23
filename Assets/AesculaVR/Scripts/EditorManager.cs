using System.Collections;
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


    private ActionManager                actionManager;
    private TrackableObjectEditorManager trackableObjectManager;
    private EditorTrackerManager trackerManager;
    private ToolManager                  toolManager;
    private ManipulatableManager         manipulatableManager;

    public ObjectManager                ObjectManager   => trackableObjectManager.SelectedObject.ObjectManager;
    public ActionManager                ActionManager   => actionManager;
    public EditorTrackerManager TrackerManager  => trackerManager;
    public MeasureManager               MeasureManager  => trackableObjectManager.SelectedObject.MeasureManager;
    public ToolManager                  ToolManager     => toolManager;
    public TrackableObjectEditorManager TrackableObjectManager  => trackableObjectManager;
    public ManipulatableManager         ManipulatableManager    => manipulatableManager;

    private void Setup()
    {

        ///Make sure setup can only be called once, otherwise we'll get a recursive problem.
        if (isSetup)
            return;
        isSetup = true;

        actionManager = new ActionManager();
        trackerManager = new EditorTrackerManager();
        trackableObjectManager = new TrackableObjectEditorManager(this);



        toolManager          = GameObject.FindObjectOfType<ToolManager>();
        manipulatableManager = GameObject.FindObjectOfType<ManipulatableManager>();
        
    }

    //object manager and measureManager should be under trackableObjectManager
}
