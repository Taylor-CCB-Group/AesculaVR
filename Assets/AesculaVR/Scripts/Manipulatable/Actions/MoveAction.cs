using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An action for moving an Manipulatable
/// </summary>
public struct MoveAction : IAction
{
    private Manipulatable target;
    private Vector3 startPosition;
    private Vector3 endPosition;

    public MoveAction(Manipulatable target, Vector3 startPosition, Vector3 endPosition)
    {
        this.target = target;
        this.startPosition = startPosition;
        this.endPosition = endPosition;

    }

    void IAction.DoAction()
    {
        target.transform.position = endPosition;
        target.OnTransformation();
    }

    string IAction.Description()
    {
        return "Move an object";
    }

    void IAction.UndoAction()
    {
        target.transform.position = startPosition;
        target.OnTransformation();
    }
}
