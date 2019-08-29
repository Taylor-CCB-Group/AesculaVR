using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An Action to create a new measure action
/// </summary>
public class CreateMeasureAction : IActionDereferenceable
{
    public string Description() => "Create a measurement";

    protected readonly Measure measure;

    public Measure Measure => measure;

    /// <summary>
    /// Construct the new create measure action.
    /// </summary>
    /// <param name="type">The type of measurement to create.</param>
    
    public CreateMeasureAction(MeasureManager.MeasureType type, Transform parent)
    {

        measure = type == MeasureManager.MeasureType.Plane ?
            (Measure)((GameObject)Resources.Load("MeasurePlane") ).GetComponent<PlaneMeasure >() :
            (Measure)((GameObject)Resources.Load("MeasureVector")).GetComponent<VectorMeasure>() ;

        measure = GameObject.Instantiate(measure.gameObject).GetComponent<Measure>();
        measure.SetColor(new Color(Random.value, Random.value, Random.value));

        measure.transform.rotation = parent.rotation;
        measure.transform.SetParent(parent);      
        measure.transform.position = Vector3.zero;

        measure.SetManipulatablesEnabled(false);
    }

    public virtual void DoAction() => measure.gameObject.SetActive(true);

    public virtual void UndoAction() => measure.gameObject.SetActive(false);

    public void OnDereferenced() => GameObject.Destroy(measure.gameObject);
}
