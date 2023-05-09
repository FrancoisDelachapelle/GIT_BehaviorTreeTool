using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;
using System.CodeDom;
using System.Linq;
using System.Reflection;
using Microsoft.CSharp;

[CustomPropertyDrawer(typeof(PropertyNumeral))]
public class PropertyNumeralPropertyDrawer : PropertyDrawer
{
    private SerializedObject comparisonNode = null;
    private SerializedProperty blackboard = null;
    private SerializedProperty firstKey = null;
    private SerializedProperty secondKey = null;
    private SerializedProperty comparisonSymbol = null;
    private TreeNode node = null;
    private bool initDone = false;
    private PropertyInfo[] allProperties;
    private int indexFirstKey = 0;
    private int indexSecondKey = 0;
    private int indexSymbol = 0;
    private List<FieldInfo> allFields = new List<FieldInfo>();

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //base.OnGUI(position, property, label);

        InitProperties(property);
        
        EditorGUI.BeginProperty(position, label, property);
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        Rect _firstKeyRect = new Rect(position.x, position.y, position.width / 3, position.height);
        Rect _symbolRect = new Rect(position.x + _firstKeyRect.width, position.y, position.width / 3, position.height);
        Rect _secondKeyRect = new Rect(_symbolRect.x + _symbolRect.width, position.y, position.width / 3, position.height);

       
            EditorGUILayout.BeginHorizontal();
            string[] _allMembersNames = GetAllPopupContent().ToArray();
            indexFirstKey = EditorGUILayout.Popup(indexFirstKey, _allMembersNames);
            indexSymbol = EditorGUILayout.Popup(indexSymbol, comparisonSymbol.enumDisplayNames);
            indexSecondKey = EditorGUILayout.Popup(indexSecondKey, _allMembersNames);
            EditorGUILayout.EndHorizontal();
            //j
        

        
        EditorGUI.BeginChangeCheck();
        EditorGUI.EndChangeCheck();
        
        
        
        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label);
    }

    void InitProperties(SerializedProperty _property)
    {
        comparisonNode = _property.serializedObject;
        node = (TreeNode)comparisonNode.targetObject;
        SerializedObject _object = new SerializedObject(node);
        blackboard = _object.FindProperty("blackboard");
        firstKey = _property.FindPropertyRelative("firstComparer");
        secondKey = _property.FindPropertyRelative("secondComparer");
        comparisonSymbol = _property.FindPropertyRelative("comparisonSymbol");
    }

    List<string> GetAllPopupContent()
    {
        Type _blackboardType = node.blackboard.GetType();
        FieldInfo[] _fields = _blackboardType.GetFields();
        List<string> _allFieldsTypeName = new List<string>();
        foreach (FieldInfo elem in _fields)
        {
            Type _type = elem.FieldType;
            if(_type == typeof(int) || _type == typeof(float))
                _allFieldsTypeName.Add(elem.Name);
            allFields.Add(elem);
        }
        return _allFieldsTypeName;
    }

    string GetPrimitiveName(string _typeName)
    {
        
        CSharpCodeProvider _provider = new CSharpCodeProvider();
        
        string _trueName = "";
        switch (_typeName)
        {
            case "SByte":  
            case "Int16":  
            case "Int32":  
            case "Int64":  
            case "Byte":  
            case "UInt16":  
            case "UInt32":  
            case "UInt64":
            case "Single":  
            case "Double":  
            case "Decimal":
            case "Boolean":
            case "Char":
                _trueName = _provider.GetTypeOutput(new CodeTypeReference(_typeName));
                break;
            default :
                    break;
        }
        
        return _trueName;
    }
}
