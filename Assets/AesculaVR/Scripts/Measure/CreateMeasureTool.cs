using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMeasureTool : ITool
{

    private CreateMeasureAction createMeasureAction;
    private EditorManager editorManager;
    private MeasureManager.MeasureType type;

    private Transform tip => editorManager.ToolManager.Tip;
    private Measure measure => createMeasureAction.Measure;
    private Transform movableMeasurePoint => ((Measure)measure).PointB.transform;

    private Sprite icon;

    public CreateMeasureTool(MeasureManager.MeasureType type)
    {
        editorManager = EditorManager.GetManager();
        this.type = type;
        icon = null;

        switch (type)
        {
            case MeasureManager.MeasureType.Plane:
                icon = (Sprite)Resources.Load<Sprite>("Icons/CreatePlane");
                break;
            case MeasureManager.MeasureType.Vector:
                icon = (Sprite)Resources.Load<Sprite>("Icons/CreateVector");
                break;
            case MeasureManager.MeasureType.Point:
                icon = (Sprite)Resources.Load<Sprite>("Icons/CreatePoint");
                break;
            default:
                throw new System.NotSupportedException();

        }

    }

    #region ITool
    public void OnDeselected()
    {
        if (createMeasureAction != null)
            TriggerUp();
    }

    public void OnSelected()
    {

    }

    public void OnUpdate()
    {
    }

    public void TriggerDown()
    {
        createMeasureAction = new CreateEditableMeasureAction(type, editorManager.TrackerManager.Main.transform);
        measure.PointA.transform.position = tip.transform.position;        
        if (measure is Measure)
            ((Measure)measure).PointB.transform.position = tip.transform.position;
        createMeasureAction.DoAction();
    }

    public void TriggerUp()
    {
        measure.SetManipulatablesEnabled(true);
        editorManager.ActionManager.DoAction(createMeasureAction, true);
        createMeasureAction = null;
    }

    public void TriggerUpdate()
    {
        if (measure is Measure)
            movableMeasurePoint.position = tip.position;
    }

    public Sprite Icon() => icon;
    #endregion
}
