using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using System;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;

public class NodeView : Node
{
    public Action<NodeView> onNodeSelected = null;
    public TreeNode node = null;
    public Port input = null;
    public Port output= null;

    Label descriptionLabel = null;

    public NodeView(TreeNode _node) : base("Assets/Scripts/RunTime//BehaviorTreeInternals/NodeView.uxml")
    {
        node = _node;
        title = _node.name;
        viewDataKey = node.guid;
        style.left = node.Position.x;
        style.top = node.Position.y;
        CreateInputPorts();
        CreateOutputPorts();
        CreateUSSClasses();
        if (_node is Root)
        {
            capabilities &= ~Capabilities.Deletable; //Remove this ability from capabilities
            capabilities &= ~Capabilities.Resizable;
        }
        InitDescription();
       
    }

    private void InitDescription()
    {

        //propertydrawer based
        SerializedObject _node = new SerializedObject(node);
        SerializedProperty _nodeDescription = _node.FindProperty("nodeDescription"); //property belonging to node
        SerializedProperty _description = _nodeDescription.FindPropertyRelative("description"); // member of nodeDescription
        descriptionLabel = this.Q<Label>("Description"); // Description here refers to the name of the UI element : WARNING : It has a "D" instead of "d"
        descriptionLabel.BindProperty(_description);
    }


    //TODO Rework �a avec les fonctions d'input et output port pour les merge en une seule func
    private void CreateUSSClasses()
    {
        if (node is Leaf)
        {
            AddToClassList("leaf");
        }
        else if (node is Composite)
        {
            AddToClassList("composite");
        }
        else if (node is Decorator)
        {
            AddToClassList("decorator");
        }
        else if (node is Root)
        {
            AddToClassList("root");
        }
        //switch (node)       //=> switch buy pattern matching
        //{
        //    case Composite _composite: 
                    //blabla
        //    default:
        //        break;
        //}
    }

    /// <summary>
    /// Create ports for input
    /// </summary>
    private void CreateInputPorts()
    {
        if (node is Leaf)
        {
            /*Orientation indicate the direction of the port, Direction is if it's an entrance to the node or an exit
            * Capacity is used to determine how many connections this port allows and type is the var type the port use to check
            */
            input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
        }
        else if (node is Composite)
        {
            input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
        }
        else if (node is Decorator)
        {
            input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
        }
        else if (node is Root)
        {
        }

        if (input != null)
        {
            input.portName = ""; //Clear the input name to make it clean
            input.style.flexDirection = FlexDirection.Column;
            inputContainer.Add(input); //Add the input to the list of visual elements of input type to be drawn
        }
    }
    /// <summary>
    /// Create ports for output
    /// </summary>
    private void CreateOutputPorts()
    {
        if (node is Leaf)
        {
        }
        else if (node is Composite)
        {
            output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Multi, typeof(bool));
        }
        else if (node is Decorator)
        {
            output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(bool));
        }
        else if (node is Root)
        {
            output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(bool));
        }
        
        if (output != null)
        {
            output.portName = "";//Clear the output name to make it clean
            output.style.flexDirection = FlexDirection.ColumnReverse;
            outputContainer.Add(output);//Add the output to the list of visual elements of input type to be drawn
        }
    }


    /// <summary>
    /// Set the position of the nodeView (graphical Element)
    /// </summary>
    /// <param name="newPos"></param>
    public override void SetPosition(Rect _newPos)
    {
        //Debug.Log($"Set Pos : {_newPos}");
        base.SetPosition(_newPos);
        Undo.RecordObject(node, "Behaviour Tree (Set Position"); //Save for undo
        node.Position = new Vector2(_newPos.x, _newPos.y);
        //node.position.x = _newPos.xMin;
        //node.position.y = _newPos.yMin;

        //Undo aussi mais auxiliaire
        EditorUtility.SetDirty(node); //Appaeremment pas n�cessaire et redondant si on a mis Undo.RecordObject. => SetDirty c'est si on ne veut pas faire d'undo
        //Semble palier au bug de non save apr�s assembly reload avec usage de undo donc on va garder
    }

    /// <summary>
    /// If this element is selected in the window, will trigger
    /// </summary>
    public override void OnSelected()
    {
        base.OnSelected();
        onNodeSelected?.Invoke(this);
    }

    public static bool operator !(NodeView _nodeView)
    {
        return _nodeView == null;
    }

    public void SortChildrenOrder()
    {
        Composite _composite = node as Composite; //(Composite)node cast does not work
        if (!_composite) return;
        _composite.allChildren.Sort(HorizontalPlacementSort); //Strangely just giving the name of the function does the job. no arguments needed
    }

    private int HorizontalPlacementSort(TreeNode _leftNode, TreeNode _rightNode)
    {
        return _leftNode.Position.x < _rightNode.Position.x ? -1 : 1;
    }

    public void UpdateState()
    {
        RemoveFromClassList("Success");
        RemoveFromClassList("Failure");
        RemoveFromClassList("Running");
        if (!Application.isPlaying) return;
        switch (node.CurrentReturnStatus)
        {
            case ENodeReturnStatus.SUCCES:
                AddToClassList("Success");
                break;
            case ENodeReturnStatus.FAILURE:
                AddToClassList("Failure"); 
                break;
            case ENodeReturnStatus.RUNNING:
                if(node.startDone)
                    AddToClassList("Running"); 
                break;
            default:
                break;
        }
    }
}
