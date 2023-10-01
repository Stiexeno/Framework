using System;

public class BehaviourTreeGraphNodeRules : GraphNodeRules<BTNode>
{
	protected override NodeProperties GatherNodeRules(Type behaviour)
	{
		if (behaviour == typeof(BTRoot))
			return new NodeProperties(typeof(RootGraphNode), true);
		
		if (behaviour == typeof(BTSelector))
			return new NodeProperties(typeof(SelectorGraphNode), true);
		
		if (behaviour == typeof(BTParallel))
			return new NodeProperties(typeof(ParallelGraphNode), true);
		
		if (behaviour == typeof(BTSequence))
			return new NodeProperties(typeof(SequenceGraphNode), true);
		
		if (behaviour == typeof(BTDecorator) || behaviour.IsSubclassOf(typeof(BTDecorator)))
			return new NodeProperties(typeof(DecoratorGraphNode), true);
		
		if (behaviour == typeof(BTWait))
			return new NodeProperties(typeof(WaitGraphNode), false);
		
		if (behaviour == typeof(BTLeaf) || behaviour.IsSubclassOf(typeof(BTLeaf)))
			return new NodeProperties(typeof(LeafGraphNode), false);

		return null;
	}
}