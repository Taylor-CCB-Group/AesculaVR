using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An action for scaling an Manipulatable
/// </summary>
public struct ScaleAction : IAction
{
    private Manipulatable target;
    private Vector3 startScale;
    private Vector3 endScale;

    public ScaleAction(Manipulatable target, Vector3 startScale, Vector3 endScale)
    {
        this.target = target;
        this.startScale = startScale;
        this.endScale = endScale;
    }

    void IAction.DoAction()
    {
        target.transform.localScale = endScale;
        target.OnTransformation();
    }

    string IAction.Description()
    {
        return "Scale an Object";
    }

    void IAction.UndoAction()
    {
        target.transform.localScale = startScale;
        target.OnTransformation();
    }
}
