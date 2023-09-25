﻿using Framework;
using Framework.Editor;
using UnityEditor;
using UnityEngine;

public class ActionGraphNode : BTGraphNode
{
	private ActionNode sequencerNode;
	
	public override Vector2 Size => new Vector2(200, 30);
	public override Color Outline => new Color(0f, 0f, 0f);
	public override bool HasOutput => false;
	
	public ActionGraphNode(GraphBehaviour behaviour) : base(behaviour)
	{
		sequencerNode = behaviour as ActionNode;
	}
	
	public override void OnGUI(Rect rect)
	{
		EditorGUI.LabelField(rect.SetHeight(20f), "Action", GraphStyle.Header0Middle);
		base.OnGUI(rect);
	}
}