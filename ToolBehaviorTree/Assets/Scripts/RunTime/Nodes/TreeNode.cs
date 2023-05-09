using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;


[Serializable]
public abstract class TreeNode : ScriptableObject
{
    // Start is called before the first frame update
    [SerializeField, HideInInspector] protected ENodeReturnStatus currentReturnStatus = ENodeReturnStatus.RUNNING;
    [SerializeField, HideInInspector] protected ENodeReturnStatus previousReturnStatus = ENodeReturnStatus.RUNNING;
    [SerializeField] public BlackBoard blackboard = null;
    [SerializeField, HideInInspector] public bool startDone = false, processDone = false, stopDone = false;
    [SerializeField, HideInInspector] public int nodeID = 0;
    [SerializeField, HideInInspector] public string guid ="";
    [SerializeField, HideInInspector] protected Vector2 position = Vector2.zero;
    //[SerializeField, TextArea] public string description =""; //TextArea allows the text to use multiple lines in the inspector
    [SerializeField] protected NodeDescriptionProperty nodeDescription = new NodeDescriptionProperty(false); //TextArea allows the text to use multiple lines in the inspector
   // [SerializeField] protected bool showDescription = false; 

    public Vector2 Position {get => position;set => position = value;}
    public ENodeReturnStatus CurrentReturnStatus => currentReturnStatus;
   // public string Description { get => description; set => description = value; }
    //public bool ShowDescription { get => showDescription; set => showDescription = value; }
    public NodeDescriptionProperty NodeDescription => nodeDescription;

    /// <summary>
    /// Coroutine that will execute the node - Start, Process then closes it.
    /// </summary>
    /// <returns></returns>
    public ENodeReturnStatus Execute()
    {
        if (!startDone)
        {
            StartNode();
            startDone = true;
        }
       currentReturnStatus = ProcessNode();
       if (currentReturnStatus != ENodeReturnStatus.RUNNING)
       {
            StopNode();
            startDone = false;
       }
       return currentReturnStatus;
    }


    /// <summary>
    /// Clone the node for avoiding original modifications. Make a separate instance to use.
    /// </summary>
    /// <returns></returns>
    public virtual TreeNode Clone()
    {
        return Instantiate(this);
    }
    protected abstract void StartNode();
    protected abstract ENodeReturnStatus ProcessNode();

    protected abstract void StopNode();
}
