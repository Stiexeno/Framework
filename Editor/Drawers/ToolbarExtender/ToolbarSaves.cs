using System.IO;
using UnityEditor;
using UnityEngine;
using SF = UnityEngine.SerializeField;

namespace Framework.Editor.Saves
{
	[InitializeOnLoad]
    public class ToolbarSaves
    {
	    static ToolbarSaves()
	    {
		    ToolbarExtender.rightToolbarGUI.Add(OnToolbarGUI);
	    }

	    private static void OnToolbarGUI()
	    {
		    var configPath = Path.Combine("Assets", "Saves");
		    if (!AssetDatabase.IsValidFolder(configPath))
			    GUI.color = new Color(0.53f, 0.53f, 0.53f);
			    
		    var content = new GUIContent(EditorGUIUtility.IconContent("d_TreeEditor.Trash"));
		    var guiContent = new GUIContent( content.image);
		    if (GUILayout.Button(guiContent, GUILayout.Width(30.0f)))
		    {
			    if (AssetDatabase.IsValidFolder(configPath))
			    {
				    AssetDatabase.DeleteAsset(configPath);
				    AssetDatabase.Refresh();   
			    }
		    }
	    }
    }
}