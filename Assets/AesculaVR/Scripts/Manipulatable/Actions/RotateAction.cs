using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An action for rotating an Manipulatable
/// </summary>
public struct RotateAction : IAction
{
    private Manipulatable target;
    private Vector3 startRotationEuler;
    private Vector3 endRotationEuler;

    public RotateAction(Manipulatable target, Vector3 startRotationEuler, Vector3 endRotationEuler)
    {
        this.target = target;
        this.startRotationEuler = startRotationEuler;
        this.endRotationEuler = endRotationEuler;

    }

    void IAction.DoAction()
    {
        target.transform.localRotation = Quaternion.Euler(endRotationEuler);
        target.OnTransformation();
    }

    string IAction.Description()
    {
        return "Rotate an Object";
    }

    void IAction.UndoAction()
    {
        target.transform.localRotation = Quaternion.Euler(startRotationEuler);
        target.OnTransformation();
    }
}
