using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TrackableObject : MonoBehaviour
{

    /// <summary>
    /// Create a Trackable Object from a TrackableObjectMemento.
    /// </summary>
    /// <param name="memento" > The memento to load. </param>
    /// <param name="parent"  > The parent object for the trackable object. </param>
    /// <param name="editable"> Will components of the trackable objects be editable? </param>
    /// <returns></returns>
    public static IAction CreateFromMemento(TrackableObjectMemento memento, Transform parent, bool editable)
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
        for (int i = 0; i < memento.measures.Count; i++)
        {
            CreateMeasureAction createMeasureAction = (editable) ?
                new CreateEditableMeasureAction ((MeasureManager.MeasureType)memento.measures[i].type, parent) :
                new CreateMeasureAction         ((MeasureManager.MeasureType)memento.measures[i].type, parent) ;
            createMeasureAction.Measure.RestoreMemento(memento.measures[i]);

            actions.Add(createMeasureAction);
        }


        return new CompoundAction(actions, "Loaded in a trackable Object");
    }

    
}
