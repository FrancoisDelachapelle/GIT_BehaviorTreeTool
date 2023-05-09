using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ClassCreatorWindow : EditorWindow
{
    Vector2 scrollPosition = Vector2.zero;
    string selectedClassName = "Selected class";
    const string mainTitle = "Create a new Node Class";
    string searchName = "Search your parent class here";
    string newClassName = "Input your class name here";
    private List<Type> allTypes = new List<Type>();
    private int size = 0;
    private float labelWidth = 0;
    private void OnGUI()
    {
        DrawWindowContent();
    }


    private void Awake()
    {
        Init();
    }

    void Init()
    {
        allTypes = ClassFinder.GetAllClassesNamesDerivedFromType(typeof(TreeNode)); //Get all classes deriving from TreeNode to display
        size = allTypes.Count;
        labelWidth = EditorGUIUtility.labelWidth;
        EditorGUIUtility.labelWidth = 350;
    }

    void DrawWindowContent()
    {
        //Main title
        EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(position.width), GUILayout.MaxHeight(20));
            GUILayout.FlexibleSpace();
            GUILayout.Label(mainTitle, EditorStyles.boldLabel, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
        //Main title
        
        //TextFields Area
            Rect _rectFields = EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true)); //begin a horizontal area
            //ClassName Area 
            EditorGUILayout.Space(5); //5 pixels margin first to the left of all elements in the horizontal area
            EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(false), GUILayout.Width(_rectFields.width/2)); //Vertical area
            
                EditorGUILayout.LabelField("New Class Name");
                newClassName = EditorGUILayout.TextField(newClassName); // input field to write class name
                
            EditorGUILayout.EndVertical();
            //ClassName Area 
            GUILayout.FlexibleSpace(); // Will push all further elements to the right
        //Search Area 
            EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(false), GUILayout.MaxWidth(_rectFields.width/2));
            
                EditorGUILayout.LabelField("Selected Parent Class"); 
                selectedClassName = EditorGUILayout.TextField(selectedClassName);
                EditorGUILayout.Space(10); // in a vertical area the space is in fact vertical (the rectangle is view as rotated so its width is now down-top)
                EditorGUILayout.LabelField("Parent Class Search");
                searchName = EditorGUILayout.TextField(searchName); // input field to search class 
                EditorGUILayout.Separator();
                DrawAllClassesList(); // will call the scrollview to display class list
                
            EditorGUILayout.EndVertical();
            GUILayout.Space(5); // 5 pixels to the right
        //Search Area 
        EditorGUILayout.EndHorizontal();
        //TextFields Area
        
        GUILayout.FlexibleSpace(); // This one will push all elements down the window
        EditorGUILayout.BeginHorizontal(); // new horizontal area
        
            if(GUILayout.Button("Create"))
            {
                if (selectedClassName == "Selected class" || selectedClassName == "")
                    selectedClassName = "none";
                ClassCreator.CreateClassFile(newClassName, selectedClassName); // if the button is pressed, will call the classCreator function
                Close(); // Close the window afterward
            }
            if (GUILayout.Button("Close"))
                Close(); // just close the window
            
        EditorGUILayout.EndHorizontal();
    }

    void DrawAllClassesList()
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.MaxHeight(200), GUILayout.MinWidth(200)); //create a scrollview with minwidth set
        for (int i = 0; i < size; i++) // that way it wont resize to a smaller version with less content
        {
            string _displayName = allTypes[i].ToString();
            if (searchName != "Search your parent class here") // if no search yet, then need to display all classes
                if (!_displayName.ToLower().StartsWith(searchName.ToLower())) continue; // otherwise do not display classes with no relation with research name
            if(_displayName=="Root")continue; // We don't want root class to be inherited
            if(GUILayout.Button(_displayName, EditorStyles.miniButton))
            {
                selectedClassName = _displayName; // Set selected Class name if button is pressed
            }
            EditorGUILayout.Separator();

        }
        EditorGUILayout.EndScrollView();
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
    }
}

