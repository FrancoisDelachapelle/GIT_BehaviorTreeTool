using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugLeaf : Leaf
{
    protected override void StartNode()
    {
        
    }

    protected override ENodeReturnStatus ProcessNode()
    {
        Debug.Log("Leaf is running");
        return ENodeReturnStatus.SUCCES;
    }

    protected override void StopNode()
    {
        
    }
}
