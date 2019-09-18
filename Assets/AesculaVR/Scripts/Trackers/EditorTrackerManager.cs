using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorTrackerManager : TrackerManager
{

    public EditorTrackerManager() : base()
    {

    }

    public void SetMainIndex(int index)
    {
        if (index >= trackers.Count && index != 0)
            throw new System.IndexOutOfRangeException();

        this.mainTrackerIndex = index;
        NotifyObservers();
    }

    public override void Remove(Tracker tracker)
    {
        int trackerIndex = trackers.IndexOf(tracker);
        if (mainTrackerIndex >= trackers.Count - 1)
            mainTrackerIndex = (trackers.Count ==1) ? 0 : trackers.Count - 2;
        
        base.Remove(tracker);
    }

}
