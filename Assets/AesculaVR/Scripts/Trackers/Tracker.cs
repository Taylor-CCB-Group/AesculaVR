using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracker : MonoBehaviour
{



    private void Start()
    {
        MasterManager.GetManager().TrackerManager.Add(this);
    }

    private void OnEnable()
    {
        MasterManager.GetManager().TrackerManager.Add(this);
    }

    private void OnDisable()
    {
        MasterManager.GetManager().TrackerManager.Remove(this);
    }

    private void OnDestroy()
    {
        MasterManager.GetManager().TrackerManager.Remove(this);
    }
}
