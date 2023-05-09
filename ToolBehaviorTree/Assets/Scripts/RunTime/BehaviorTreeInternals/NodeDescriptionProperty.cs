using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class NodeDescriptionProperty
{
    [SerializeField] bool displayDescription = false;
    [SerializeField, TextArea(15,20)] string description= "";

    public bool DisplayDescription => displayDescription;
    public string Description => description;

    public NodeDescriptionProperty(bool _displayDescription) => displayDescription = _displayDescription;
}
