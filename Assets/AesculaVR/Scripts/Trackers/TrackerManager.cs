﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Managers all the vive trackers in the scene.
/// </summary>
public class TrackerManager : ObservableObject
{
    private List<Tracker> trackers;
    private HashSet<Tracker> trackersHash;

    public List<Tracker> Trackers { get { return new List<Tracker>(trackers); } }
    public List<Tracker> TrackersReference { get { return trackers; } }
    
    public Tracker Main { get { return (trackers.Count > 0) ? trackers[0] : null;} }

    public TrackerManager()
    {
        trackers     = new List<Tracker>();
        trackersHash = new HashSet<Tracker>();
    }

    public void Add(Tracker tracker)
    {
        Debug.Log("adding tracker.");

        if (trackersHash.Contains(tracker))
            return;

        trackers    .Add(tracker);
        trackersHash.Add(tracker);

        NotifyObservers();
    }

    public void Remove(Tracker tracker)
    {
        if (!trackersHash.Contains(tracker))
            return;

        trackers    .Remove(tracker);
        trackersHash.Remove(tracker);

        NotifyObservers();
    }

    
}
