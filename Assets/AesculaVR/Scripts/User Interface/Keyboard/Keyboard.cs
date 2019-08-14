using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class Keyboard : ObservableComponent
{
    private UnityAction<string> OnAcceptedAction;

    [SerializeField] private Key keyPrefab;
    [SerializeField] private List<Transform> rows;
    [SerializeField] private TMP_InputField textDisplay;
    [SerializeField] private Button acceptInput;
    [SerializeField] private Button dismiss;

    /// <summary>
    /// The current text the keyboard is showing.
    /// </summary>
    public string Text { get { return text; } }
    private string text;


    /// <summary>
    /// Add a character to the current text.
    /// </summary>
    /// <param name="character">the character to add.</param>
    public void AddCharacter(string character)
    {
        text += character;
        textDisplay.text = text;
        NotifyObservers();
    }

    /// <summary>
    /// Clear the keyboards text, so that it is empty.
    /// </summary>
    public void Clear()
    {
        text = "";
        textDisplay.text = text;
        NotifyObservers();
    }

    /// <summary>
    /// Is the keyboard visible?
    /// </summary>
    public bool IsShowing { get { return this.gameObject.activeSelf; } }

    /// <summary>
    /// Show the keyboard and set whats done when the user presses enter.
    /// </summary>
    /// <param name="OnAcceptedAction">The action to be done, when the user presses enter. </param>
    public void Show(UnityAction<string> OnAcceptedAction)
    {
        this.OnAcceptedAction = OnAcceptedAction;
        this.gameObject.SetActive(true);
    }

    /// <summary>
    /// User has accepted their input.
    /// </summary>
    private void AcceptInput()
    {
        OnAcceptedAction.Invoke(Text);
        Hide();
    }

    /// <summary>
    /// Hide the keyboard, and get it ready for the next use.
    /// </summary>
    public void Hide()
    {
        this.gameObject.SetActive(false);
        OnAcceptedAction = null;
        Clear();
    }


    #region Setup

    private void Awake()
    {
        Hide();

        acceptInput.onClick.AddListener(AcceptInput);
        dismiss.onClick.AddListener(Hide);

        CreateNumberKeys();
        CreateKeysQtoP();
        CreateKeysAtoL();
        CreateKeysZtoM();
    }


    /// <summary>
    /// Create a single key.
    /// </summary>
    /// <param name="character"> The key character. </param>
    /// <param name="parent"> the parent for the key. </param>
    /// <returns>the keythat was created. </returns>
    private Key CreateKey(string character, Transform parent)
    {
        Key key = GameObject.Instantiate(keyPrefab).GetComponent<Key>();
        key.transform.SetParent(parent);
        key.transform.localScale = Vector3.one;
        key.transform.localPosition = Vector3.zero;
        key.Setup(this, character);
        return key;
    }

    /// <summary>
    /// Create the number keys
    /// </summary>
    private void CreateNumberKeys()
    {
        for (int i = 0; i < 10; i++)
            CreateKey(i.ToString(), rows[0]);

    }

    /// <summary>
    /// create the row for Q to P
    /// </summary>
    private void CreateKeysQtoP()
    {
        CreateKey("Q", rows[1]);
        CreateKey("W", rows[1]);
        CreateKey("E", rows[1]);
        CreateKey("R", rows[1]);
        CreateKey("T", rows[1]);
        CreateKey("Y", rows[1]);
        CreateKey("U", rows[1]);
        CreateKey("I", rows[1]);
        CreateKey("O", rows[1]);
        CreateKey("P", rows[1]);
    }

    /// <summary>
    /// create the row for A to L
    /// </summary>
    private void CreateKeysAtoL()
    {
        CreateKey("A", rows[2]);
        CreateKey("S", rows[2]);
        CreateKey("D", rows[2]);
        CreateKey("F", rows[2]);
        CreateKey("G", rows[2]);
        CreateKey("H", rows[2]);
        CreateKey("J", rows[2]);
        CreateKey("K", rows[2]);
        CreateKey("L", rows[2]);
    }

    /// <summary>
    /// Create the row for Z to M
    /// </summary>
    private void CreateKeysZtoM()
    {
        CreateKey("Z", rows[3]);
        CreateKey("X", rows[3]);
        CreateKey("C", rows[3]);
        CreateKey("V", rows[3]);
        CreateKey("B", rows[3]);
        CreateKey("N", rows[3]);
        CreateKey("M", rows[3]);

    }

    #endregion
}
