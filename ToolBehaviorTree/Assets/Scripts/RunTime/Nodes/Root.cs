using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

[Serializable]
public class Root : TreeNode
{

    [SerializeField] private TreeNode child = null;
    [SerializeField, HideInInspector] private static Root instance = null;
    
    public Root Instance => instance;

    public TreeNode Child { get => child; set { child = value; } }


    void Init()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }
        instance = ScriptableObject.CreateInstance<Root>();
        return;
    }
    
    protected override void StartNode()
    {
    }

    protected override ENodeReturnStatus ProcessNode()
    {
        if (!child) return ENodeReturnStatus.FAILURE;
        return child.Execute();
    }

    protected override void StopNode()
    {
    }

    public override TreeNode Clone()
    {
        Root _node = Instantiate(this);
        if (!_node.child) return _node;
        _node.child = child.Clone();
        return _node;
    }
}
