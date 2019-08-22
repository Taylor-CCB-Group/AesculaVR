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
    private Transform movableMeasurePoint => measure.PointB.transform;

    private Sprite icon;

    public CreateMeasureTool(MeasureManager.MeasureType type)
    {
        editorManager = EditorManager.GetManager();
        this.type = type;
        icon = (type == MeasureManager.MeasureType.Vector) ?
            (Sprite)Resources.Load<Sprite>("Icons/CreateVector") :
            (Sprite)Resources.Load<Sprite>("Icons/CreatePlane" ) ;
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
        createMeasureAction = new CreateMeasureAction(type);

        createMeasureAction.Measure.PointA.transform.position = tip.transform.position;
        createMeasureAction.Measure.PointB.transform.position = tip.transform.position;

        measure.SetManipulatablesEnabled(false);
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
        movableMeasurePoint.position = tip.position;
    }

    public Sprite Icon() => icon;
    #endregion
}
