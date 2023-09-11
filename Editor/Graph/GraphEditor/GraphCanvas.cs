using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
	public class GraphCanvas
	{
		private readonly List<GraphNode> nodes = new List<GraphNode>();
		public IReadOnlyList<GraphNode> Nodes => nodes;
		
		public GraphNode Root { get; set; }
		
		public CanvasTransform CanvasTransform { get; set; }

		public GraphCanvas(List<GraphNode> nodes)
		{
			ReconstructNodes(nodes);

			if (Nodes.Count > 0)
			{
				Root = Nodes[0];	
			}
		}

		public void OnGUI()
		{
			UpdateNodeGUI();
		}

		public GraphNode CreateNode(Type nodeType, Type behaviourType)
		{
			var behaviour = ScriptableObject.CreateInstance(behaviourType) as GraphBehaviour;
			var node = Activator.CreateInstance(nodeType, new object[] {behaviour}) as GraphNode;

			nodes.Add(node);
			return node;
		}

		private void UpdateNodeGUI()
		{
			foreach (var node in nodes)
			{
				node.UpdateGUI();
			}
		}
		
		private object ReconstructNodes(IEnumerable<GraphNode> nodes)
		{
			var nodeMap = new Dictionary<GraphBehaviour, GraphNode>();
			
			foreach (var node in nodes)
			{
				GraphNode newNode = ReconstructEditorNode(node);
				nodeMap.Add(newNode.Behaviour, node);
			}

			return nodeMap;
		}

		private GraphNode ReconstructEditorNode(GraphNode node)
		{
			node.Center = node.Behaviour.nodePosition;
			nodes.Add(node);
			return node;
		}

		public void AddChild(GraphNode parent, GraphNode child)
		{
			child.SetParent(parent);
		}
	}
}