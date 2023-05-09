using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class WaitNode : Leaf
{
    [SerializeField] protected float duration = 1;
    private float startTime = 0;


    protected override void StartNode()
    {
        startTime = Time.time;
    }

    protected override ENodeReturnStatus ProcessNode()
    {
        if (Time.time - startTime > duration)
            return ENodeReturnStatus.SUCCES;
        return ENodeReturnStatus.RUNNING;
    }

    protected override void StopNode()
    {
       
    }
}
