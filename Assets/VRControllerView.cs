﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRTK;

public class VRControllerView : LateObserver
{
#pragma warning disable 0649
    [SerializeField] private VRTK_RadialMenu radialMenu;
    [SerializeField] private Image primaryImage, secondaryImage;
    [SerializeField] private GameObject secondaryRoot;
    [SerializeField] private Sprite  undoIcon, redoIcon;    
#pragma warning restore 0649

    EditorManager editorManager;


    protected void Awake()
    {
        editorManager = EditorManager.GetManager();
        editorManager.ToolManager.AddObserver(this);

        radialMenu.buttons.Clear();
        radialMenu.AddButton(UndoButton());
        radialMenu.AddButton(RedoButton());
    }

    public override void LateNotify(object Sender, EventArgs args)
    {

        ToolManager toolManager = editorManager.ToolManager;
        primaryImage.sprite = toolManager.ActiveTool?.Icon();

        secondaryRoot.SetActive(toolManager.CanToggleMode());
        secondaryImage.overrideSprite = 
            (toolManager.ToolMode == ToolManager.Mode.Pointer) ? 
            secondaryImage.sprite = toolManager.Tool?.Icon() : 
            toolManager.Pointer.Icon();
  

    }


    private VRTK_RadialMenu.RadialMenuButton UndoButton()
    {
        VRTK_RadialMenu.RadialMenuButton button = new VRTK_RadialMenu.RadialMenuButton();
        button.ButtonIcon = undoIcon;
        button.OnClick.AddListener(Undo);
        return button;
    }

    private VRTK_RadialMenu.RadialMenuButton RedoButton()
    {
        VRTK_RadialMenu.RadialMenuButton button = new VRTK_RadialMenu.RadialMenuButton();
        button.ButtonIcon = redoIcon;
        button.OnClick.AddListener(Redo);
        return button;
    }

    private void Undo()
    {
        if (editorManager.ActionManager.CanUndo())
            editorManager.ActionManager.UndoAction();
    }

    private void Redo()
    {
        if (editorManager.ActionManager.CanRedo())
            editorManager.ActionManager.RedoAction();
    }

}
