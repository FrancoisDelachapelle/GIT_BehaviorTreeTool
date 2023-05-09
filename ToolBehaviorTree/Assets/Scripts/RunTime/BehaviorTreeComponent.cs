using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorTreeComponent : MonoBehaviour
{
    [SerializeField] public BehaviorTree currentTree = null;
    [SerializeField] private BlackBoard currentBlackboard = null;
    //[SerializeField] PropertyNumeral machin = new PropertyNumeral();
    void Start()
    {
        currentTree = currentTree.Clone();
        currentTree.GiveBlackboardToNodes();
        currentTree.EnableTreeRunning();
    }
    
    void Update()
    {
        currentTree.TreeUpdate();
        //Debug.Log(machin);
    }
}
