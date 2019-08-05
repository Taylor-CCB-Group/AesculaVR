using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An interface to store IPoolable objects.
/// </summary>
public interface IObjectPool
{
    void Push(IPoolable target);
    IPoolable Pop ();
    void Fill(int amount = 1);

    int  Count();
    
}
