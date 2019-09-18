using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class ToolManager : ObservableComponent
{
    /// <summary>
    /// The VRTK components that enable a UI pointer to work in a scene.
    /// </summary>
    [System.Serializable]
    public struct UIPointer
    {
#pragma warning disable 0649
        [SerializeField] private VRTK_Pointer pointer;
        [SerializeField] private VRTK_UIPointer uiPointer;
        [SerializeField] private VRTK_BasePointerRenderer uiPointerRenderer;
        [SerializeField] private VRTK_ControllerEvents controllerEvents;
#pragma warning restore 0649


        public VRTK_UIPointer Pointer { get { return uiPointer; } }
        public VRTK_BasePointerRenderer Renderer { get { return uiPointerRenderer; } }
        public VRTK_ControllerEvents ControllerEvents => controllerEvents;

        public void SetEnabled(bool value)
        {
            this.pointer.enabled = value;
            this.uiPointer.enabled = value;
            this.uiPointerRenderer.enabled = value;
        }
    }

#pragma warning disable 0649
    [SerializeField] UIPointer hand;
#pragma warning restore 0649

    public Transform Tip => editorManager.ManipulatableManager.Right.Tip;

    public enum Mode { Tool, Pointer }
    private Mode mode;
    public Mode ToolMode => mode;

    private EditorManager editorManager;
    private bool isTriggerDown;

    private ITool activeTool => (mode == Mode.Tool ? setTool : pointerTool);

    private ITool setTool  = null;
    private ITool pointerTool = null;
    
    public ITool ActiveTool => activeTool;
    public ITool Tool => setTool;
    public ITool Pointer => pointerTool;

    /// <summary>
    /// Set the current mode.
    /// </summary>
    /// <param name="mode">The mode we want to set to</param>
    public void SetMode(Mode mode)
    {
        if(mode == Mode.Pointer)
        {
            this.setTool?.OnDeselected();
            this.pointerTool.OnSelected();
        }
        else
        {
            this.pointerTool.OnDeselected();
            this.setTool?.OnSelected();
        }

        this.mode = mode;
        NotifyObservers();
    }

    /// <summary>
    /// Set the current tool.
    /// </summary>
    /// <param name="tool">The tool we want to use.</param>
    public void SetTool(ITool tool)
    {
        Debug.Log("Setting tool to " + tool.ToString());

        if(mode == Mode.Tool)
            this.setTool?.OnDeselected();
        else
        {
            this.pointerTool.OnDeselected();
            this.mode = Mode.Tool;
        }
        this.setTool = tool;
        this.setTool.OnSelected();

        NotifyObservers();
    }
    
    /// <summary>
    /// If we can do so, Toggle the mode between tool and pointer.
    /// </summary>
    public void ToggleMode()
    {
        if (!CanToggleMode())
            return;

        if(this.mode == Mode.Tool)
            SetMode(Mode.Pointer);
        else
        {
            if (setTool == null)
                return;
            else
                SetMode(Mode.Tool);
        }
    }

    /// <summary>
    /// Can we toggle between tool mode and pointer mode? e.g, has the tool been set if we're in pointer mode?
    /// </summary>
    /// <returns></returns>
    public bool CanToggleMode() => !(this.mode == Mode.Pointer && setTool == null);


    private void Awake()
    {
        this.editorManager = EditorManager.GetManager();
        this.pointerTool = new PointerTool(this.hand);
        SetMode(Mode.Pointer);

        hand.ControllerEvents.TriggerClicked += (object sender, ControllerInteractionEventArgs e) => { TriggerDown(); };
        hand.ControllerEvents.TriggerUnclicked += (object sender, ControllerInteractionEventArgs e) => { TriggerUp(); };

        hand.ControllerEvents.ButtonTwoReleased += (object sender, ControllerInteractionEventArgs e) => { ToggleMode(); };
    }


    private void Update()
    {
        this.activeTool?.OnUpdate();

        if (isTriggerDown)
            this.TriggerUpdate();
    }

    /// <summary>
    /// The first frame the trigger is down.
    /// </summary>
    public void TriggerDown()
    {
        isTriggerDown = true;
        this.activeTool?.TriggerDown();
    }


    /// <summary>
    /// Gets called while the trigger is held down,
    /// </summary>
    private void TriggerUpdate()
    {
        this.activeTool?.TriggerUpdate();
    }

    /// <summary>
    /// The first frame of the trigger being released.
    /// </summary>
    public void TriggerUp()
    {
        isTriggerDown = false;
        this.activeTool?.TriggerUp();
    }
}
