using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MeasureManagerView : LateObserver
{

#pragma warning disable 0649
    [SerializeField] private RadioToggle planeOrVectorToggle;
    [SerializeField] private MeasureView measureViewPrefab;
    [SerializeField] private Color color1, color2;
    [SerializeField] private ErrorDialog errorDialog;
    [SerializeField] private Transform contentRoot;
#pragma warning restore 0649


    /// <summary>
    /// an object pool  for the measure views.
    /// </summary>
    private class MeasureViewPool : ObjectPool
    {
        private MeasureView viewPrefab;

        public MeasureViewPool(MeasureView viewPrefab) : base()
        {
            this.viewPrefab = viewPrefab;
        }

        public override void Fill(int amount = 1)
        {
            MeasureView obj = GameObject.Instantiate(viewPrefab).GetComponent<MeasureView>();
            this.Push(obj);
        }
    }

    private enum PlaneOrVector { Plane, Vector }
    private enum CreateOrDestroy { Create, Destroy }

    private PlaneOrVector planeOrVector => planeOrVectorToggle.Value == 0 ? PlaneOrVector.Plane : PlaneOrVector.Vector;
    private EditorManager editorManager;

    private MeasureViewPool measureViewPool;
    private List<MeasureView> activeViews;

    private void Awake()
    {
        editorManager = EditorManager.GetManager();
        measureViewPool = new MeasureViewPool(measureViewPrefab);
        activeViews = new List<MeasureView>();
    }

    private void Start()
    {
        planeOrVectorToggle.onValueChanged.AddListener(SetTool);
        editorManager.MeasureManager.AddObserver(this);
    }

    /// <summary>
    /// Set the tool to the planeOrVectorToggle value.
    /// </summary>
    /// <param name="unused"></param>
    private void SetTool(int unused)
    {
        MeasureManager.MeasureType type;
        switch (planeOrVectorToggle.Value)
        {
            case 0:
                type = MeasureManager.MeasureType.Plane;
                break;
            case 1:
                type = MeasureManager.MeasureType.Vector;
                break;
            case 2:
                type = MeasureManager.MeasureType.Point;
                break;
            default:
                throw new System.NotImplementedException();
        }
        editorManager.MeasureManager.SetToolToCreateMeasure(type);
    }

    public override void LateNotify(object Sender, EventArgs args)
    {
        Debug.Log("PlaneVectorView =>  LateNotify");
        List<Measure> measures = editorManager.MeasureManager.Measures;

        //remove old views, add them to the pool.
        while (activeViews.Count > 0)
        {
            measureViewPool.Push(activeViews[activeViews.Count - 1]);
            activeViews.RemoveAt(activeViews.Count - 1);
        }
            
        //create a new view for each measure object.
        for(int i = 0; i < measures.Count; i++)
        {
            MeasureView view = (MeasureView)measureViewPool.Pop();
            
            view.transform.SetParent(contentRoot);
            view.transform.localPosition = Vector3.zero;
            view.transform.localScale = Vector3.one;

 
            view.SetUp(measures[i], ((i % 2 == 0) ? color1 : color2), errorDialog);
            activeViews.Add(view);
        }        
    }


    public override void LateUpdate()
    {
        base.LateUpdate();

        //update the distance text on each active view.
        for(int i = 0; i < activeViews.Count; i++)
        {
            activeViews[i].UpdateDistanceText();
        }
    }
}
