using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A radio view just to select a tracker
/// </summary>
public class TrackerRadioToggleView : RadioToggle, IObserver
{
    private MainManager mainManager;

#pragma warning disable 0649
    [SerializeField] private Toggle prefab;
    [SerializeField] private Transform contentRoot;
#pragma warning restore 0649

    #region Pooling
    private ObjectPool togglePool;
    private Stack<PoolableToggle> activeToggles;

    private class PoolableToggle : IPoolable
    {
        private Transform contentRoot;
        private Toggle toggle;
        public  Toggle Toggle => toggle;

        public PoolableToggle(Toggle toggle)
        {
            this.toggle = toggle;
        }

        public void OnPoppedFromPool() => toggle.gameObject.SetActive(true);

        public void OnPushedToPool() => toggle.gameObject.SetActive(false);        
    }
    #endregion

    /// <summary>
    /// Create a new toggle.
    /// </summary>
    /// <returns>The created toggle</returns>
    private PoolableToggle GetToggle()
    {
        if (togglePool.Count() > 0)
            return (PoolableToggle)togglePool.Pop();
        else
        {
            Toggle toggle = GameObject.Instantiate(prefab).GetComponent<Toggle>();
            toggle.transform.SetParent(contentRoot);

            toggle.transform.localPosition = Vector3.zero;
            toggle.transform.localScale = Vector3.one;

            return new PoolableToggle(toggle);
        }
    }

    protected override void Awake()
    {
        base.Awake();
        mainManager = MainManager.GetManager();

        togglePool = new ObjectPool();
        activeToggles = new Stack<PoolableToggle>();
    }

    void Start()
    {
        mainManager.TrackerManager.AddObserver(this);
    }

    public void Notify(object Sender, EventArgs args)
    {
        this.toggles = new List<Toggle>();
        int count = mainManager.TrackerManager.TrackersReference.Count;

        ///remove all the active toggles
        while(activeToggles.Count > 0)
        {
            togglePool.Push(activeToggles.Pop());
        }

        ///create the toggles for each tracker.
        int oldActive = this.activeIndex;
        for (int i = 0; i < count; i++)
        {
            PoolableToggle toggle = GetToggle();
            this.AddToggle(toggle.Toggle);
            activeToggles.Push(toggle);
        }

        ///Set the new active value.
        int newActiveValue = EmptyValue;
        if(oldActive == EmptyValue)
        {
            
            if (activeToggles.Count == 0)
                newActiveValue = EmptyValue;
            else
                newActiveValue = 0;
        }
        else if(oldActive >= activeToggles.Count)
        {
            newActiveValue = activeToggles.Count - 1;
        }
        else
        {
            newActiveValue = oldActive;
        }

        SetActive(newActiveValue);
    }

    public Tracker Tracker => mainManager.TrackerManager.Trackers[Value];
}
