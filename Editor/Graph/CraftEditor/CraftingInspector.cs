using Framework.Editor;
using UnityEditor;
using UnityEngine;

namespace Framework
{
	public class CraftingInspector : GraphInspector
	{
		protected override void OnGUI(Rect rect)
		{
			for (int i = 0; i < 10; i++)
			{
				GUILayout.BeginVertical();

				if (GUILayout.Button(FenrirEditor.CreateNamedIcon("Recipe_Gas", "d_ScriptableObject Icon"), EditorStyles.label, GUILayout.Height(18)))
				{
				}

				GUILayout.EndVertical();

				if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Tab)
				{
					Event.current.Use();
				}
			}
		}
	}
}