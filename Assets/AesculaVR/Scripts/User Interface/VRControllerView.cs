using System;
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

    private EditorManager editorManager;
    private MainManager mainManager;

    protected void Awake()
    {

        mainManager = MainManager.GetManager();

        editorManager = EditorManager.GetManager();
        editorManager?.ToolManager.AddObserver(this);

        radialMenu.buttons.Clear();
        radialMenu.AddButton(UndoButton());
        radialMenu.AddButton(RedoButton());
    }

    public override void LateNotify(object Sender, EventArgs args)
    {
        if (editorManager)
        {
            ToolManager toolManager = editorManager.ToolManager;
            primaryImage.sprite = toolManager.ActiveTool?.Icon();

            secondaryRoot.SetActive(toolManager.CanToggleMode());
            secondaryImage.overrideSprite =
                (toolManager.ToolMode == ToolManager.Mode.Pointer) ?
                secondaryImage.sprite = toolManager.Tool?.Icon() :
                toolManager.Pointer.Icon();
        }  

    }

    /// <summary>
    /// Generate The undo button
    /// </summary>
    /// <returns> the undo button. </returns>
    private VRTK_RadialMenu.RadialMenuButton UndoButton()
    {
        VRTK_RadialMenu.RadialMenuButton button = new VRTK_RadialMenu.RadialMenuButton();
        button.ButtonIcon = undoIcon;
        button.OnClick.AddListener(Undo);
        return button;
    }

    /// <summary>
    /// Generate The redo button
    /// </summary>
    /// <returns> the redo button. </returns>
    private VRTK_RadialMenu.RadialMenuButton RedoButton()
    {
        VRTK_RadialMenu.RadialMenuButton button = new VRTK_RadialMenu.RadialMenuButton();
        button.ButtonIcon = redoIcon;
        button.OnClick.AddListener(Redo);
        return button;
    }

    /// <summary>
    /// if possible, Undo an action
    /// </summary>
    private void Undo()
    {
        if (editorManager)
        {
            if (editorManager.ActionManager.CanUndo())
                editorManager.ActionManager.UndoAction();

        }

        if (mainManager)
        {
            if (mainManager.ActionManager.CanUndo())
                mainManager.ActionManager.UndoAction();
        }
    }

    /// <summary>
    /// if possible, redo an action
    /// </summary>
    private void Redo()
    {
        if (editorManager)
        {
            if (editorManager.ActionManager.CanRedo())
                editorManager.ActionManager.RedoAction();
        }
        if (mainManager)
        {
            if (mainManager.ActionManager.CanRedo())
                mainManager.ActionManager.RedoAction();
        }
    }

}
