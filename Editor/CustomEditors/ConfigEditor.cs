using UnityEditor;
using UnityEngine;
namespace Framework
{
    [CustomEditor(typeof(Config), true), CanEditMultipleObjects]
    public class ConfigEditor : UnityEditor.Editor
    {
        //Private fields

        private Config config;
        
        //Properties

        public override void OnInspectorGUI()
        {
	        GUI.enabled = false;
            config.Guid = EditorGUILayout.TextField("GUID", config.Guid);
            GUI.enabled = true;

            var rect = GUILayoutUtility.GetLastRect();

            rect.x += EditorGUIUtility.labelWidth - 30f;
            rect.width = 30;

            var icon = new GUIContent(EditorGUIUtility.IconContent("Refresh@2x"));
            if (GUI.Button(rect, icon))
            {
	            config.Guid = System.Guid.NewGuid().ToString();
	            EditorUtility.SetDirty(config);
	            AssetDatabase.SaveAssets();
            }
            
            base.OnInspectorGUI();
        }

        private void OnEnable()
        {
            config = target as Config;

            if (config.Guid == "UNSET")
            {
                config.Guid = System.Guid.NewGuid().ToString();
                EditorUtility.SetDirty(config);
                AssetDatabase.SaveAssets();
            }
        }
    }
}