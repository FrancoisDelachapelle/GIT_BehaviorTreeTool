using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class Decorator : TreeNode
{
    [HideInInspector]public TreeNode child = null;

    protected override void StartNode()
    {
    }

    protected override ENodeReturnStatus ProcessNode()
    {
        throw new NotImplementedException();
    }

    protected override void StopNode()
    {
        throw new NotImplementedException();
    }
    
    public override TreeNode Clone()
    {
        Decorator _node = Instantiate(this);
        _node.child = child.Clone();
        return _node;
    }
}
