using UnityEditor;
using UnityEngine;

namespace Framework.Editor.Controller
{
	[InitializeOnLoad]
    public class ToolbarController
    {
	    static ToolbarController()
	    {
		    ToolbarExtender.RegisterRightEntry(OnToolbarGUI, 2);
	    }

	    private static void OnToolbarGUI()
	    {
		    var content = new GUIContent(EditorGUIUtility.IconContent("d_Settings@2x"));
		    var guiContent = new GUIContent( content.image);
		    if (GUILayout.Button(guiContent, EditorStyles.toolbarButton,GUILayout.Width(30.0f)))
		    {
			    Selection.activeObject = ControllerSettings.Settings;
		    }
            
		    GUILayout.Space(5f);
	    }
    }
}