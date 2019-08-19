using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/// <summary>
/// We have a selection of toggles, but only one can be active at a time.
/// </summary>
public class RadioToggle : ObservableComponent
{

    /// <summary>
    /// gets invoked when the value is set. (even if its the same as before).
    /// </summary>
    public RadioToggleEvent onValueChanged = new RadioToggleEvent();
    public class RadioToggleEvent : UnityEvent<int> { }
    
   
    [SerializeField] private List<Toggle> toggles = null;
    private int activeIndex = 0;

    /// <summary>
    /// A list with each of the toggle elements.
    /// </summary>
    public List<Toggle> Toggles { get { return new List<Toggle>(toggles); } }

    /// <summary>
    /// which Index is currently active
    /// </summary>
    public int Value { get { return activeIndex; } }

    /// <summary>
    /// How many toggle elements are there?
    /// </summary>
    public int Count => toggles.Count;


    /// <summary>
    /// Set an index to be active.
    /// </summary>
    /// <param name="value">The index to set</param>
    public void SetActive(int value)
    {
        activeIndex = value;

        for (int i = 0; i < toggles.Count; i++)
            toggles[i].SetIsOnWithoutNotify((i == activeIndex));  

        onValueChanged.Invoke(activeIndex);
        NotifyObservers();
    }

    protected void Awake()
    {

        SetActive(0);

        for (int i = 0; i < toggles.Count; i++)
            toggles[i].onValueChanged.AddListener(valueChangeAction(i));
        

    }

    private UnityAction<bool> valueChangeAction(int i) => new UnityAction<bool>((bool v) => { SetActive(i); });
}
