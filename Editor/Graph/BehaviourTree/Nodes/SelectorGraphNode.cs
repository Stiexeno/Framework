using UnityEngine;
using SF = UnityEngine.SerializeField;

public class SelectorGraphNode : CompositeGraphNode
{
	private BTSequence btSequence;

	public override string Header => "Selector";
	public override Texture2D Icon => BehaviourTreePreferences.Instance.selectorIcon;
}