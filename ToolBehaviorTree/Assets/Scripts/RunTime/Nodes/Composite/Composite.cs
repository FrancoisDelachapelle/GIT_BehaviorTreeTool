using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

[Serializable]
public enum ENodeReturnStatus
{
    SUCCES,
    FAILURE,
    RUNNING
}

public abstract class Composite : TreeNode
{
    //public event Action<ENodeReturnStatus> OnNodeReturn = null; 
    [SerializeField, HideInInspector] public List<TreeNode> allChildren = new List<TreeNode>();
    public int Count => allChildren.Count;
    
    /// <summary>
    /// Clone the  node and all its children. Avoid modifying the  original tree.
    /// </summary>
    /// <returns></returns>
    public override TreeNode Clone()
    {
        Composite _node = Instantiate(this);
        _node.allChildren = allChildren.ConvertAll(c => c.Clone()); // Pas sûr de ça.
        return _node;
    }
}
