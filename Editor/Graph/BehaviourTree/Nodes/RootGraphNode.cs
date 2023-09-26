using Framework;
using Framework.Editor;
using UnityEditor;
using UnityEngine;

public class RootGraphNode : BTGraphNode
{
	private RootNode rootNode;
	
	public override Vector2 Size => new Vector2(100, 40);
	public override Color Outline => new Color(0.73f, 0.73f, 0.75f);

	public override void OnGUI(Rect rect)
	{
		EditorGUI.LabelField(rect.SetHeight(20f), "Root", GraphStyle.Header0Middle);
		
		base.OnGUI(rect);
	}
}