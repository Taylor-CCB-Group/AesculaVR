using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTrackerAction : IAction
{

    private TrackerRadioToggle trackerRadioToggle;
    private int newValue, oldValue;

    public SetTrackerAction(TrackerRadioToggle toggle, int newValue, int oldValue)
    {
        this.trackerRadioToggle = toggle;
        this.newValue = newValue;
        this.oldValue = oldValue;
    }


    public string Description() => "Set an tracker active.";

    public void DoAction() => trackerRadioToggle.SetActiveBase(newValue);

    public void UndoAction() => trackerRadioToggle.SetActiveBase(oldValue);
}

