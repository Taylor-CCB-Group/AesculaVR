using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An Action to delete an measure.
/// </summary>
public class DeleteMeasureAction : IAction
{
    public string Description() => "Delete a measurement";

    private readonly Measure measure;
    private readonly EditorManager editorManager;

    /// <summary>
    /// construct the new delete measure action.
    /// </summary>
    /// <param name="measure"> The measure to delete. </param>
    public DeleteMeasureAction(Measure measure)
    {
        this.measure = measure;
        this.editorManager = EditorManager.GetManager();
    }

    public void DoAction()
    {
        measure.gameObject.hideFlags = HideFlags.HideInHierarchy;
        this.editorManager.MeasureManager.RemoveMeasure(measure);
        measure.gameObject.SetActive(false);
    }

    public void UndoAction()
    {
        measure.gameObject.SetActive(true);
        this.editorManager.MeasureManager.AddMeasure(measure);
        measure.gameObject.hideFlags = HideFlags.None;
    }
}
