using Framework;
using Framework.Editor;
using UnityEditor;
using UnityEngine;
using SF = UnityEngine.SerializeField;

public class BehaviourTreeInspector : GraphInspector
{
	private BTNode node;

	protected override void OnEnable()
	{
		base.OnEnable();
		
		Selection.selectionChanged -= SelectionChanged;
		Selection.selectionChanged += SelectionChanged;
	}

	private void SelectionChanged()
	{
		if (Selection.activeObject != null && Selection.activeObject is BTNode btNode)
		{
			node = btNode;
		}
	}

	protected override void OnGUI(Rect rect)
	{
		GUILayout.Space(15);
		if (node != null)
		{
			var serialziedObjhect = new SerializedObject(node);
			serialziedObjhect.DrawInspectorExcept("m_Script");
		}
	}
}