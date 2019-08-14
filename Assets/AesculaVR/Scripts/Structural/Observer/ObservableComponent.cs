using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObservableComponent : MonoBehaviour, IObservable
{

    private List<IObserver> observersList    = new List<IObserver>();
    private HashSet<IObserver> observersHash = new HashSet<IObserver>();

    public void NotifyObservers() => NotifyObservers(null);

    public void NotifyObservers(EventArgs args)
    {
        int count = observersList.Count;
        for (int i = 0; i < count; i++)
        {
            observersList[i].Notify(this, args);
        }
    }

    public bool RemoveObserver(IObserver observer)
    {
        if (!observersHash.Contains(observer))
            return false;

        observersList.Remove(observer);
        observersHash.Remove(observer);

        return true;
    }

    public bool AddObserver(IObserver observer)
    {
        if (observersHash.Contains(observer))
            return false;

        observersList.Add(observer);
        observersHash.Add(observer);

        return true;
    }

    int IObservable.Count() => observersList.Count;
}
