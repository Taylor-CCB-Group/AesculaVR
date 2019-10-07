using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MeasureManager : ObservableObject
{
    public enum MeasureType { Plane = 0, Vector = 1, Point = 2, TriangularPlane = 3 }

    private List<Measure> measures;
    private EditorManager editorManager;

    public List<Measure> Measures => new List<Measure>(measures);
    public List<Measure> MeasuresReference => (measures);

    public MeasureManager()
    {
        this.editorManager = EditorManager.GetManager();
        this.measures = new List<Measure>();
    }


    /// <summary>
    /// Adds a measure to the list for tracking. To create a measurement use "CreateMeasure".
    /// </summary>
    /// <param name="measure">The measure to add.</param>
    public void AddMeasure(Measure measure)
    {
        measures.Add(measure);
        NotifyObservers();
    }

    /// <summary>
    /// Remove a measure from tracking. To Delete a measure use "DeleteMeasure"
    /// </summary>
    /// <param name="measure">The measure to remove.</param>
    public void RemoveMeasure(Measure measure)
    {
        measures.Remove(measure);
        NotifyObservers();
    }

    /// <summary>
    /// delete all the measurements in the scene.
    /// </summary>
    public void Clear() => editorManager.ActionManager.DoAction(ClearMeasures());

    /// <summary>
    /// Returns an action that deletes all the measurements in the scene.
    /// </summary>
    /// <returns>The action (not yet done) that deletes all the measurements. </returns>
    public IAction ClearMeasures()
    {
        List<IAction> actions = new List<IAction>(measures.Count);

        for(int i = measures.Count - 1; i >= 0; i--)
        {
            actions.Add(DeleteMeasure(measures[i]));
        }

        return new CompoundAction(actions, "Cleared all the measurements.");
    }


    /// <summary>
    /// Returns an action that creates a new meaurement.
    /// </summary>
    /// <param name="type"> The type of measurement tp create </param>
    /// <returns>The action that creates the measurement.</returns>
    public IAction CreateMeasure(MeasureType type) => new CreateEditableMeasureAction(type, editorManager.TrackerManager.Main.transform);

    /// <summary>
    /// Returns an action that deletes a measurement.
    /// </summary>
    /// <param name="measure">The measure to delete</param>
    /// <returns>The action that deletes the measure.</returns>
    public IAction DeleteMeasure(Measure measure) => new DeleteMeasureAction(measure);

    public void SetToolToCreateMeasure(MeasureType type) => editorManager.ToolManager.SetTool(new CreateMeasureTool(type));
}

