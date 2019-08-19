using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracker : MonoBehaviour
{
    private void Start()
    {
        EditorManager.GetManager().TrackerManager.Add(this);
    }

    private void OnEnable()
    {
        EditorManager.GetManager().TrackerManager.Add(this);
    }

    private void OnDisable()
    {
        EditorManager.GetManager().TrackerManager.Remove(this);
    }

    private void OnDestroy()
    {
        EditorManager.GetManager().TrackerManager.Remove(this);
    }
}
