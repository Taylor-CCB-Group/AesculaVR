using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// A drop down UI element for setting a sort by field
/// </summary>
[RequireComponent(typeof(TMP_Dropdown))]
public class SortByDropdown : ObservableComponent
{

    private TMP_Dropdown dropdown;

    private const string SelectedText = "Sort by : ";

    private List<TMP_Dropdown.OptionData> options;
    private List<string> optionsText = new List<string>(new string[]{ "Name", "Created", "Accessed", "Modified" });


    public FileSorter.SortBy Value()
    {
        return (FileSorter.SortBy)dropdown.value;
    }

    protected void Awake()
    {

        options = new List<TMP_Dropdown.OptionData>();
        options.Add(new TMP_Dropdown.OptionData(optionsText[0]));
        options.Add(new TMP_Dropdown.OptionData(optionsText[1]));
        options.Add(new TMP_Dropdown.OptionData(optionsText[2]));
        options.Add(new TMP_Dropdown.OptionData(optionsText[3]));

        this.dropdown = GetComponent<TMP_Dropdown>();
        this.dropdown.ClearOptions();
        this.dropdown.AddOptions(options);

        OnValueChanged(this.dropdown.value);

        this.dropdown.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnValueChanged(int value)
    {
        this.dropdown.captionText.SetText(SelectedText + optionsText[value]);
        NotifyObservers();
    }
}
