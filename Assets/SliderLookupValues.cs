using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class SliderLookupValues : MonoBehaviour
{
    [SerializeField] private List<float> values;
    [SerializeField] private Slider slider;

    [SerializeField] private GameObject knob;
    [SerializeField] private GameObject popup;
    [SerializeField] private TextMeshProUGUI popupText;


    public UnityEvent OnReleased;

    public Slider Slider => slider;

    public float Value => values[(int)slider.value];

    private void Awake()
    {
        slider.wholeNumbers = true;
        slider.minValue = 0;
        slider.maxValue = values.Count - 1;
        slider.value = slider.maxValue;
        slider.onValueChanged.AddListener((float value) => popupText.SetText(Value.ToString()));
    }

    public void ShowPopup()
    {
        popup.SetActive(true);
    }

    public void HidePopup()
    {
        popup.SetActive(false);
        //
    }

    public void OnReleasedInvoke() => OnReleased.Invoke();

}
