using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine.UIElements;
using Cursor = UnityEngine.UIElements.Cursor;

[CreateAssetMenu(fileName = "BehaviorTree", menuName = "Behavior Tree")]
public class BehaviorTree : ScriptableObject
{

    [SerializeField] protected List<TreeNode> allNodes = new List<TreeNode>();
    [SerializeField] public BlackBoard blackboard = null; //TODO check to add encapsulation
    [SerializeField] protected bool canRun = false;
    [SerializeField] protected float startTime = .1f;
    [SerializeField] protected float repeatRate = .3f;
    [SerializeField] protected int treeSize = 0;
    [SerializeField] public Root root = null;

    public List<TreeNode> AllNodes => allNodes;
    public Root Root 
    {
        get => root;
        set
        {
            root = value;
        }
    }
    public int TreeSize => allNodes.Count;

    void Init()
    {
        treeSize = TreeSize;
    }

    private void OnEnable()
    {
        GiveBlackboardToNodes();
    }

    /// <summary>
    /// Allow the tree to run
    /// </summary>
    public void EnableTreeRunning()
    {
        canRun = true;
    }
    
    /// <summary>
    /// Disallow the tree to run or stop it if it's already running
    /// </summary>
    public void DisableTreeRunning()
    {
        canRun = false;
    }

    /// <summary>
    /// Custom update function for the tree
    /// </summary>
    public void TreeUpdate()
    {
        if (!canRun) return;
        if (!root)
        {
            Debug.LogError("Needs a root before running");
            return;
        }

        root.Execute();

    }

    /// <summary>
    /// Change the root node to the one in parameter
    /// </summary>
    /// <param name="_newRoot"></param>
    public void SetRoot(Root _newRoot)
    {
        root = _newRoot;
    }

    /// <summary>
    /// Create a node of the type in parameter and add it to the tree SO
    /// </summary>
    /// <param name="_type"></param>
    /// <returns></returns>
    public TreeNode CreateNode(System.Type _type)
    {
        if (Application.isPlaying) return null; // Disallows possibility to add nodes while in PIE
        TreeNode _node = ScriptableObject.CreateInstance(_type) as TreeNode; //Create an instance of the node as a SO
        _node.name = _type.Name;
        _node.guid = GUID.Generate().ToString(); //Gives a unique ID

        Undo.RecordObject(this, "Behaviour Tree (CreateNode");
        allNodes.Add(_node);
        
        AssetDatabase.AddObjectToAsset(_node, this); //Add this node SO to the tree SO
        Undo.RegisterCreatedObjectUndo(_node, "Behaviour Tree (CreateNode)");
        AssetDatabase.SaveAssets(); //Save the Assets so it's not lost
        return _node;
    }

    /// <summary>
    /// Delete this node from the list owned by the tree and removes it from the tree SO
    /// </summary>
    /// <param name="_node"></param>
    public void DeleteNode(TreeNode _node)
    {
        Undo.RecordObject(this, "Behaviour Tree (DeleteNode");
        allNodes.Remove(_node);
        //AssetDatabase.RemoveObjectFromAsset(_node); //Remove the node from the tree ScriptableObject
        Undo.DestroyObjectImmediate(_node); //Replace RemoveOBjectFromAsset by this and it works as well.
        AssetDatabase.SaveAssets();
    }

    /// <summary>
    /// Add child to a parent node's children list
    /// </summary>
    /// <param name="_parent"></param>
    /// <param name="_child"></param>
    public void AddChild(TreeNode _parent, TreeNode _child)
    {
        //TODO trouver un moyen de faire �a plus propre
        //Sans doute qu'un enum de type sur le tree node pourrait aider
        Root _root = _parent as Root;
        if (_root)
        {
            Undo.RecordObject(_root, "Behaviour Tree (AddChild)");
            _root.Child = _child ;
            EditorUtility.SetDirty(_root);
        }
        Decorator _decorator = _parent as Decorator;
        if (_decorator)
        {
            Undo.RecordObject(_decorator, "Behaviour Tree (AddChild)");
            _decorator.child = _child;
            EditorUtility.SetDirty(_decorator);
        }
        Composite _composite = _parent as Composite;
        if (_composite)
        {
            Undo.RecordObject(_composite, "Behaviour Tree (AddChild)");
            _composite.allChildren.Add(_child);
            EditorUtility.SetDirty(_composite);
        }

    }
    
    /// <summary>
    /// Remove child from its parent list of children
    /// </summary>
    /// <param name="_parent"></param>
    /// <param name="_child"></param>
    public void RemoveChild(TreeNode _parent, TreeNode _child)
    {
        Root _root = _parent as Root;
        if (_root)
        {
            Undo.RecordObject(_root, "Behaviour Tree (RemoveChild)");
            _root.Child = null ;
            EditorUtility.SetDirty(_root);
        }
        Decorator _decorator = _parent as Decorator;
        if (_decorator)
        {
            Undo.RecordObject(_decorator, "Behaviour Tree (RemoveChild)");   
            _decorator.child = null;
            EditorUtility.SetDirty(_decorator);
        }
        Composite _composite = _parent as Composite;
        if (_composite)
        {
            Undo.RecordObject(_composite, "Behaviour Tree (RemoveChild)");   
            _composite.allChildren.Remove(_child);
            EditorUtility.SetDirty(_composite);
        }
    }
    
    /// <summary>
    /// Get all the children of the given node
    /// </summary>
    /// <param name="_parent"></param>
    /// <returns></returns>
    public List<TreeNode> GetChildren(TreeNode _parent)
    {
        List<TreeNode> _children = new List<TreeNode>();
        Root _root = _parent as Root;
        if (_root && _root.Child != null)
        {
            _children.Add(_root.Child);
        }
        Decorator _decorator = _parent as Decorator;
        if (_decorator && _decorator.child != null)
        {
            _children.Add(_decorator.child);
        }
        Composite _composite = _parent as Composite;
        if (_composite)
        {
            return _composite.allChildren;
        }

        return _children;
    }

    public void TreeWideCallbackActivator(TreeNode _node, Action<TreeNode> _callback)
    {
        if (!_node) return;
        _callback?.Invoke(_node);
        List<TreeNode> _tempChildren = GetChildren(_node);
        _tempChildren.ForEach((n) => TreeWideCallbackActivator(n, _callback));
    }

    /// <summary>
    /// Clone the tree when running to avoid modifying the original asset.
    /// </summary>
    /// <returns></returns>
    public BehaviorTree Clone()
    {
        BehaviorTree _tree = Instantiate(this);
        _tree.blackboard = blackboard.Clone();
        _tree.Root = (Root)_tree.Root.Clone();
        _tree.allNodes = new List<TreeNode>();
        TreeWideCallbackActivator(_tree.root, (n) => _tree.allNodes.Add(n));
        return _tree;
    }

    public void GiveBlackboardToNodes() 
    {
        TreeWideCallbackActivator(root, node =>
        {
            node.blackboard = blackboard;
        });
    }
}
