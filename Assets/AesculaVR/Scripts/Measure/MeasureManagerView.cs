using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeasureManagerView : LateObserver
{

    /* Vector mode.
     *
     *--Create mode
     * + Create two manlipulatable spheres that the user can move around
     * + Click down ->create first, Click Up -> Create seconed.
     * + Vectors are stored (runtime) as two transforms (one for each sphere).
     * 
     * 
     * --Destroy mode
     * + Delete vector if either the spheres or the link between them is grabbed.
     * +
     */

    /* Plane mode.
     * 
     * -Create mode
     * + Create two manlipulatable spheres that the user can move around, these for the normal to the plane.
     * + The plane can be represented by a quad.
     * + works the same way as a vector
     * + 
     */

#pragma warning disable 0649
    [SerializeField] private RadioToggle planeOrVectorToggle, createOrDestroyToggle;
    [SerializeField] private MeasureView measureViewPrefab;
    [SerializeField] private Color color1, color2;
    [SerializeField] private ErrorDialog errorDialog;
    [SerializeField] private Transform contentRoot;
#pragma warning restore 0649

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
    private CreateOrDestroy createOrDestroy => createOrDestroyToggle.Value == 0 ? CreateOrDestroy.Create : CreateOrDestroy.Destroy;

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
        createOrDestroyToggle.onValueChanged.AddListener(SetTool);

        editorManager.MeasureManager.AddObserver(this);
    }

    private void SetTool(int unused)
    {
        if (createOrDestroyToggle.Value == 0)
        {
            //create
            MeasureManager.MeasureType type = planeOrVectorToggle.Value == 0 ? MeasureManager.MeasureType.Plane : MeasureManager.MeasureType.Vector;
            editorManager.MeasureManager.SetToolToCreateMeasure(type);
        }
        else
        {
            //destroy
        }
    }

    public override void LateNotify(object Sender, EventArgs args)
    {
        Debug.Log("PlaneVectorView =>  LateNotify");
        List<Measure> measures = editorManager.MeasureManager.measures;

        while (activeViews.Count > 0)
        {
            measureViewPool.Push(activeViews[activeViews.Count - 1]);
            activeViews.RemoveAt(activeViews.Count - 1);
        }
            

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

        for(int i = 0; i < activeViews.Count; i++)
        {
            activeViews[i].UpdateDistanceText();
        }
    }
}
