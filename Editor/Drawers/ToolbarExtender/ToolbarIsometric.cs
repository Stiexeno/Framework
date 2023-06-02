using UnityEditor;
using UnityEngine;
using SF = UnityEngine.SerializeField;

namespace Framework.Editor.Isometric
{
    [InitializeOnLoad]
    public class ToolbarIsometric
    {
        private static bool isometricEnabled = false;
        
        static ToolbarIsometric()
        {
            ToolbarExtender.rightToolbarGUI.Add(OnToolbarGUI);
        }
        
        private static void OnToolbarGUI()
        {
            if (isometricEnabled == false)
                GUI.color = new Color(0.53f, 0.53f, 0.53f);
            
            var content = new GUIContent(EditorGUIUtility.IconContent("SceneViewCamera"));
            var guiContent = new GUIContent( content.image);
            if (GUILayout.Button(guiContent, GUILayout.Width(30.0f)))
            {
                isometricEnabled = !isometricEnabled;
                var sceneCamera = SceneView.lastActiveSceneView;
                sceneCamera.pivot = Camera.main.transform.position;
                sceneCamera.orthographic = isometricEnabled;
                sceneCamera.rotation = Camera.main.transform.rotation;
            }
        }
    }
}