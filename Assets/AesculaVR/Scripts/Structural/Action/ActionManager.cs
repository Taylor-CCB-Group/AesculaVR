using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Managers the order and history of actions, This allows use to undo and redo actions.
/// </summary>
public class ActionManager : ObservableObject
{
    private List<IAction> pastActions;
    private List<IAction> futureActions;

    public List<IAction> PastActions { get { return new List<IAction>(pastActions); } }
    public List<IAction> PastActionsReference { get { return pastActions; } }

    public List<IAction> FutureActions { get { return new List<IAction>(futureActions); } }
    public List<IAction> FutureActionsReference { get { return futureActions; } }


    public ActionManager() : base()
    {
        this.pastActions = new List<IAction>();
        this.futureActions = new List<IAction>();
    }

    /// <summary>
    /// Do a new Action
    /// </summary>
    /// <param name="action"> The new action </param>
    /// <param name="hasActionBeenDone"> has the action already been done, and it just needs recording? </param>
    public void DoAction(IAction action, bool hasActionBeenDone = false)
    {
        if (!hasActionBeenDone)
            action.DoAction();

        pastActions.Add(action);

        if (CanRedo())
            this.futureActions.Clear();

        NotifyObservers();
    }

    /// <summary>
    /// Undo the latest action
    /// </summary>
    public void UndoAction()
    {
        Debug.Assert(CanUndo(), "We're trying to undo an action when there's no actions to undo");

        IAction action = pastActions[pastActions.Count - 1];
        pastActions.RemoveAt(pastActions.Count - 1);
        action.UndoAction();
        futureActions.Add(action);

        NotifyObservers();
    }

    /// <summary>
    /// redo the last action that was done.
    /// </summary>
    public void RedoAction()
    {
        Debug.Assert(CanRedo(), "We're trying to redo an action when there's no actions to redo");

        IAction action = futureActions[futureActions.Count - 1];
        futureActions.RemoveAt(futureActions.Count - 1);

        action.DoAction();
        pastActions.Add(action);

        NotifyObservers();
    }

    /// <summary>
    /// is there an action to undo?
    /// </summary>
    /// <returns> past actions count is larger then 0. </returns>
    public bool CanUndo() { return pastActions.Count > 0; }

    /// <summary>
    /// is there an action to redo?
    /// </summary>
    /// <returns> future actions count is larger then 0. </returns>
    public bool CanRedo() { return futureActions.Count > 0; }

    /// <summary>
    /// How many actions have been undone?
    /// </summary>
    public int FutureActionsCount   => futureActions.Count;
    /// <summary>
    /// How many actions have been done?
    /// </summary>
    public int PastActionsCount => pastActions.Count;
}
