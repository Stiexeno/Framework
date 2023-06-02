using UnityEditor;
using UnityEngine;

namespace Framework.Editor
{
	public class EditorSkin
	{
		public readonly int titlebarHeight = 20;
		public readonly int imageIconSize = 16;
		public readonly int imagePadding = EditorStyles.foldout.padding.left + 1;

		public readonly GUIStyle boldFoldout = new GUIStyle(EditorStyles.foldout)
		{
			fontStyle = FontStyle.Bold,
			padding = new RectOffset(EditorStyles.foldout.padding.left + 16 + 2, 0, 2, 0),
		};

		public readonly GUIStyle inspectorTitlebar = new GUIStyle("IN Title")
		{
			alignment = TextAnchor.MiddleLeft
		};

		public readonly string cross = "\u2715";
		public readonly float buttonHeight = EditorGUIUtility.singleLineHeight;
		public readonly float buttonWidth = 19.0f;

		public Color inspectorTitlebarBackground =>
			EditorGUIUtility.isProSkin ? new Color32(64, 64, 64, 255) : new Color32(222, 222, 222, 255);
	}	
}