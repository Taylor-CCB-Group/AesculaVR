using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObserver
{
    void Notify(object Sender, EventArgs args);
}
