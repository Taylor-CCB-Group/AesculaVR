using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using System;

/// <summary>
/// A dialog box to ask a yes/no question. e.g. Are tou sure you want to delete this file?
/// <remark>
/// See : https://material.io/design/components/dialogs.html#anatomy
/// </remark>
/// </summary>
public class Dialog : ObservableComponent
{
    private const string errorString = "???";

    private const string defaultNegativeText = "Cancel";
    private const string defaultPositiveText = "Accept";

#pragma warning disable 0649
    [SerializeField]private TextMeshProUGUI title, message, negativeButtonText, positiveButtonText;
    [SerializeField] private Button positiveButton;
    [SerializeField] private Button negativeButton = null, dismissButton = null;
    [SerializeField] private ErrorDialog errorDialog;
#pragma warning restore 0649

    private UnityAction currentAction;
    private bool easyDismiss;

    private void Awake()
    {
        negativeButton.onClick.AddListener(Hide);
        positiveButton.onClick.AddListener(Hide);
        dismissButton.onClick.AddListener(Dismiss);

        if(IsShowing)
            this.HideNoAction();
    }

    /// <summary>
    /// Is the dialog visible?
    /// </summary>
    public bool IsShowing { get { return this.gameObject.activeSelf; } }

    /// <summary>
    /// Show the dialog to the user.
    /// </summary>
    /// <param name="action"> The action to take, if the user presses the positive button. </param>
    /// <param name="title"> The Title text</param>
    /// <param name="message"> The Message text</param>
    /// <param name="negativeText"> The text for the negative button </param>
    /// <param name="positiveText"> The text for the positive button </param>
    /// <param name="easyDismiss"> Can we dismiss this dialog by clicking off the dialog? </param>
    public void Show(UnityAction action, string title, string message, string negativeText, string positiveText, bool easyDismiss = true)
    {
        Debug.Assert(!IsShowing);

        //title
        this.title  .SetText(title);
        this.message.SetText(message);

        //button
        this.negativeButtonText.SetText(negativeText);
        this.positiveButtonText.SetText(positiveText);

        //button action
        UnityAction eAction = errorDialog ? ErrorAction(action) : action;
        this.positiveButton.onClick.AddListener(eAction);
        this.currentAction = eAction;

        this.easyDismiss = easyDismiss;

        this.gameObject.SetActive(true);
    }

    /// <summary>
    /// Show the dialog to the user with the default button text.
    /// </summary>
    /// <param name="action"> The action to take, if the user presses the positive button. </param>
    /// <param name="title"> The Title text</param>
    /// <param name="message"> The Message text</param>
    /// <param name="easyDismiss"> Can we dismiss this dialog by clicking off the dialog? </param>
    public void Show(UnityAction action, string title, string message, bool easyDismiss = true) => Show(action, title, message, defaultNegativeText, defaultPositiveText, easyDismiss);

   

    /// <summary>
    /// Hide the dialog from the user
    /// </summary>
    public void Hide()
    {
        Debug.Assert(IsShowing);


        this.positiveButton.onClick.RemoveListener(currentAction);
        this.currentAction = null;
        HideNoAction();
    }

    /// <summary>
    /// Hide the dialog from the user on the AWAKE CALL ONLY.
    /// </summary>
    private void HideNoAction()
    {
        this.gameObject.SetActive(false);

        this.title.SetText(errorString);
        this.message.SetText(errorString);

        this.negativeButtonText.SetText(errorString);
        this.positiveButtonText.SetText(errorString);
    }

    /// <summary>
    /// Hide the dialog from the user if we allow for the easy dismiss.
    /// </summary>
    private void Dismiss()
    {
        if (this.easyDismiss)
            Hide();
    }

    /// <summary>
    /// Wrap an action, so that if it throws an error; We can catch it.
    /// </summary>
    /// <param name="action">The action we want to wrap</param>
    /// <returns>The wrapped action</returns>
    private UnityAction ErrorAction(UnityAction action)
    {
        return new UnityAction(() => 
        {
            try
            {
                action.Invoke();
            }
            catch(Exception e)
            {
                this.Hide(); 
                errorDialog.Show(e.Message);
                Debug.Log(e.StackTrace);
            }
           
        });
    }


}
