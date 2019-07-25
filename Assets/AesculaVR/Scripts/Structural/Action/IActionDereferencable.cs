using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// If the action creates a GameObject, 
/// Then it must be able to delete it when the action is no longer being referenced by the action manager.
/// </summary>
public interface IActionDereferenceable: IAction
{
    /// <summary>
    /// This method is called when the action is no longer being referenced.
    /// </summary>
    void OnDereferenced();
}
