using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceNode : Composite
{
    private int current = 0;
    protected override void StartNode()
    {
        current = 0;
    }

    protected override ENodeReturnStatus ProcessNode()
    {
        // if (Count < 1)
        // {
        //     Debug.Log("Error : No children nodes found");
        //     return ENodeReturnStatus.FAILURE;
        // }
        // for (int i = 0; i < Count; i++)
        // {
        //     TreeNode _child = allChildren[i];
        //     if (!_child)
        //     {
        //         Debug.Log("Error : null child encountered");
        //         return ENodeReturnStatus.FAILURE;
        //     }
        //     ENodeReturnStatus _status = _child.Execute();
        //     if (_status == ENodeReturnStatus.FAILURE)
        //         return _status;
        // }
        //
        // return ENodeReturnStatus.SUCCES;

        TreeNode _child = allChildren[current];
        switch (_child.Execute())
        {
            case ENodeReturnStatus.SUCCES:
                current++;
                break;
            case ENodeReturnStatus.FAILURE:
                return ENodeReturnStatus.FAILURE;
            case ENodeReturnStatus.RUNNING:
                return ENodeReturnStatus.RUNNING;
        }

        return current == allChildren.Count ? ENodeReturnStatus.SUCCES : ENodeReturnStatus.RUNNING;
    }

    protected override void StopNode()
    {

    }
}
