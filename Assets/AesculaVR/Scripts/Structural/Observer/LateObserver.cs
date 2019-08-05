using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// On abstract observer, that only needs to be notified at the end of the frame
/// </summary>
public abstract class LateObserver : MonoBehaviour, IObserver
{

    private object Sender;
    private EventArgs args;

    private bool willNotify = false;

    public void Notify(object Sender, EventArgs args)
    {
        this.Sender = Sender;
        this.args = args;

        willNotify = true;
    }

    public virtual void LateUpdate()
    {
        
        if(willNotify)
            LateNotify(Sender, args);

        willNotify = false;
    }

    /// <summary>
    /// Gets called at the end of the frame, if the observer has been notified.
    /// </summary>
    /// <param name="Sender"></param>
    /// <param name="args"></param>
    public abstract void LateNotify(object Sender, EventArgs args);
}
