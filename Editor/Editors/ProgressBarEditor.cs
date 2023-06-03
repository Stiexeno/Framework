using System;
using Framework.Utils;
using UnityEditor;
using UnityEngine;
using SF = UnityEngine.SerializeField;

namespace Framework.Editor
{
	[CustomEditor(typeof(Progressbar))]
	public class ProgressBarEditor : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			Progressbar myTarget = target as Progressbar;

			float previousValue = myTarget.DirectValue;
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Value", GUILayout.MaxWidth(212.0f));
			myTarget.DirectValue = EditorGUILayout.Slider(myTarget.DirectValue, 0, 1, GUILayout.MaxWidth(700.0f));
			EditorGUILayout.EndHorizontal();

			if (Application.isPlaying) return;

			float newValue = myTarget.DirectValue;
			if (Math.Abs(previousValue - newValue) > 0.001f)
			{
				myTarget.DirectValue = newValue;
				EditorUtility.SetDirty(myTarget);
			}
		}
	}
}