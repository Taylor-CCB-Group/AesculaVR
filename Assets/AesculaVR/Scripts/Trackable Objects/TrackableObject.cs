using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TrackableObjectManager;

public class TrackableObject : MonoBehaviour
{

    MasterManager masterManager;

    public void Load(IMemento memento)
    {
        GenerateObjects(memento);
        SetTracker(0);
    }

    public void SetTracker(int i)
    {
       this.transform.SetParent(masterManager.TrackerManager.Trackers[i].transform);
    }

    /// <summary>
    /// generate the objects from the memento
    /// </summary>
    /// <param name="memento"></param>
    private void GenerateObjects(IMemento memento)
    {
        TrackableObjectsMemento trackableObjectsMemento = (TrackableObjectsMemento)memento;
        List<IAction> actions = new List<IAction>(trackableObjectsMemento.objects.Count + 1);

        for (int i = 0; i < trackableObjectsMemento.objects.Count; i++)
        {
            ObjectManager.GenerateObjectAction action = GenerateObjecFomMemento(trackableObjectsMemento.objects[i], masterManager.TrackerManager.Main?.transform);
            action.GeneratedObject.GetComponent<BoxCollider>().enabled = false;
            actions.Add(action);
        }

        IAction compound = new CompoundAction(actions, "Loaded in a trackable Object");
        masterManager.ActionManager.DoAction(compound);
    }
}
