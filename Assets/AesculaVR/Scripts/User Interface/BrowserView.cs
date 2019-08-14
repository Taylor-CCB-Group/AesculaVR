using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrowserView : MonoBehaviour
{
    [SerializeField] private RadioToggle sidebar;
    [SerializeField] private List<GameObject> mainViews;

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
