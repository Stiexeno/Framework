using System;

public class BehaviourTreeGraphNodeRules : GraphNodeRules<Node>
{
	protected override NodeProperties GatherNodeRules(Type behaviour)
	{
		if (behaviour == typeof(RootNode))
		{
			return new NodeProperties(typeof(RootGraphNode), true);
		}
		
		if (behaviour == typeof(SequencerNode))
		{
			return new NodeProperties(typeof(SequencerGraphNode), true);
		}

		return null;
	}
}