using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

[Serializable]
public class BlackboardComparisonNode : Decorator
{
    [SerializeField] public PropertyNumeral comparison = new PropertyNumeral();
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
