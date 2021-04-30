using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TrackersView :  LateObserver
{

    private class ViewPool : ObjectPool
    {
        private TrackerSelectView prefab;
        private Transform contentRoot;

        public ViewPool(TrackerSelectView prefab, Transform contentRoot)
        {
            this.prefab = prefab;
            this.contentRoot = contentRoot;
        }

        public override void Fill(int amount = 1)
        {
            for (int i = 0; i < amount; i++)
            {
                TrackerSelectView instance = GameObject.Instantiate(prefab.gameObject).GetComponent<TrackerSelectView>();
                instance.transform.SetParent(contentRoot);

                instance.transform.localPosition = Vector3.zero;
                instance.transform.localScale = Vector3.one;
                instance.transform.localRotation = Quaternion.identity;
                
                

                Push(instance);
            }
        }
    }

    private ViewPool viewPool;

    private TrackerManager trackerManager;

    [SerializeField] private TrackerSelectView trackerViewPrefab;
    [SerializeField] private Transform contentRoot;

    private List<TrackerSelectView> activeViews;
    private TrackerSelectView mainView;

    public Tracker Tracker => trackerManager.TrackersReference[trackerManager.GetActiveTrackerIndex];

    private void Awake()
    {

        if(trackerManager == null) trackerManager  = MainManager.GetManager()?.TrackerManager;
        if (trackerManager == null) trackerManager = EditorManager.GetManager()?.TrackerManager;


        viewPool = new ViewPool(trackerViewPrefab, contentRoot);
        activeViews = new List<TrackerSelectView>();
        Setup();

        trackerManager.AddObserver(this);
    }

    public void SetActive(int value)
    {
        //gets called but the tracker select view. 
        //(if we use the invoking method, this'll loop).

        mainView.SetIsOnNoInvoke(false);
        trackerManager.SetTrackerActive(value);

        mainView = activeViews[value];
        mainView.SetIsOnNoInvoke(true);

        trackerManager.SetTrackerActive(value);
    }

    public UnityAction<bool> SetActiveAction(int value) => new UnityAction<bool>((bool b) => { if(b) SetActive(value); });

    private void Setup()
    {
        //get rid of any existing views.
        while (activeViews.Count > 0)
        {
            TrackerSelectView view = activeViews[activeViews.Count - 1];
            view.OnValuedChanged.RemoveAllListeners();

            activeViews.RemoveAt(activeViews.Count - 1);
            viewPool.Push(view);
        }

        mainView = null;
        //make the new views.
        for (int i = 0; i < trackerManager.TrackersReference.Count; i++)
        {
            TrackerSelectView view = (TrackerSelectView)viewPool.Pop();
            view.OnValuedChanged.AddListener(SetActiveAction(i));

            

            if ((trackerManager.GetActiveTrackerIndex == i))
            {
                view.SetIsOnNoInvoke(true);
                mainView = view;
            }
            else
            {
                view.SetIsOnNoInvoke(false);
            }

            activeViews.Add(view);
        }       
                
    }

    public override void LateNotify(object Sender, EventArgs args)
    {
        Setup();
    }
}
