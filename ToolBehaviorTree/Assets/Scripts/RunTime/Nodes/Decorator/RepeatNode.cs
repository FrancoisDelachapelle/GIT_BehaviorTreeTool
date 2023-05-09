using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class RepeatNode : Decorator
{
    protected override void StartNode()
    {
        base.StartNode();
    }

    protected override ENodeReturnStatus ProcessNode()
    {
        return base.ProcessNode();
    }

    protected override void StopNode()
    {
        base.StopNode();
    }
}
