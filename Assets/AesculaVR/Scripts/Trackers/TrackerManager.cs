using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Managers all the vive trackers in the scene.
/// </summary>
public class TrackerManager : ObservableObject
{
    protected List<Tracker> trackers;
    protected HashSet<Tracker> trackersHash;

    public List<Tracker> Trackers { get { return new List<Tracker>(trackers); } }
    public List<Tracker> TrackersReference { get { return trackers; } }
    
    public Tracker Main { get { return (trackers.Count > 0) ? trackers[mainTrackerIndex] : null;} }

    protected int mainTrackerIndex = 0;

    public TrackerManager()
    {
        trackers     = new List<Tracker>();
        trackersHash = new HashSet<Tracker>();
    }

    public virtual void Add(Tracker tracker)
    {
        Debug.Log("adding tracker.");

        if (trackersHash.Contains(tracker))
            return;

        trackers    .Add(tracker);
        trackersHash.Add(tracker);

        NotifyObservers();
    }

    public virtual void Remove(Tracker tracker)
    {
        if (!trackersHash.Contains(tracker))
            return;

        trackers    .Remove(tracker);
        trackersHash.Remove(tracker);

        NotifyObservers();
    }

    
}
