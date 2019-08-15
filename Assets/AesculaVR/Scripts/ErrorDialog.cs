using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ErrorDialog : MonoBehaviour
{

    private const string errorString = "???";
    private const string defaultPositiveText = "Ok";


    [SerializeField] private TextMeshProUGUI message, positiveButtonText;
    [SerializeField] private Button positiveButton;

    /// <summary>
    /// Is the dialog visible?
    /// </summary>
    public bool IsShowing { get { return this.gameObject.activeSelf; } }

    public void Show(string message, string positiveText)
    {
        Debug.Assert(!IsShowing);

        this.positiveButtonText.SetText(positiveText);
        this.message.SetText(message);
        this.gameObject.SetActive(true);
    }

    public void Show(string message) => Show(message, defaultPositiveText);

    public void Hide()
    {
        this.gameObject.SetActive(false);
        this.message.SetText(errorString);
        this.positiveButtonText.SetText(errorString);
    }

    private void Awake()
    {
        positiveButton.onClick.AddListener(Hide);
        Hide();
    }
}
