using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class Leaf : TreeNode
{
    protected override void StartNode()
    {
        throw new NotImplementedException();
    }

    protected override ENodeReturnStatus ProcessNode()
    {
        throw new NotImplementedException();
    }

    protected override void StopNode()
    {
        throw new NotImplementedException();
    }
}


