using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// A tool to select UI elements.
/// </summary>
public class PointerTool : ITool
{

    private ToolManager.UIPointer UIPointer;
    private Sprite icon;

    /// <summary>
    /// Construct the new pointer tool
    /// </summary>
    /// <param name="UIPointer">The UIPointer for the tools hand.</param>
    public PointerTool(ToolManager.UIPointer UIPointer)
    {
        this.UIPointer = UIPointer;
        icon = (Sprite)Resources.Load<Sprite>("Icons/Pointer");
    }

    public Sprite Icon() => icon;

    public void OnDeselected()
    {
        UIPointer.SetEnabled(false);
    }

    public void OnSelected()
    {
        UIPointer.SetEnabled(true);
    }

    public void OnUpdate()
    {
    }

    public void TriggerDown()
    {
    }

    public void TriggerUp()
    {
    }

    public void TriggerUpdate()
    {
    }
}
