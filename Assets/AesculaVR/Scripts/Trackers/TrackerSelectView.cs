using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TrackerSelectView : MonoBehaviour, IPoolable
{
    private bool isOn = false;

    private class UnityBoolEvent : UnityEvent<bool> { };

    private UnityEvent<bool> onValuedChanged;
    public UnityEvent<bool> OnValuedChanged
    {
        get
        {
            if (onValuedChanged == null)
                onValuedChanged = new UnityBoolEvent();
            return onValuedChanged;
        }
    }

    [SerializeField] private Image onRingImage;
    [SerializeField] private Button toggleValue;

    public bool IsOn
    {
        get { return isOn; }
        set
        {
            SetIsOnNoInvoke(value);
            OnValuedChanged.Invoke(value);
            
        }
    }

    public void SetIsOnNoInvoke(bool value)
    {
        this.isOn = value;
        toggleValue.interactable = !value;
        UpdateView();
    }

    private void UpdateView() => onRingImage.enabled = isOn;

    public void OnPoppedFromPool()
    {

        this.gameObject.SetActive(true);
        this.gameObject.hideFlags = HideFlags.HideInInspector;
    }

    public void OnPushedToPool()
    {
        this.gameObject.SetActive(false);
        this.gameObject.hideFlags = HideFlags.HideInInspector;

    }
}
