using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MovementToolView : LateObserver
{
    [SerializeField] private Toggle toggle;
    [SerializeField] private SliderLookupValues slider;
    [SerializeField] private Toggle moveX, moveY, moveZ;

    private EditorManager editorManager;
    private Sprite icon;

    private void Awake()
    {
        this.editorManager = EditorManager.GetManager();
        icon = (Sprite)Resources.Load<Sprite>("Icons/SmoothMove");


        toggle.onValueChanged.AddListener(ToggleValue);

        slider.OnReleased.AddListener(SetTool);
        moveX.onValueChanged.AddListener(SetTool);
        moveY.onValueChanged.AddListener(SetTool);
        moveZ.onValueChanged.AddListener(SetTool);


    }

    private void Start()
    {
        this.editorManager.ToolManager.AddObserver(this);
    }

    private void SetTool()
    {
        editorManager.ToolManager.SetTool(new MovementTool(slider.Value, moveX.isOn, moveY.isOn, moveZ.isOn));
    }

    private void SetTool(bool value) => SetTool();

    private void ToggleValue(bool value)
    {
        if (value)
            SetTool();
        else
            editorManager.ToolManager.SetMode(ToolManager.Mode.Pointer);
    }

    public override void LateNotify(object Sender, EventArgs args)
    {
        this.toggle.SetIsOnWithoutNotify(editorManager.ToolManager.ActiveTool is MovementTool);
    }
}
