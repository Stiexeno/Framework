using Framework.Editor;
using UnityEditor;
using UnityEngine;

public class SequencerGraphNode : BTGraphNode
{
	private BTSequence btSequence;
	
	public override Vector2 Size => new Vector2(200, 80);
	public override Color Outline => new Color(0.22f, 0.79f, 0.23f);
    
	
	public override void OnGUI(Rect rect)
	{
		EditorGUI.LabelField(rect.SetHeight(20f), "Sequencer", GraphStyle.Header0Middle);

		var icon = preferences.sequencerIcon;
        var iconRect = new Rect(rect.x + rect.width / 2f - 50, rect.y + 5, 100, 100);

        if (icon != null)
		{
			GUI.DrawTexture(iconRect, icon);
		}
        
		base.OnGUI(rect);
	}
}