using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DebugLogNode : Leaf
{
     public string message = "";

    protected override void StartNode()
    {
        Debug.Log($"Start : {message}");
    }

    protected override ENodeReturnStatus ProcessNode()
    {
        Debug.Log($"Process : {message}");
        Debug.Log($"BB moveToPos : {blackboard.moveToPosition}");
        blackboard.moveToPosition.x += 1;
        return ENodeReturnStatus.SUCCES;
    }

    protected override void StopNode()
    {
        Debug.Log($"Stop : {message}");
    }
}
