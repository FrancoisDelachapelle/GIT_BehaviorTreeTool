using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using System.Reflection;
public class InspectorView : VisualElement
{
    public new class UxmlFactory : UxmlFactory<InspectorView, VisualElement.UxmlTraits> { }

    private Editor editor = null;
    public InspectorView()
    {
    }

    /// <summary>
    /// Update the inspector in the window.
    /// </summary>
    /// <param name="_nodeView"> nodeView selected/deselected</param>
    public void UpdateSelection(NodeView _nodeView)
    {
        Clear();
        UnityEngine.Object.DestroyImmediate(editor);
        editor = Editor.CreateEditor(_nodeView.node); //Make a custom editor to contains the node informations
        IMGUIContainer _container = new IMGUIContainer(() =>
        { 
            if(editor.target)
            {
               editor.OnInspectorGUI();             
            }
            
        }); // Create a container IMGUI which will
            // manage the given actions of the editor
        Add(_container);//Add it to the window
    }
}
