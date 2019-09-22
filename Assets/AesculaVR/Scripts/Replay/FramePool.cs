using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FramePool : ObjectPool
{
    public override void Fill(int amount = 1)
    {
        for (int i = 0; i < amount; i++)
            this.Push(new KeyFrame());
    }
}
