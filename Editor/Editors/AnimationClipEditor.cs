using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof (AnimationClip), true)]
public class AnimationClipEditor : Editor
{
	private AnimationClip clip;
	private Editor editorInstance;

	private bool Legacy
	{
		get => legacy;

		set
		{
			if (legacy != value)
			{
				legacy = value;
				clip.legacy = legacy;
				EditorUtility.SetDirty(clip);
			}
		}
	}

	private bool legacy;
	
	private void OnEnable()
	{
		clip = target as AnimationClip;
		Legacy = clip.legacy;
		editorInstance = CreateEditor(targets, Type.GetType("UnityEditor.AnimationClipEditor, UnityEditor"));
	}
	
	public override void OnInspectorGUI()
	{
		Legacy = EditorGUILayout.Toggle("Legacy", Legacy);
		if (Legacy)
		{
			EditorGUILayout.HelpBox("Animancer requires not legacy animations", MessageType.Info);	
		}
		
		editorInstance.OnInspectorGUI();
	}
}
