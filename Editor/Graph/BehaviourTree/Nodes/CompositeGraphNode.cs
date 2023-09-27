using DG.DemiEditor;
using UnityEditor;
using UnityEngine;
using SF = UnityEngine.SerializeField;

public abstract class CompositeGraphNode : BTGraphNode
{
	private BTSequence btSequence;

	public override Vector2 Size => new Vector2(200, 80);
	public override Color Outline => new Color(0.22f, 0.79f, 0.23f);
	
	public virtual string Header { get; } = "Composite";
	public virtual Texture2D Icon { get; } = null;

	public override void OnGUI(Rect rect)
	{
		EditorGUI.LabelField(rect.SetHeight(20f), Header, GraphStyle.Header0Middle);

		var icon = Icon;
		var iconRect = new Rect(rect.x + rect.width / 2f - 50, rect.y + 5, 100, 100);

		if (icon != null)
		{
			GUI.DrawTexture(iconRect, icon);
		}

		base.OnGUI(rect);
	}
}