using Framework;
using Framework.Editor;
using UnityEditor;
using UnityEngine;
using SF = UnityEngine.SerializeField;

public class WaitGraphNode : BTGraphNode
{
	private bool editMode;
	
	public override Vector2 Size => new Vector2(175, 105);
	public override Color Outline => new Color(0f, 0f, 0f);

	public override void OnGUI(Rect rect)
	{
		DynamicSize = Size;
		EditorGUI.LabelField(rect.SetHeight(20f), "Wait", GraphStyle.Header0Middle);

		var icon = BehaviourTreePreferences.Instance.waitIcon;
		var iconRect = new Rect(rect.x + rect.width / 2f - 32, rect.y + 30, 64, 32);

		if (icon != null)
		{
			GUI.DrawTexture(iconRect, icon);
		}
		
		GraphStyle.DrawHorizontalLine(rect.AddY(70));
		
		var waitBehaviour = (BTWait)Behaviour;

		rect = rect.SetHeight(20f).AddY(80).AddWidth(-10f).AddX(5f);

		if (editMode)
		{
			if ((Event.current.type == EventType.Ignore || Event.current.type == EventType.Used) &&
			    rect.Contains(Event.current.mousePosition) == false)
			{
				editMode = false;
				Event.current.Use();
			}
			
			if (GraphInput.IsEnterAction(Event.current))
			{
				editMode = false;
				Event.current.Use();
			}
			
			GUI.SetNextControlName("SearchField");
			waitBehaviour.duration = EditorGUI.FloatField(rect, waitBehaviour.duration);
			EditorGUI.FocusTextInControl("SearchField");
		}
		else
		{
			GUI.color = new Color(0.97f, 0.59f, 0f);
			EditorGUI.LabelField(rect.SetWidth(20).SetHeight(20).AddX(-2), new GUIContent("", BehaviourTreePreferences.Instance.dotIcon));
			GUI.color = Color.white;
			if (GUI.Button(rect.AddX(15f), $"{waitBehaviour.duration} Seconds", EditorStyles.label))
			{
				editMode = true;
			}
		}
		
		DynamicSize += new Vector2(0, 20);
		
		var loadingRect = rect.SetHeight(18f).AddY(22f);
		
		EditorGUI.DrawRect(loadingRect, new Color(0.18f, 0.18f, 0.19f));
		
		var progress = waitBehaviour.Timelasped / waitBehaviour.duration;
		
		EditorGUI.DrawRect(loadingRect.SetWidth(loadingRect.width * progress), new Color(0.4f, 0.4f, 0.4f));
	}
}
