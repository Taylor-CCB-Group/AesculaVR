using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


/// <summary>
/// A view that represenets a single measurement.
/// </summary>
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

    /// <summary>
    /// Sets up the view.
    /// </summary>
    /// <param name="measure"> The measure to represent. </param>
    /// <param name="backgroundColor"> the background color to the view. </param>
    /// <param name="errorDialog"> The error dialog to use. </param>
    public virtual void SetUp(Measure measure, Color backgroundColor, ErrorDialog errorDialog)
    {
        this.measure = measure;

        this.backgroundColor.color = backgroundColor;

        this.type.SetText((measure is VectorMeasure) ? "Vector" : "Plane");

        if (measure is VectorMeasure)
            this.type.SetText("Vector");
        else if (measure is PlaneMeasure)
            this.type.SetText("Plane");
        else if (measure is PointMeasure)
            this.type.SetText("Point");
        else
            this.type.SetText(defaultString);

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

    /// <summary>
    /// Delete the Measure.
    /// </summary>
    private  void OnButtonPress() => editorManager.ActionManager.DoAction(new DeleteMeasureAction(measure));

    /// <summary>
    /// Update the vector measurement text.
    /// </summary>
    public void UpdateDistanceText() => direction.SetText(measure.Value.normalized.ToString());

    /// <summary>
    /// Run the OnButtonPress method, but catch any errors it might throw.
    /// </summary>
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
