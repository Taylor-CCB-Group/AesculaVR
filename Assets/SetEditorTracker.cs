using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TrackerRadioToggle))]
public class SetEditorTracker : MonoBehaviour
{
    private EditorManager editorManager;

    private void Start()
    {
        editorManager = EditorManager.GetManager();
        TrackerRadioToggle radio = GetComponent<TrackerRadioToggle>();
        radio.onValueChanged.AddListener(SetValue);
        editorManager.TrackableObjectManager.AddObserver(radio);
    }

    public void SetValue(int value)
    {
        editorManager.TrackerManager.SetMainIndex(value);
    }

}
