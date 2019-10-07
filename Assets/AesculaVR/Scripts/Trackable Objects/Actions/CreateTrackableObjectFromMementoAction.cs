using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateTrackableObjectFromMementoAction : IAction
{

    private CompoundAction compoundAction;
    private List<Measure> measures;

    public List<Measure> Measures => new List<Measure>(measures);

    public CreateTrackableObjectFromMementoAction(TrackableObjectMemento memento, Transform parent, bool editable)
    {
        List<IAction> actions = new List<IAction>(memento.objects.Count + memento.measures.Count + 1);

        //objects
        for (int i = 0; i < memento.objects.Count; i++)
        {
            GenerateObjectAction action = GeneratedObject.GenerateObjectFomMemento(memento.objects[i], parent);
            action.GeneratedObject.GetComponent<BoxCollider>().enabled = editable;
            actions.Add(action);
        }

        //measures.
        this.measures = new List<Measure>(memento.measures.Count);
        for (int i = 0; i < memento.measures.Count; i++)
        {
            CreateMeasureAction createMeasureAction = (editable) ?
                new CreateEditableMeasureAction((MeasureManager.MeasureType)memento.measures[i].type, parent) :
                new CreateMeasureAction((MeasureManager.MeasureType)memento.measures[i].type, parent);

            if(memento.measures[i].type == (int)MeasureManager.MeasureType.TriangularPlane)
                createMeasureAction.Measure.RestoreMemento(memento.measures[i] as TriangularPlaneMeasure.Memento);
            else
                createMeasureAction.Measure.RestoreMemento(memento.measures[i]);

            measures.Add(createMeasureAction.Measure);
            actions.Add(createMeasureAction);
        }


        compoundAction = new CompoundAction(actions, "Loaded in a trackable Object");
    }

    public string Description() => compoundAction.Description();

    public void DoAction() => compoundAction.DoAction();


    public void UndoAction() => compoundAction.UndoAction();

}
