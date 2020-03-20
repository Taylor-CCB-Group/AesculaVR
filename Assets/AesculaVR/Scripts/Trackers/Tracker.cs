using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracker : MonoBehaviour
{

#pragma warning disable 0649
    [SerializeField] private GameObject activeObject; //The object that appears if this active.
#pragma warning restore 0649

    #region Unity
    private void Start()
    {
        EditorManager.GetManager()?.TrackerManager.Add(this);
        MainManager  .GetManager()?.TrackerManager.Add(this);
    }

    private void OnEnable()
    {
        EditorManager.GetManager()?.TrackerManager.Add(this);
        MainManager  .GetManager()?.TrackerManager.Add(this);
    }

    private void OnDisable()
    {
        EditorManager.GetManager()?.TrackerManager.Remove(this);
        MainManager  .GetManager()?.TrackerManager.Remove(this);
    }

    private void OnDestroy()
    {
        EditorManager.GetManager()?.TrackerManager.Remove(this);
        MainManager  .GetManager()?.TrackerManager.Remove(this);
    }
    #endregion

    public void SetActive(bool value) => activeObject.SetActive(true);
    public bool IsActive() => activeObject.activeSelf;



}
