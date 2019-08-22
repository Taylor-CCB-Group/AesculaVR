using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITool : ISelectable
{
    void TriggerDown();
    void TriggerUpdate(); //When Trigger is down
    void TriggerUp();
    void OnUpdate(); //per frame regardless of trigger

    Sprite Icon();
}
