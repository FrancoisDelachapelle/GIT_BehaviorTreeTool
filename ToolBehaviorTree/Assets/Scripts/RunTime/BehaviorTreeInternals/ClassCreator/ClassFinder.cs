using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Reflection;
using UnityEditor;

[Serializable]
public static class ClassFinder
{
    /// <summary>
    /// Get all and every classes, generic or not
    /// </summary>
    /// <returns></returns>
    public static List<Type> GetAllClassesNames()
    {
        List<Type> _allTypes = new List<Type>();
        _allTypes = GetClassesFromAssembly().ToList();
        return _allTypes;
    }
    /// <summary>
    /// Get classes deriving from given type cached in Unity
    /// </summary>
    /// <param name="_type">type used to derive classes from</param>
    /// <returns></returns>
    public static List<Type> GetAllCachedNodeClassesNamesDerivedFromType(Type _type) 
    {
        List<Type> _allTypes = new List<Type>();
        //_allTypes = GetClassesFromAssembly().ToList();
        _allTypes = TypeCache.GetTypesDerivedFrom(_type).ToList();
        return _allTypes;
    }
    
    /// <summary>
    /// Get all classes deriving from given type
    /// </summary>
    /// <param name="_type">type used to derive classes from</param>
    /// <returns></returns>
    public static List<Type> GetAllClassesNamesDerivedFromType(Type _type) 
    {
        List<Type> _allTypes = new List<Type>();
        //_allTypes = GetClassesFromAssembly().ToList();
        _allTypes = GetClassesFromAssemblyDerivedFromType(_type);
        return _allTypes;
    }
    
    /// <summary>
    /// Internal function used to get all classes from the assembly
    /// </summary>
    /// <returns></returns>
    static  Type[] GetClassesFromAssembly()
    {
        Assembly _assembly = typeof(ClassFinder).Assembly;
        Type[] _arrayTypes = new Type[]{};
        _arrayTypes = _assembly.GetTypes();
        return _arrayTypes;
    }
    
    /// <summary>
    /// Internal function used to gather all classes deriving from a given type in assembly
    /// </summary>
    /// <param name="_type">type used to derive classes from</param>
    /// <returns></returns>
    static  List<Type> GetClassesFromAssemblyDerivedFromType(Type _type)
    {
        Assembly _assembly = typeof(ClassFinder).Assembly; // get assembly where a class is stocked
        List<Type> _allTypes  = new List<Type>();
        _allTypes = _assembly.GetTypes().Where(t => t != _type && _type.IsAssignableFrom(t)).ToList();
        return _allTypes;
    }
    
    
}
