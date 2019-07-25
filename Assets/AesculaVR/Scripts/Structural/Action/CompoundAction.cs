using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A compound action is an action that consists of multiple actions, that will be treated as a single action by the action manager.
/// </summary>
public class CompoundAction : IActionDereferenceable
{

    private IAction[] actions;
    private string description;

    /// <summary>
    /// Get a copy of the actions that make up this compund action;
    /// </summary>
    public List<IAction> Actions { get { return new List<IAction>(actions); } }

    public int Count => actions.Length;


    public CompoundAction(List<IAction> actions)
    {
        this.actions = actions.ToArray();
        this.description = "A coumpound action consisting of " + actions.Count + " actions"; 
    }

    public CompoundAction(IAction[] actions)
    {
        this.actions = actions;
        this.description = "A coumpound action consisting of " + actions.Length + " actions";

    }

    public string Description() => description;

    public void DoAction()
    {
        int count = actions.Length;
        for(int i = 0; i < count; i++)
        {
            actions[i].DoAction();
        }
    }

    public void UndoAction()
    {
        int count = actions.Length;
        for (int i = count - 1; i >= 0; i--)
        {
            actions[i].UndoAction();
        }
    }

    public void OnDereferenced()
    {
        int count = actions.Length;
        for (int i = count - 1; i >= 0; i--)
        {
            if (actions[i] is IActionDereferenceable)
                ((IActionDereferenceable)actions[i]).OnDereferenced();
        }
    }
}
