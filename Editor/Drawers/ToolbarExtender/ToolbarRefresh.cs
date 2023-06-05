using Framework.Core;
using UnityEditor;
using UnityEngine;

namespace Framework.Editor
{
    [InitializeOnLoad]
    public class ToolbarRefresh
    {
        static ToolbarRefresh()
        {
            ToolbarExtender.RegisterRightEntry(OnToolbarGUI, 1);
        }
        
        private static void OnToolbarGUI()
        {
            var content = new GUIContent(EditorGUIUtility.IconContent("d_Refresh"));
            var guiContent = new GUIContent( content.image);
            if (GUILayout.Button(guiContent, EditorStyles.toolbarButton,GUILayout.Width(30.0f)))
            {
                Context.GenerateInstallers();
                ConfigSettings.Settings.GenerateConfigs();
                AssetSettings.Settings.GenerateAssetsScript();
            }
            
            GUILayout.Space(5f);
        }
    }
}