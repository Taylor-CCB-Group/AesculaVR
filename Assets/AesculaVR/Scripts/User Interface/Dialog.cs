using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

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


    [SerializeField]private TextMeshProUGUI title, message, negativeButtonText, positiveButtonText;
    [SerializeField] private Button positiveButton;
    [SerializeField] private Button negativeButton, dismissButton;

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
        this.positiveButton.onClick.AddListener(action);
        this.currentAction = action;

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

}
