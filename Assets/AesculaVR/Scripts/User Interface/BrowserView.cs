using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrowserView : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private RadioToggle sidebar;
    [SerializeField] private List<GameObject> mainViews;
#pragma warning restore 0649

    private void Start()
    {
        sidebar.onValueChanged.AddListener(ShowView);
        ShowView(sidebar.Value);
    }

    private void ShowView(int value)
    {
        for(int i = 0; i < mainViews.Count; i++)
            mainViews[i].SetActive(i == value);
        
    }
}
