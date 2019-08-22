using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMeasureAction : IActionDereferenceable
{
    public string Description() => "Create a measurement";
    private Measure measure;
    private EditorManager editorManager;

    public Measure Measure => measure;

    public CreateMeasureAction(MeasureManager.MeasureType type)
    {
        editorManager = EditorManager.GetManager();

        measure = type == MeasureManager.MeasureType.Plane ?
            (Measure)((GameObject)Resources.Load("MeasurePlane") ).GetComponent<PlaneMeasure >() :
            (Measure)((GameObject)Resources.Load("MeasureVector")).GetComponent<VectorMeasure>() ;

        measure = GameObject.Instantiate(measure.gameObject).GetComponent<Measure>();
        measure.SetColor(new Color(Random.value, Random.value, Random.value));
    }

    public void DoAction()
    {
        editorManager.MeasureManager.AddMeasure(measure);
        measure.gameObject.SetActive(true);
    }

    public void UndoAction()
    {
        measure.gameObject.SetActive(false);
        editorManager.MeasureManager.AddMeasure(measure);
    }

    public void OnDereferenced() => GameObject.Destroy(measure.gameObject);
}
