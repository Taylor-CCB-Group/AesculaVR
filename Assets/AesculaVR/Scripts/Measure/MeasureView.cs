using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class MeasureView : MonoBehaviour, IPoolable
{

    private const string defaultString = "???";
    private Color defaultColor => new Color(1, 1, 1);


#pragma warning disable 0649
    [SerializeField] private TextMeshProUGUI type, direction;
    [SerializeField] private Image color, backgroundColor;
    [SerializeField] private Button deleteButton;
#pragma warning restore 0649

    private EditorManager editorManager;
    private ErrorDialog errorDialog;
    private Measure measure;

    void Awake()
    {
        editorManager = EditorManager.GetManager();
        this.deleteButton.onClick.AddListener(ErrorCheck);
    }

    public virtual void SetUp(Measure measure, Color backgroundColor, ErrorDialog errorDialog)
    {
        this.measure = measure;

        this.backgroundColor.color = backgroundColor;

        this.type.SetText((measure is VectorMeasure) ? "Vector" : "Plane");
        this.color.color = measure.Color;
        this.direction.SetText(measure.Value.normalized.ToString());
        
        this.errorDialog = errorDialog;
    }
    
    #region IPoolable
    public void OnPoppedFromPool()
    {
        this.gameObject.hideFlags = HideFlags.None;
        this.gameObject.SetActive(true);
    }

    public void OnPushedToPool()
    {
        this.backgroundColor.color = defaultColor;
        this.color.color           = defaultColor;

        this.type.SetText     (defaultString);
        this.direction.SetText(defaultString);


        this.gameObject.SetActive(false);
        this.gameObject.hideFlags = HideFlags.HideInHierarchy;
    }
    #endregion

    private  void OnButtonPress() => editorManager.ActionManager.DoAction(new DeleteMeasureAction(measure));

    public void UpdateDistanceText() => direction.SetText(measure.Value.normalized.ToString());

    private void ErrorCheck()
    {
        if (!errorDialog)
        {
            //if we dont have a dialog, dont check.
            OnButtonPress();
            return;
        }


        try
        {
            OnButtonPress();
        }
        catch (Exception e)
        {
            Debug.Log(e.TargetSite);
            errorDialog.Show(e.Message);
        }
    }
}
