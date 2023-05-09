using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(NodeDescriptionProperty))]
public class NodeDescriptionPropertyDrawer : PropertyDrawer
{
    SerializedProperty displayDesc = null;
    SerializedProperty desc = null;
  

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        displayDesc = property.FindPropertyRelative("displayDescription");
        desc = property.FindPropertyRelative("description");
        
        EditorStyles.textField.wordWrap = true;// allows the text to create a new line when too large
        EditorGUI.BeginProperty(position, label, property);
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        int _indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = -2; //reduce indentation level to make the displayed properties nearer to the label
        Rect _toggleRect = new Rect(position.x, position.y - 15, position.width/5, position.height);
        Rect _descRect = new Rect(position.x + _toggleRect.width, position.y, position.width * .75f, position.height);
        
        EditorGUI.BeginChangeCheck();//check if there was a change in a displayed property
        displayDesc.boolValue = EditorGUI.Toggle(_toggleRect, displayDesc.boolValue); // create a toggle
        if (displayDesc.boolValue)
            desc.stringValue = EditorGUI.TextField(_descRect, desc.stringValue, EditorStyles.textField); // create a text field
        EditorGUI.EndChangeCheck();//end the check and modify the properties accordingly
        
        EditorGUI.indentLevel = _indent; // reset indentation level to previous value
        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float _totalHeight = base.GetPropertyHeight(property, label) * 3; //create a height equal to property height * 3 (lines)
        return _totalHeight;
    }

}
