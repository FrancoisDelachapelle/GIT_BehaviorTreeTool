using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorNode : Composite
{
    private int current = 0;
    protected override void StartNode()
    {
        current = 0;
    }

    protected override ENodeReturnStatus ProcessNode()
    {
        TreeNode _child = allChildren[current];
        switch(_child.Execute())
        {
            case ENodeReturnStatus.SUCCES:
                return ENodeReturnStatus.SUCCES;
            case ENodeReturnStatus.FAILURE:
                current++;
                break;
            case ENodeReturnStatus.RUNNING:
                return ENodeReturnStatus.RUNNING;
        }
        return current >= allChildren.Count ? ENodeReturnStatus.SUCCES : ENodeReturnStatus.RUNNING;
    }

    protected override void StopNode()
    {
        
    }
}
