using Framework;
using Framework.Editor;
using UnityEditor;
using UnityEngine;

public class RootGraphNode : BTGraphNode
{
	private Root root;
	
	public override Vector2 Size => new Vector2(100, 40);
	public override Color Outline => new Color(0.73f, 0.73f, 0.75f);

	public override void OnGUI(Rect rect)
	{
		DynamicSize = Size;
		EditorGUI.LabelField(rect.SetHeight(20f), "Root", GraphStyle.Header0Middle);
		
		//base.OnGUI(rect);
	}
}