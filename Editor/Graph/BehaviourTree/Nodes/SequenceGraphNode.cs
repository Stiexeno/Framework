using UnityEngine;

public class SequenceGraphNode : CompositeGraphNode
{
	private BTSequence btSequence;

	public override string Header => "Sequencer";
	public override Texture2D Icon => BehaviourTreePreferences.Instance.sequencerIcon;
}