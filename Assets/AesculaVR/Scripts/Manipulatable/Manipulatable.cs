using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// An Object that can be manlipulated, moved, scaled and rotated.
/// </summary>
public class Manipulatable : MonoBehaviour, IManipulatable
{

    /// <summary>
    /// What events does the manlipulatable have?
    /// </summary>
    public enum Event { Started, Ended, Transformation }

    private UnityEvent OnTransformationStartedEvent;
    private UnityEvent OnTransformationEndedEvent;
    private UnityEvent OnTransformationEvent;

    private bool isTransforming;

    /// <summary>
    /// Add an action to an event
    /// </summary>
    /// <param name="type">What event do you want to add to?</param>
    /// <param name="action">The action you want to add.</param>
    public void AddListner(Event type, UnityAction action)
    {
        switch (type)
        {
            case Event.Ended:
                OnTransformationEndedEvent.AddListener(action);
                break;
            case Event.Started:
                OnTransformationStartedEvent.AddListener(action);
                break;
            case Event.Transformation:
                OnTransformationEvent.AddListener(action);
                break;
        }
    }

    /// <summary>
    /// Remove an action from an event
    /// </summary>
    /// <param name="type">What event do you want to remove from?</param>
    /// <param name="action">The action you want to remove.</param>
    public void RemoveListner(Event type, UnityAction action)
    {
        switch (type)
        {
            case Event.Ended:
                OnTransformationEndedEvent.RemoveListener(action);
                break;
            case Event.Started:
                OnTransformationStartedEvent.RemoveListener(action);
                break;
            case Event.Transformation:
                OnTransformationEvent.RemoveListener(action);
                break;
        }
    }

    protected virtual void Awake()
    {
        isTransforming = false;
        OnTransformationStartedEvent = new UnityEvent();
        OnTransformationEndedEvent   = new UnityEvent();
        OnTransformationEvent        = new UnityEvent();

        
    }

    #region IManipulatable

    public bool IsTransforming() => isTransforming;

    public virtual void OnTransformation()
    {
        OnTransformationEvent.Invoke();
    }

    public virtual void OnTransformationEnded()
    {
        isTransforming = false;
        OnTransformationEndedEvent.Invoke();
    }

    public virtual void OnTransformationStarted()
    {
        isTransforming = true;
        OnTransformationStartedEvent.Invoke();
    }

    #endregion
}


