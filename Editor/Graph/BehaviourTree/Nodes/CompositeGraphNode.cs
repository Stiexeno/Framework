using DG.DemiEditor;
using UnityEditor;
using UnityEngine;
using SF = UnityEngine.SerializeField;

public abstract class CompositeGraphNode : BTGraphNode
{
	private BTSequence btSequence;

	public override Vector2 Size => new Vector2(175, 70);
	public override Color Outline => Color.green;
	
	public virtual string Header { get; } = "Composite";
	public virtual Texture2D Icon { get; } = null;

	public override void OnGUI(Rect rect)
	{
		EditorGUI.LabelField(rect.SetHeight(15f), Header, GraphStyle.Header0Middle);

		var icon = Icon;
		var iconRect = new Rect(rect.x + rect.width / 2f - 32, rect.y + 25, 64, 32);

		if (icon != null)
		{
			GUI.DrawTexture(iconRect, icon);
		}

		base.OnGUI(rect);
	}
}