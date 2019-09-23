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

    public static int EmptyValue => -1;

    /// <summary>
    /// gets invoked when the value is set. (even if its the same as before).
    /// </summary>
    public RadioToggleEvent onValueChanged = new RadioToggleEvent();
    public class RadioToggleEvent : UnityEvent<int> { }
    
   
    [SerializeField] protected List<Toggle> toggles = null;
    protected int activeIndex = EmptyValue;

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
    public virtual void SetActive(int value)
    {
        SetActiveWithoutNotify(value);

        onValueChanged.Invoke(activeIndex);
        NotifyObservers();
    }

    public void SetActiveWithoutNotify(int value)
    {
        activeIndex = value;

        for (int i = 0; i < toggles.Count; i++)
        {
            toggles[i].SetIsOnWithoutNotify((i == activeIndex));
            toggles[i].graphic.enabled = i == activeIndex;

        }
    }


    protected virtual void Awake()
    {

        if(toggles.Count > 0)
        {
            SetActive(0);
            for (int i = 0; i < toggles.Count; i++)
                toggles[i].onValueChanged.AddListener(valueChangeAction(i));
        }
        else
        {
            activeIndex = EmptyValue;
        }
    }

    public void AddToggle(Toggle toggle)
    {
        toggles.Add(toggle);

        if (activeIndex == EmptyValue)
        {
            activeIndex = 0;
        }

        int i = toggles.Count - 1;
        toggles[i].onValueChanged.AddListener(valueChangeAction(i));

        
    }


    public void RemoveToggle(Toggle toggle)
    {
        toggles.Remove(toggle);
        toggle.onValueChanged.RemoveAllListeners();


    }

    private UnityAction<bool> valueChangeAction(int i) => new UnityAction<bool>((bool v) => { SetActive(i); });
}
