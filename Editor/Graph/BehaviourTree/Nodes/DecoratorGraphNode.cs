using Framework.Editor;
using UnityEditor;
using UnityEngine;

namespace Framework.Editor.Graph.BT
{
	public class DecoratorGraphNode : BTGraphNode
	{
		public override Vector2 Size => new Vector2(175, 60);
		public override Color Outline => new Color(1f, 0.56f, 0f);

		public override void OnGUI(Rect rect)
		{
			DynamicSize = Size;
			EditorGUI.LabelField(rect.SetHeight(20f), $"{Behaviour.GetType().Name}", GraphStyle.Header0Middle);
			GraphStyle.DrawHorizontalLine(rect.AddY(30));

			GUI.color = new Color(0.97f, 0.02f, 0f);
			EditorGUI.LabelField(rect.SetWidth(20).SetHeight(20).AddX(-2).AddY(35), new GUIContent("", BehaviourTreePreferences.Instance.dotIcon));
			GUI.color = Color.white;
			if (GUI.Button(rect.AddX(15f).AddY(35).SetHeight(20), $"Self", EditorStyles.label))
			{
			}
		}
	}
}
