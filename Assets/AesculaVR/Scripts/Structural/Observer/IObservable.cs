using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Observables can be observered by any IObserver,  When NotifyObservers is called each IObserver will have its "notify" method called.
/// </summary>
public interface IObservable
{
    /// <summary>
    /// Add an observer to be watch this object.
    /// </summary>
    /// <param name="observer">The IObserver to add</param>
    /// <returns>true if the object is added, false if the object cannot be added.</returns>
    bool AddObserver(IObserver observer);

    /// <summary>
    /// Removes an obsever thats watching this object.
    /// </summary>
    /// <param name="observer"></param>
    /// <returns></returns>
    bool RemoveObserver(IObserver observer);

    /// <summary>
    /// Notifys every IObserver watching this object
    /// </summary>
    void NotifyObservers();

    /// <summary>
    /// Notifys every IObserver watching this object
    /// </summary>
    /// <param name="args">Arguments to send each IObserver</param>
    void NotifyObservers(EventArgs args);

    /// <summary>
    /// The Number of IObservers watching this object.
    /// </summary>
    /// <returns>The Number of IObservers watching this object.</returns>
    int Count();
}
