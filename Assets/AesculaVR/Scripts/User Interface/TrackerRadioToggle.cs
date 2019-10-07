using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A Radio toggle for selecting a tracker.
/// </summary>
public class TrackerRadioToggle : RadioToggle, IObserver
{

    private EditorManager editorManager;
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
        public Toggle Toggle => toggle;

        public PoolableToggle(Toggle toggle)
        {
            this.toggle = toggle;
        }

        public void OnPoppedFromPool() => toggle.gameObject.SetActive(true);

        public void OnPushedToPool() => toggle.gameObject.SetActive(false);
    }
    #endregion


    public override void SetActive(int value)
    {
        if (editorManager)
            editorManager.ActionManager.DoAction(new SetTrackerAction(this, value, this.activeIndex));
        else if (mainManager)
            mainManager.ActionManager.DoAction(new SetTrackerAction(this, value, this.activeIndex));
        else
            throw new System.Exception();
    }

    public void SetActiveBase(int value) => base.SetActive(value);
  

    private TrackerManager GetTrackerManager()
    {
        if (mainManager)
            return mainManager.TrackerManager;
        else if (editorManager)
            return editorManager.TrackerManager;

        throw new System.Exception();
    }

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

    public void OnNumberOfTrackersChanged(int unused)
    {
        this.toggles = new List<Toggle>();
        int count = GetTrackerManager().TrackersReference.Count;

        ///remove all the active toggles
        while (activeToggles.Count > 0)
        {
            PoolableToggle toggle = activeToggles.Pop();
            this.RemoveToggle(toggle.Toggle);
            togglePool.Push(toggle);
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
        if (oldActive == EmptyValue)
        {

            if (activeToggles.Count == 0)
                newActiveValue = EmptyValue;
            else
                newActiveValue = 0;
        }
        else if (oldActive >= activeToggles.Count)
        {
            newActiveValue = activeToggles.Count - 1;
        }
        else
        {
            newActiveValue = oldActive;
        }

        SetActiveWithoutNotify(newActiveValue);
    }

    public void OnEnable()
    {
        Notify(null, null);
    }

    protected override void Awake()
    {
        base.Awake();

        this.togglePool = new ObjectPool();
        this.activeToggles = new Stack<PoolableToggle>();

        this.mainManager = MainManager.GetManager();
        this.editorManager = EditorManager.GetManager();

        this.onValueChanged.AddListener(OnNumberOfTrackersChanged);

        GetTrackerManager().AddObserver(this);
        
    }

    public void Notify(object Sender, EventArgs args)
    {        
        if (activeToggles.Count != GetTrackerManager().TrackersReference.Count)
            OnNumberOfTrackersChanged(0);

        SetActiveWithoutNotify(Value);
    }

    /// <summary>
    /// The selected tracker.
    /// </summary>
    public Tracker Tracker => GetTrackerManager().Trackers[Value];

}
