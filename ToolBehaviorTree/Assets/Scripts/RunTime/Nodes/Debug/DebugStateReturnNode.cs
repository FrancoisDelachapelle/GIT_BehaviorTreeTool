using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class DebugStateReturnNode : Leaf
{
    [SerializeField] protected ENodeReturnStatus debugStatusValue = ENodeReturnStatus.SUCCES;
    protected override void StartNode()
    {
       
    }

    protected override ENodeReturnStatus ProcessNode()
    {
        Debug.Log(debugStatusValue);
        return debugStatusValue;
    }

    protected override void StopNode()
    {
     
    }
}
