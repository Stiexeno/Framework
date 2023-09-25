using Framework;
using Framework.Editor;
using UnityEditor;
using UnityEngine;

public class BTGraphNode : GraphNode
{
	protected GraphPreferences preferences;
	
	private const string PREFS_PATH = "Packages/com.framework.dependency-injection/Editor/Graph/BehaviourTree/Preferences";
	private const string PREFS_NAME = "BehaviourTreePreferences";
	
	public BTGraphNode(GraphBehaviour behaviour) : base(behaviour)
	{
		InitializePrefs();
	}

	private void InitializePrefs()
	{
		preferences = AssetDatabase.LoadAssetAtPath(PREFS_PATH + "/BehaviourTreePreferences.asset", typeof(GraphPreferences)) as GraphPreferences;

		if (preferences == null)
		{
			preferences = ScriptableObject.CreateInstance<GraphPreferences>();
			AssetDatabase.CreateAsset(preferences, PREFS_PATH + "/" + PREFS_NAME + ".asset");
			AssetDatabase.SaveAssets();
		}
	}

	public override void OnGUI(Rect rect)
	{
		DrawDecorators(rect);
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
