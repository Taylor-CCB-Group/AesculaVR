using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An interface for objects that can reused multiple times, and are stored by IObjectPools
/// </summary>
public interface IPoolable
{
    void OnPoppedFromPool();
    void OnPushedToPool  ();
}
