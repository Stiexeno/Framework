﻿using System.Collections.Generic;
using Framework;
using Framework.Editor;
using UnityEditor;
using UnityEngine;

namespace Framework.Editor.Graph.BT
{
	public class LeafGraphNode : BTGraphNode
	{
		public override Vector2 Size => new Vector2(175, 30);
		public override Color Outline => new Color(0f, 0f, 0f);
    
		public override void OnGUI(Rect rect)
		{
			DynamicSize = Size;
			EditorGUI.LabelField(rect.SetHeight(20f), $"{Behaviour.GetType().Name}", GraphStyle.Header0Middle);
			base.OnGUI(rect);
		
			//DrawDecorators(rect, new List<string>());
		}
	}
}
