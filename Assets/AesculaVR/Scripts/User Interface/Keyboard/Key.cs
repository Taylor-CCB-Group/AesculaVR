using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// A single key on a keyboard.
/// </summary>
[RequireComponent(typeof(Button))]
public class Key : MonoBehaviour
{
    private Button button;
    private TextMeshProUGUI text;
    private string character;
    private Keyboard keyboard;

    private void Awake()
    {
        this.button = GetComponent<Button>();
        this.text = GetComponentInChildren<TextMeshProUGUI>();
    }

    /// <summary>
    /// Setup the key.
    /// </summary
    /// <param name="keyboard"> The keyboard this key belongs to. </param>
    /// <param name="character"> The character this key represents. </param>
    public void Setup(Keyboard keyboard, string character)
    {
        this.keyboard = keyboard;
        this.character = character;
        this.text.SetText(character);
        this.button.onClick.AddListener(OnClick);
    }

    private void OnClick() => keyboard.AddCharacter(this.character);
}
