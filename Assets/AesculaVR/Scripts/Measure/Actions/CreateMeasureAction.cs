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

        Measure measure = null;
        switch (type)
        {
            case MeasureManager.MeasureType.Plane:
                measure = ((GameObject)Resources.Load("MeasurePlane")).GetComponent<PlaneMeasure>();
                break;
            case MeasureManager.MeasureType.Point:
                measure = ((GameObject)Resources.Load("MeasurePoint")).GetComponent<PointMeasure>();
                break;
            case MeasureManager.MeasureType.Vector:
                measure = ((GameObject)Resources.Load("MeasureVector")).GetComponent<VectorMeasure>();
                break;
            case MeasureManager.MeasureType.TriangularPlane:
                measure = ((GameObject)Resources.Load("MeasureTriangularPlane")).GetComponent<TriangularPlaneMeasure>();
                break;
            default:
                throw new System.NotSupportedException();
        }


        measure = GameObject.Instantiate(measure);

        measure.SetColor(new Color(Random.value, Random.value, Random.value));

        measure.transform.SetParent(parent);      
        measure.transform.position = Vector3.zero;
        measure.transform.rotation = Quaternion.identity;

        measure.SetManipulatablesEnabled(false);

        this.measure = measure;
    }

    public virtual void DoAction() => measure.gameObject.SetActive(true);

    public virtual void UndoAction() => measure.gameObject.SetActive(false);

    public void OnDereferenced() => GameObject.Destroy(measure.gameObject);
}
