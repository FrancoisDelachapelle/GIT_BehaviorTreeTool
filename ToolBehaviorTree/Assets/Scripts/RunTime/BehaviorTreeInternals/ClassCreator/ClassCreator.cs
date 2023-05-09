using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;


public static class ClassCreator 
{
    static readonly string TEMPLATE_CLASS_FILENAME = "TemplateNewClass";
    static readonly string className = "#className";
    static readonly string parentClassName = "#parentClass";
    /// <summary>
    /// Create a new .cs file (new class) with the two parameters as the class' name
    /// and its parent's name if it has one
    /// </summary>
    /// <param name="_className">Class Name</param>
    /// <param name="_parentClassName">Parent Class Name</param>
    public static void CreateClassFile(string _className, string _parentClassName)
    {
        TextAsset _textAsset  = Resources.Load<TextAsset>(TEMPLATE_CLASS_FILENAME); //Find template file in ressources folder
        string _text = _textAsset.text;
        _text = _text.Replace(className, _className);
        _text = _text.Replace(parentClassName, _parentClassName =="none" ? "" : _parentClassName); //Replace the preview names with the wished ones
        string _newFileName = _className + ".cs"; //Add extension
        string _filePath = Path.Combine(Application.dataPath, _newFileName);
        File.WriteAllText(_filePath, _text); //Create the cs file
        AssetDatabase.Refresh(); //refresh database to display it
    }
    
    
}
