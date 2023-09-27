using System;

public class BehaviourTreeGraphNodeRules : GraphNodeRules<BTNode>
{
	protected override NodeProperties GatherNodeRules(Type behaviour)
	{
		if (behaviour == typeof(Root))
			return new NodeProperties(typeof(RootGraphNode), true);
		
		if (behaviour == typeof(BTSelector))
			return new NodeProperties(typeof(SelectorGraphNode), true);
		
		if (behaviour == typeof(BTSequence))
			return new NodeProperties(typeof(SequenceGraphNode), true);

		return null;
	}
}