using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using AesculaVR.Manipulations;


[System.Serializable]
public class InputManager
{
    public enum Controller { Left, Right }

    //private vars
    [SerializeField]
    private VRTK_ControllerEvents VRTK_ControllerEvents;
    [SerializeField]
    private Transform tip;
    [SerializeField]
    private Transform children;
    [SerializeField]
    private Controller type;


    //public vars
    public Controller Type { get { return this.type; } }
    public Transform Tip { get { return this.tip; } }
    public Transform Children { get { return this.children; } }
    public Manipulation Manlipulation { get; set; }

    public bool IsTriggerDown { get { return VRTK_ControllerEvents.triggerClicked; } }
    public bool IsGripDown { get { return VRTK_ControllerEvents.gripClicked; } }
    public bool HasChildren { get { return Children.childCount > 0; } }
    public bool IsManlipulating { get { return Manlipulation != null; } }

    //events
    public delegate void OnTriggerClickedEvent(InputManager InputController);
    public delegate void OnTriggerHeldEvent(InputManager InputController);
    public delegate void OnTriggerReleasedEvent(InputManager InputController);

    public delegate void OnGripClickedEvent(InputManager InputController);
    public delegate void OnGripHeldEvent(InputManager InputController);
    public delegate void OnGripUnclickedEvent(InputManager InputController);

    public event OnTriggerClickedEvent OnTriggerClicked;
    public event OnTriggerHeldEvent OnTriggerHeld;
    public event OnTriggerReleasedEvent OnTriggerUnclicked;

    public event OnGripClickedEvent OnGripClicked;
    public event OnGripHeldEvent OnGripHeld;
    public event OnGripUnclickedEvent OnGripUnclicked;

    //methods
    public void Setup()
    {

        //trigger
        VRTK_ControllerEvents.TriggerClicked += TriggerClickHandle;
        VRTK_ControllerEvents.TriggerUnclicked += TriggerUnclickHandle;

        //grip
        VRTK_ControllerEvents.GripClicked += GripClickHandle;
        VRTK_ControllerEvents.GripUnclicked += GripUnclickHandle;

        //events
        OnTriggerClicked += (InputManager ic) => { };
        OnTriggerHeld += (InputManager ic) => { };
        OnTriggerUnclicked += (InputManager ic) => { };

        OnGripClicked += (InputManager ic) => { };
        OnGripHeld += (InputManager ic) => { };
        OnGripUnclicked += (InputManager ic) => { };
    }

    public void Update()
    {
        if (IsTriggerDown)
            this.OnTriggerHeld(this);


        if (IsGripDown)
            this.OnGripHeld(this);
    }

    private void GripClickHandle(object sender, ControllerInteractionEventArgs e) { this.OnGripClicked(this); }
    private void GripUnclickHandle(object sender, ControllerInteractionEventArgs e) { this.OnGripUnclicked(this); }

    private void TriggerClickHandle(object sender, ControllerInteractionEventArgs e) { this.OnTriggerClicked(this); }
    private void TriggerUnclickHandle(object sender, ControllerInteractionEventArgs e) { this.OnTriggerUnclicked(this); }
}