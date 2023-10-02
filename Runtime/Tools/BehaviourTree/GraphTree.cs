using System;
using System.Collections.Generic;
using System.Linq;
using Framework.Graph.BT;
using UnityEngine;

namespace Framework.Graph
{
	public abstract class GraphTree : ScriptableObject
	{
		public List<GraphBehaviour> nodes = new List<GraphBehaviour>();

		[SerializeField, HideInInspector] public Blackboard blackboard;

		[HideInInspector] public Vector2 panPosition = Vector2.zero;

		[HideInInspector] public Vector2 zoomPosition = Vector2.one;

		public void ClearStructure()
		{
			foreach (GraphBehaviour behaviour in nodes)
			{
				ClearChildrenStructure(behaviour);
				behaviour.preOrderIndex = GraphBehaviour.kInvalidOrder;
				behaviour.indexOrder = 0;
				behaviour.Parent = null;
				behaviour.treeOwner = null;
			}

			nodes = new List<GraphBehaviour>();
		}

		private void ClearChildrenStructure(GraphBehaviour behaviour)
		{
			var node = behaviour as BTNode;

			if (node.NodeType == NodeType.Composite)
			{
				var composite = node as BTComposite;
				composite.SetChildren(Array.Empty<BTNode>());
			}

			else if (node.NodeType == NodeType.Decorator)
			{
				var decorator = node as BTDecorator;
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

		/// <summary>
		/// Unused nodes are nodes that are not part of the root.
		/// These are ignored when tree executes and excluded when cloning.
		/// </summary>
		[SerializeField] public List<GraphBehaviour> unusedNodes = new List<GraphBehaviour>();
	}
}