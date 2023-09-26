using System;
using System.Collections.Generic;
using System.Linq;
using Framework;
using UnityEngine;

public abstract class GraphTree : ScriptableObject
{
	public List<GraphBehaviour> nodes = new List<GraphBehaviour>();
	
	[SerializeField, HideInInspector]
	public Blackboard blackboard;
	
	[HideInInspector]
	public Vector2 panPosition = Vector2.zero;

	[HideInInspector]
	public Vector2 zoomPosition = Vector2.one;
	
	public void ClearStructure()
	{
		foreach (GraphBehaviour behaviour in nodes)
		{
			ClearChildrenStructure(behaviour);
			//behaviour.preOrderIndex = BehaviourNode.kInvalidOrder;
			behaviour.indexOrder = 0;
			behaviour.Parent = null;
			behaviour.treeOwner = null;
		}

		nodes = new List<GraphBehaviour>();
	}
	
	private void ClearChildrenStructure(GraphBehaviour behaviour)
	{
		var node = behaviour as Node;
		
		if (node.IsComposite())
		{
			var composite = node as CompositeNode;
			composite.SetChildren(Array.Empty<Node>());
		}
		
		else if (node.IsDecorator())
		{
			var decorator = node as DecoratorNode;
			decorator.SetChild(null);
		}
	}
	
	public void SetNodes(GraphBehaviour root)
	{
		SetNodes(TreeTraversal.PreOrder(root));
	}
	
	public void SetNodes(IEnumerable<GraphBehaviour> nodes)
	{
		this.nodes = nodes.ToList();
		
		int preOrderIndex = 0;
		foreach (GraphBehaviour node in nodes)
		{
			node.treeOwner = this;
			node.preOrderIndex = preOrderIndex++;
		}
	}
}
