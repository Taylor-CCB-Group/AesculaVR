    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterManager
{

    private static MasterManager manager;
    public static MasterManager GetManager()
    {
        if (manager == null)
            manager = new MasterManager();
        return manager;
    }

    private readonly ObjectManager          objectManager;
    private readonly ActionManager          actionManager;
    private readonly TrackableObjectManager trackableObjectManager;
    private readonly TrackerManager         trackerManager;

    public ObjectManager          ObjectManager { get { return objectManager; } }
    public ActionManager          ActionManager { get { return actionManager; } }
    public TrackableObjectManager TrackableObjectManager { get { return trackableObjectManager; } }
    public TrackerManager         TrackerManager { get { return trackerManager; } }

    private MasterManager()
    {
        objectManager = new ObjectManager();
        actionManager = new ActionManager();
        trackableObjectManager = new TrackableObjectManager(this);
        trackerManager = new TrackerManager();
    }

}
