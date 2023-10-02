using System.Collections.Generic;
using Framework;
using Framework.Editor;
using UnityEditor;
using UnityEngine;

namespace Framework.Editor.Graph.BT
{
	public class BTGraphNode : GraphNode
	{
		public override void OnGUI(Rect rect)
		{
			DynamicSize = Size;
			var position = Size;
			GraphStyle.DrawHorizontalLine(rect.AddY(position.y));
			//EditorGUI.LabelField(rect.SetHeight(20f).AddY(position.y), "- Decorators -", GraphStyle.Header1Middle);

			position.y += 20;
			DynamicSize = position;
			//DrawDecorators(rect);
		}

		protected void DrawDecorators(Rect rect, List<string> decorators)
		{
			//if (decorators.Count <= 0)
			//	return;

			var position = Size;
			EditorGUI.LabelField(rect.SetHeight(20f).AddY(position.y), "- Decorators -", GraphStyle.Header1Middle);
			position.y += 20;

			for (int i = 0; i < 5; i++)
			{
				EditorGUI.LabelField(rect.SetHeight(20f).AddY(position.y), "Check for type", GraphStyle.Header1Left);
				position.y += 18;
			}

			position.y += 10;
			GraphStyle.DrawHorizontalLine(rect.AddY(position.y));
			position.y += 20;
			DynamicSize = position;
		}

		private void DrawDecorators(Rect rect)
		{
			DynamicSize = Size;
			var position = Size;
			GraphStyle.DrawHorizontalLine(rect.AddY(position.y));
			EditorGUI.LabelField(rect.SetHeight(20f).AddY(position.y), "- Decorators -", GraphStyle.Header1Middle);

			position.y += 40;
			DynamicSize = position;
		}
	}
}