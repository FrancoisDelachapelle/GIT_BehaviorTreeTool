using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Blackboard", menuName = "Blackboard")]
public class BlackBoard : ScriptableObject
{
    public Vector3 moveToPosition = Vector3.zero;
    public GameObject moveToTarget = null;
    public float distanceToPlayer = 0;
    public float range = 0;

    public BlackBoard Clone()
    {
        BlackBoard _bb = Instantiate(this);
        return _bb;
    }
}
