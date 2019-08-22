using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointerTool : ITool
{

    private ToolManager.UIPointer UIPointer;
    private Sprite icon; 

    public PointerTool(ToolManager.UIPointer UIPointer)
    {
        this.UIPointer = UIPointer;
        icon = (Sprite)Resources.Load<Sprite>("Icons/Pointer");
    }

    public Sprite Icon() => icon;

    public void OnDeselected()
    {
        Debug.Log("PointerTool => OnDeselected");
        UIPointer.SetEnabled(false);
    }

    public void OnSelected()
    {
        Debug.Log("PointerTool => OnSelected");
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
