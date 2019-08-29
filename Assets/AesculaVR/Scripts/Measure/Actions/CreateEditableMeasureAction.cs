using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateEditableMeasureAction : CreateMeasureAction
{
    private readonly EditorManager editorManager;

    public CreateEditableMeasureAction(MeasureManager.MeasureType type, Transform parent) : base (type, parent)
    {
        editorManager = EditorManager.GetManager();
        measure.SetManipulatablesEnabled(true);
    }

    public override void DoAction()
    {
        base.DoAction();
        editorManager.MeasureManager.AddMeasure(measure);
    }

    public override void UndoAction()
    {
        base.DoAction();
        editorManager.MeasureManager.RemoveMeasure(measure);
    }
}
