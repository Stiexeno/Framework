using UnityEditor;
using UnityEngine;

namespace Framework.Editor.Timescale
{
	[InitializeOnLoad]
	public class ToolbarTimeScale
	{
		static ToolbarTimeScale()
		{
			ToolbarExtender.leftToolbarGUI.Add(OnToolbarGUI);
		}

		private static void OnToolbarGUI()
		{
			GUILayout.FlexibleSpace();

			if (GUILayout.Button("Reset"))
			{
				Time.timeScale = 1;
			}
			Time.timeScale = EditorGUILayout.Slider(Time.timeScale, 0, 5, GUILayout.Width(300));
		}
	}
}