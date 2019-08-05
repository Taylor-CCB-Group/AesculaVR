using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A concreate class that implements the IObjectPool interface.
/// </summary>
public class ObjectPool : IObjectPool
{

    private Stack<IPoolable> objectPool;

    public int Count() => objectPool.Count;

    //some objects may have to be created outside the object pool and pushed in.
    public virtual void Fill(int amount = 1) => throw new System.InvalidOperationException("ObjectPool is Empty");

    public ObjectPool()
    {
        this.objectPool = new Stack<IPoolable>();
    }

    public IPoolable Pop()
    {
        if (objectPool.Count <= 0)
            Fill();

        IPoolable target = objectPool.Pop();
        target.OnPoppedFromPool();

        return target;
    }

    public void Push(IPoolable target)
    {
        target.OnPushedToPool();
        objectPool.Push(target);
    }
}
