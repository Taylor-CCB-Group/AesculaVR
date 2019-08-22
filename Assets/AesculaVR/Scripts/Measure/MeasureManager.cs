using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MeasureManager : ObservableObject
{
    public enum MeasureType { Plane, Vector }

    public List<Measure> measures;
    private EditorManager editorManager;

    public MeasureManager()
    {
        this.editorManager = EditorManager.GetManager();
        this.measures = new List<Measure>();
    }

    public void AddMeasure(Measure measure)
    {
        measures.Add(measure);
        NotifyObservers();
    }

    public void RemoveMeasure(Measure measure)
    {
        measures.Remove(measure);
        NotifyObservers();
    }

    public IAction CreateMeasure(MeasureType type) => new CreateMeasureAction(type);

    public IAction DeleteMeasure(Measure measure) => new DeleteMeasureAction(measure);

    public void SetToolToCreateMeasure(MeasureType type) => editorManager.ToolManager.SetTool(new CreateMeasureTool(type));
}
