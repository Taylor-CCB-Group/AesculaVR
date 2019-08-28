using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An Action to create a new measure action
/// </summary>
public class CreateMeasureAction : IActionDereferenceable
{
    public string Description() => "Create a measurement";

    private readonly Measure measure;
    private readonly EditorManager editorManager;
    public Measure Measure => measure;

    /// <summary>
    /// Construct the new create measure action.
    /// </summary>
    /// <param name="type">The type of measurement to create.</param>
    public CreateMeasureAction(MeasureManager.MeasureType type)
    {
        editorManager = EditorManager.GetManager();

        measure = type == MeasureManager.MeasureType.Plane ?
            (Measure)((GameObject)Resources.Load("MeasurePlane") ).GetComponent<PlaneMeasure >() :
            (Measure)((GameObject)Resources.Load("MeasureVector")).GetComponent<VectorMeasure>() ;

        measure = GameObject.Instantiate(measure.gameObject).GetComponent<Measure>();
        measure.SetColor(new Color(Random.value, Random.value, Random.value));

        measure.transform.SetParent(editorManager.TrackerManager.Main.transform);
    }

    public void DoAction()
    {
        editorManager.MeasureManager.AddMeasure(measure);
        measure.gameObject.SetActive(true);
    }

    public void UndoAction()
    {
        measure.gameObject.SetActive(false);
        editorManager.MeasureManager.RemoveMeasure(measure);
    }

    public void OnDereferenced() => GameObject.Destroy(measure.gameObject);
}
