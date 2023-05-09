using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor.Callbacks;


public class BehaviorTreeEditorWindow : EditorWindow
{
    private BehaviorTreeView treeView = null;
    private InspectorView inspectorView = null;
    private IMGUIContainer blackboardView = null;
    private Button createNodeClassButton = null;
    SerializedObject treeObject = null;
    SerializedProperty blackboardProperty = null;

    [MenuItem("BehaviorTreeEditorWindow/Editor ...")]
    public static void OpenWindow()
    {
        BehaviorTreeEditorWindow wnd = GetWindow<BehaviorTreeEditorWindow>();
        wnd.titleContent = new GUIContent("BehaviorTreeEditorWindow");
    }

    [OnOpenAsset]
    public static bool OnOpenAsset(int _instanceID, int _line)
    {
        if(Selection.activeObject is BehaviorTree)
        {
            OpenWindow();
            return true;
        }
        return false;
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;
        

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/RunTime/BehaviorTreeInternals/BehaviorTreeEditorWindow.uxml");
        visualTree.CloneTree(root);
   

        // A stylesheet can be added to a VisualElement.
        // The style will be applied to the VisualElement and all of its children.
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/RunTime/BehaviorTreeInternals/BehaviorTreeEditorWindow.uss");
        root.styleSheets.Add(styleSheet);

        treeView = root.Query<BehaviorTreeView>(); //Get the instance of the view from the window UI itself
        inspectorView = root.Query<InspectorView>();
        blackboardView = root.Query<IMGUIContainer>();
        createNodeClassButton = root.Query<Button>("CreateClassButton");
        createNodeClassButton.clicked += OpenClassCreatorWindow;
        
        blackboardView.onGUIHandler += DisplayBlackboardContent;
        treeView.OnNodeSelected = OnNodeSelectionChange;
        OnSelectionChange(); //Called on update
    }

    void DisplayBlackboardContent()
    {
        if (treeObject == null || treeObject.targetObject == null) return;
        treeObject.Update();
        EditorGUILayout.PropertyField(blackboardProperty);
        treeObject.ApplyModifiedProperties();
    }

    /// <summary>
    /// If a valid asset (that can be opened in editor) is selected, will display its content in the window
    /// </summary>
    private void OnSelectionChange()
    {
        BehaviorTree _tree = Selection.activeObject as BehaviorTree;          
        if(!_tree)
        {
            if (Selection.activeGameObject)
            {
                BehaviorTreeComponent _behaviorTreeCompo = Selection.activeGameObject.GetComponent<BehaviorTreeComponent>();
                if (!_behaviorTreeCompo)
                {
                    Debug.Log("Error : No active Behavior Tree Component in the active game object");
                    return;
                }
                _tree = _behaviorTreeCompo.currentTree;
            }
        }
        if(Application.isPlaying)
        {
            if (_tree != null && treeView != null)
                treeView.PopulateView(_tree); //Draw and display the asset content on the window.
        }
        else
        {
            if(_tree && AssetDatabase.CanOpenAssetInEditor(_tree.GetInstanceID()))
                treeView.PopulateView(_tree); //Draw and display the asset content on the window.
        }
        if(_tree)
        {
            treeObject = new SerializedObject(_tree);
            blackboardProperty = treeObject.FindProperty("blackboard");
        }
    }

    private void OnEnable()
    {
        EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private void OnDisable()
    {
        EditorApplication.playModeStateChanged -= OnPlayModeStateChanged; 
        
    }
    void OnPlayModeStateChanged(PlayModeStateChange _change)
    {
        switch (_change)
        {
            case PlayModeStateChange.EnteredEditMode:
                OnSelectionChange();
                blackboardView.onGUIHandler += DisplayBlackboardContent;
                break;
            case PlayModeStateChange.ExitingEditMode:
                break;
            case PlayModeStateChange.EnteredPlayMode:
                OnSelectionChange();
                break;
            case PlayModeStateChange.ExitingPlayMode:
                blackboardView.onGUIHandler -= DisplayBlackboardContent;
                break;
            default:
                break;
        }
    }

    void OnNodeSelectionChange(NodeView _node)
    {
        inspectorView.UpdateSelection(_node);
    }

    private void OnInspectorUpdate()
    {
        if (treeView == null) return;
        treeView?.UpdateNodeState();
    }

    void OpenClassCreatorWindow()
    {
        ClassCreatorWindow _window = GetWindow<ClassCreatorWindow>();
        _window.minSize = new Vector2(520, 500);
        _window.maxSize = new Vector2(520, 500);
    }
}