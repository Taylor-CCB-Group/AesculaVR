using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  An abstract object that implements IObservable
/// </summary>
public abstract class ObservableObject : IObservable
{

    private List<IObserver>    observersList;
    private HashSet<IObserver> observersHash;

    public ObservableObject()
    {
        this.observersList = new List<IObserver>();
        this.observersHash = new HashSet<IObserver>();
    }

    /// <summary>
    /// Notify all the observers watching this object.
    /// </summary>
    public void NotifyObservers() => NotifyObservers(null);

    /// <summary>
    /// Notify all the observers watching this object.
    /// </summary>
    /// <param name="args">What data do we want each observer to have?</param>
    public void NotifyObservers(EventArgs args) => NotifyObservers(this, args);

    /// <summary>
    /// Notify all the observers watching this object.
    /// </summary>
    /// <param name="Sender">Who is sending the notify observers message?</param>
    /// <param name="args">What data do we want each observer to have?</param>
    public void NotifyObservers(object Sender, EventArgs args)
    {
        int count = observersList.Count;
        for (int i = 0; i < count; i++)
        {
            observersList[i].Notify(Sender, args);
        }
    }

    /// <summary>
    /// Removes an observer
    /// </summary>
    /// <param name="observer">The observer to remove</param>
    /// <returns>returns true if the observer was removed, false if not.</returns>
    public bool RemoveObserver(IObserver observer)
    {
        if (!observersHash.Contains(observer))
            return false;

        observersList.Remove(observer);
        observersHash.Remove(observer);

        return true;
    }

    /// <summary>
    /// adds an observer
    /// </summary>
    /// <param name="observer">The observer to add.</param>
    /// <returns>returns true if the observer was added, false if not.</returns>
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
