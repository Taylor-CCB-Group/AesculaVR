using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorTrackerManager : TrackerManager
{

    public EditorTrackerManager() : base()
    {

    }

    public void SetMainIndex(int index, bool notifyObservers = true)
    {
        if (index >= trackers.Count && index != 0)
            throw new System.IndexOutOfRangeException();

        this.SetTrackerActive(index);
    }

    public override void Remove(Tracker tracker)
    {
        int trackerIndex = trackers.IndexOf(tracker);

        if (mainTrackerIndex >= trackers.Count - 1)
            SetTrackerActive((trackers.Count ==1) ? 0 : trackers.Count - 2);
                
        base.Remove(tracker);
        NotifyObservers();
    }

}
