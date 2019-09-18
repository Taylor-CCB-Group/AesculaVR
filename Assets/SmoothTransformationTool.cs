using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothMovementTool : ITool
{
    private struct TipTransform
    {
        public Vector3 position;
        public Quaternion rotation;
    }

    private int size;
    private int current;

    private TipTransform[] transforms;
    private EditorManager editorManager;

    private Transform tip => editorManager.ManipulatableManager.Right.Tip;
    private GameObject tempTip;

    private Transform orignalParent;
    private Manipulatable manipulatable;

    private Vector3 startPosition, startRotation;

    private Sprite icon;

    public SmoothMovementTool(int SmoothingAmount)
    {
        this.editorManager = EditorManager.GetManager();
        this.size = SmoothingAmount;
        this.transforms = new TipTransform[size];
        this.current = 0;

        this.icon = (Sprite)Resources.Load<Sprite>("Icons/SmoothMove");

    }

    private void Clear()
    {
        for(int i = 0;i < size; i++)
        {            
            transforms[i].position = tip.position;
            transforms[i].rotation = tip.rotation;   
        }
        current = 0;

        if (tempTip)
        {
            GameObject.Destroy(tempTip);
            tempTip = null;
        }

    }

    private void IncrementCurrent()
    {
        current = ((current + 1) % size);
    }

    private TipTransform CalculateAverage()
    {
        TipTransform average = new TipTransform();
        float x = 0, y = 0, z = 0, w = 0;

        for(int i = 0; i < size; i++)
        {
            average.position += transforms[i].position;
            x += transforms[i].rotation.x;
            y += transforms[i].rotation.y;
            z += transforms[i].rotation.z;
            w += transforms[i].rotation.w;
        }

        float k = 1.0f / Mathf.Sqrt(x * x + y * y + z * z + w * w);

        average.position /= size;
        average.rotation = new Quaternion(x*k, y*k, z*k, w*k);

        return average;
    }

    private void CreateTempTip()
    {
        tempTip = GameObject.Instantiate(new GameObject());
        tempTip.transform.position = tip.position;
        tempTip.transform.localScale = tip.lossyScale;
        tempTip.transform.rotation = tip.rotation;
    }

    #region ITool
    public void TriggerDown()
    {
        Clear();

        manipulatable = editorManager.ManipulatableManager.GetManipulatableUnderRightController();
        if (manipulatable)
        {
            CreateTempTip();

            startPosition = manipulatable.transform.position;
            startRotation = manipulatable.transform.rotation.eulerAngles;

            orignalParent = manipulatable.transform.parent;
            manipulatable.transform.SetParent(tempTip.transform);

          
        }

    }

    public void TriggerUpdate()
    {
        if (!manipulatable)
            return;

        transforms[current].position = tip.position;
        transforms[current].rotation = tip.rotation;
        IncrementCurrent();

        TipTransform average = CalculateAverage();
        tempTip.transform.position = average.position;
        tempTip.transform.rotation = average.rotation;
    }

    public void TriggerUp()
    {

        if (manipulatable)
        {
            manipulatable.transform.SetParent(orignalParent);

            IAction[] actions = new IAction[2];
            actions[0] = new MoveAction(manipulatable, startPosition, manipulatable.transform.position);
            actions[1] = new RotateAction(manipulatable, startRotation, manipulatable.transform.rotation.eulerAngles);
            editorManager.ActionManager.DoAction(new CompoundAction(actions),true);

            }

        Clear();
    }

    public void OnUpdate()
    {}

    public Sprite Icon() => icon;

    public void OnSelected()
    {
        Clear();
    }

    public void OnDeselected()
    {
        Clear();
    }
    #endregion
}
