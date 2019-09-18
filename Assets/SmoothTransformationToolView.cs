using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmoothTransformationToolView : LateObserver
{
    [SerializeField] private Toggle toggle;
    [SerializeField] private SliderLookupValues slider;

    private EditorManager editorManager;


    private void Awake()
    {
        this.editorManager = EditorManager.GetManager();

        toggle.onValueChanged.AddListener(ToggleValue);
        slider.OnReleased.AddListener(SetTool);

    }

    private void Start()
    {
        this.editorManager.ToolManager.AddObserver(this);
    }

    private void SetTool()
    {
        editorManager.ToolManager.SetTool(new SmoothMovementTool((int)slider.Value));
    }

    private void ToggleValue(bool value)
    {
        if (value)
            SetTool();
        else
            editorManager.ToolManager.SetMode(ToolManager.Mode.Pointer);
    }

    public override void LateNotify(object Sender, EventArgs args)
    {
        this.toggle.SetIsOnWithoutNotify(editorManager.ToolManager.ActiveTool is SmoothMovementTool);
    }
}
