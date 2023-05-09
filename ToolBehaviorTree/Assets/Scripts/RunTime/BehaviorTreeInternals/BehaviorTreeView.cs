using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


enum EContextualMenuOriginTypes
{
    Leaf,
    Decorator,
    Composite
}
public class BehaviorTreeView : GraphView
{
    public Action<NodeView> OnNodeSelected;
    public new class UxmlFactory : UxmlFactory<BehaviorTreeView, GraphView.UxmlTraits> { }
    private BehaviorTree tree = null;
    private Vector2 mousePosition = Vector2.zero;
    public BehaviorTreeView()
    {
        Insert(0, new GridBackground());
        
        this.AddManipulator(new ContentZoomer());//Add zoom ability
        this.AddManipulator(new ContentDragger());//add click and drag for visual elements
        this.AddManipulator(new SelectionDragger());// idem but for a selection
        this.AddManipulator(new RectangleSelector()); //allow rectangle selection

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/RunTime/BehaviorTreeInternals/BehaviorTreeEditorWindow.uss");
        styleSheets.Add(styleSheet);

        Undo.undoRedoPerformed += OnUndoRedo;
    }

    private void OnUndoRedo()
    {
        PopulateView(tree);
        AssetDatabase.SaveAssets();
    }

    NodeView FindNodeView(TreeNode _node)
    {
        return GetNodeByGuid(_node.guid) as NodeView; //find a nodeView (visual element) based on guid value
    }

    /// <summary>
    /// Create the visual render of the tree
    /// </summary>
    /// <param name="_tree"> tree to create visual content for</param>
    internal void PopulateView(BehaviorTree _tree)
    {
        tree = _tree;
        graphViewChanged -= OnGraphViewChanged; //Unbind graphViewChange
        DeleteElements(graphElements); //Clean all visual elements before recreation
        graphViewChanged += OnGraphViewChanged; //Bind graphViewChange

        if (!tree.Root) //If no root node, create one
        {
            Root _root = (Root)tree.CreateNode(typeof(Root));
            tree.Root = _root;
            EditorUtility.SetDirty(tree); //Register change to tree withtout creating an undo entry
            AssetDatabase.SaveAssets(); //Save all changes to BT scriptable Object asset
        }
        //Create node view
        tree.AllNodes.ForEach(n => CreateNodeView(n));
        //Create edges
        tree.AllNodes.ForEach(n =>
        {
            var _children = tree.GetChildren(n);// For each child of a node
            _children.ForEach(c =>
            {
                NodeView _parentView = FindNodeView(n); // Get the visual element of the parent
                NodeView _childView = FindNodeView(c); // idem for the child
                Edge _edge = _parentView.output.ConnectTo(_childView.input); //Create the edge from the output-input connection
                AddElement(_edge); //Create the edge visual element
            });
        });
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        return ports.ToList().Where(endPort => 
            endPort.direction != startPort.direction && endPort.node != startPort.node).ToList();

        
        // return ports.ToList().Where(endPort => //Get all ports and distinguish each port type and ensure no same type can connect to one another
        //     endPort.direction != startPort.direction &&
        //     endPort.node != startPort.node).ToList();
    }

    /// <summary>
    /// Update the visual view of the BT window for deleted elements
    /// </summary>
    /// <param name="_graphviewchange">struct containing all changes (removed/deleted/created) of the visual elements</param>
    /// <returns></returns>
    private GraphViewChange OnGraphViewChanged(GraphViewChange _graphviewchange)
    {
        {

        /*
        //Save working
        // Debug.Log("GraphViewChange enter");
        // if (_graphviewchange.elementsToRemove != null)
        // {
        //     Debug.Log("GraphViewChange no null");
        //     _graphviewchange.elementsToRemove.ForEach(elem =>
        //     {
        //         NodeView _nodeView = elem as NodeView;
        //         if(_nodeView != null)
        //             tree.DeleteNode(_nodeView.node);
        //         
        //         Edge _edge = elem as Edge;
        //         if (_edge != null)
        //         {
        //             NodeView _parentView = _edge.output.node as NodeView;
        //             NodeView _childView = _edge.input.node as NodeView;
        //             tree.RemoveChild(_parentView.node, _childView.node);
        //         }
        //     });
        // }
        // if (_graphviewchange.edgesToCreate != null) // Create edges
        // {
        //     _graphviewchange.edgesToCreate.ForEach(edge =>
        //     {
        //         Debug.Log("Test");
        //         NodeView _parentView = edge.output.node as NodeView; //Same as above
        //         NodeView _childView = edge.input.node as NodeView;
        //         tree.AddChild(_parentView.node, _childView.node);
        //     });
        // }
        */
        }
        if (_graphviewchange.elementsToRemove != null)
        {
            int _size = _graphviewchange.elementsToRemove.Count;
            for (int i = 0; i < _size; i++)
            {
                if(_graphviewchange.elementsToRemove[i] is NodeView _nodeView)
                    tree.DeleteNode(_nodeView.node); //Delete the node contained in the nodeView

                if (_graphviewchange.elementsToRemove[i] is Edge _edge)
                {
                    NodeView _parentView = _edge.output.node as NodeView; // Get the nodeView from parent and child
                    NodeView _childView = _edge.input.node as NodeView;
                    tree.RemoveChild(_parentView.node, _childView.node); //remove the node (not the visual one) from the list of children
                }
            }
        }
        
        if (_graphviewchange.edgesToCreate != null) // Create edges
        {
            _graphviewchange.edgesToCreate.ForEach(edge =>
            {
                NodeView _parentView = edge.output.node as NodeView; //Same as above
                NodeView _childView = edge.input.node as NodeView;
                tree.AddChild(_parentView.node, _childView.node);
            });
        }
        if (_graphviewchange.movedElements != null)
        {
            nodes.ForEach((n) =>
            {
                NodeView _nodeView = n as NodeView;
                _nodeView.SortChildrenOrder();
            });
        }
        return _graphviewchange;
    }
    
    /// <summary>
    /// Build a contextual menu on Right click with mouse
    /// </summary>
    /// <param name="evt"></param>
    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        base.BuildContextualMenu(evt);
        mousePosition = viewTransform.matrix.inverse.MultiplyPoint(evt.localMousePosition);//Save mousePosition from click to create NodeView here
        string[] _menuNamesArray = Enum.GetNames(typeof(EContextualMenuOriginTypes));
        int _size = _menuNamesArray.Length;
        for (int i = 0; i < _size; i++)
        {
            Type _originType = Type.GetType(_menuNamesArray[i]);
            /* Warning : GetTypesDerivedFrom only get the types that inherit from the type used as template
            * the templated type is not included.*/
            var _subNodeTypes = TypeCache.GetTypesDerivedFrom(_originType); 
            foreach (var _subNodeType in _subNodeTypes)
            {
                int _subTypesCount = _subNodeTypes.Count;
                string _subTypeExtension = $"/[{_subNodeType.Name}]";
                evt.menu.AppendAction($"[{_subNodeType.BaseType.Name}]" + (_subTypesCount > 0 ? _subTypeExtension : ""),
                    (a) => CreateNode(_subNodeType));
            }
        }
    }

    void CreateNode(System.Type _type)
    {
        TreeNode _node = tree.CreateNode(_type); //Create the  SO Node
        if (!_node) return;
        /*Save the mouse Pos used to spawn the nodeView at click position on graphView
         and save it into the node ScriptObject to keep it in memory to load it at this pos later*/
        _node.Position = mousePosition;
        CreateNodeView(_node); //Create the visual element related to this SO
    }
    void CreateNodeView(TreeNode _node)
    {
        NodeView _nodeView = new NodeView(_node);
        _nodeView.onNodeSelected = OnNodeSelected; //Used to update the inspector
        AddElement(_nodeView); //Add this visual element to the list of those to be drawn and displayed.
    }

    public void UpdateNodeState()
    {
        nodes.ForEach(n =>
        {
            NodeView _nodeView = n as NodeView;
            _nodeView.UpdateState();
        });
    }
}
