using UnityEditor;
using UnityEngine;
using SF = UnityEngine.SerializeField;

namespace Framework
{
	public class CraftingToolbar : GraphToolbar
	{
		protected override void OnGUI()
		{
			GUIWindow.ToolbarHeight = 21f;
			if (GUILayout.Button("?", EditorStyles.toolbarButton, GUILayout.Width(20f)))
			{
				
			}
		}
	}
}