using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Framework
{
	public class GraphCanvas : IGraphCanvas
	{
		private readonly List<GraphNode> nodes = new List<GraphNode>();
		public IReadOnlyList<GraphNode> Nodes => nodes;
		
		public GraphNode Root { get; set; }
		public GraphTree Tree { get; set; }
		
		public CanvasTransform CanvasTransform { get; set; }

		public GraphCanvas(GraphTree tree)
		{
			Tree = tree;
			
			var nodeMap = ReconstructEditorNodes(Tree.nodes.Concat(tree.unusedNodes));
			ReconstructEditorConnections(nodeMap);
			
			if (Nodes.Count > 0)
			{
				Root = Nodes[0];	
			}
		}

		public void OnGUI()
		{
			UpdateNodeGUI();
		}

		private void UpdateNodeGUI()
		{
			foreach (var node in nodes)
			{
				node.UpdateGUI();
			}
		}
		
		private Dictionary<GraphBehaviour, GraphNode> ReconstructEditorNodes(IEnumerable<GraphBehaviour> behaviours)
		{
			var nodeMap = new Dictionary<GraphBehaviour, GraphNode>();
			
			foreach (var behaviour in behaviours)
			{
				GraphNode node = ReconstructEditorNode(behaviour);
				nodeMap.Add(behaviour, node);
			}

			return nodeMap;
		}

		private GraphNode ReconstructEditorNode(GraphBehaviour behaviour)
		{
			GraphNode node = CreateNode(behaviour);
			node.Behaviour = behaviour;
			node.Position = behaviour.nodePosition;
			return node;
		}
		
		public GraphNode CreateNode(Type graphBehaviour)
		{
			var behaviour = ScriptableObject.CreateInstance(graphBehaviour) as GraphBehaviour;
			return CreateNode(behaviour);
		}

		private GraphNode CreateNode(GraphBehaviour behaviour)
		{
			var node = CreateEditorNode(behaviour.GetType());
			node.Behaviour = behaviour;
			return node;
		}
		
		private GraphNode CreateEditorNode(Type nodeType)
		{
			var nodeProperty = GraphEditor.GetNodeProperties(nodeType);
			var node = AddEditorNode(nodeProperty);

			return node;
		}

		private GraphNode AddEditorNode(NodeProperties nodeProperties)
		{
			var node = Activator.CreateInstance(nodeProperties.nodeType) as GraphNode;
			node.HasOutput = nodeProperties.hasOutput;
			
			nodes.Add(node);
			return node;
		}
        
		private void ReconstructEditorConnections(Dictionary<GraphBehaviour, GraphNode> nodeMap)
		{
			foreach (GraphNode node in nodes)
			{
				int childCount = node.Behaviour.ChildCount();
				for (int i = 0; i < childCount; i++)
				{
					GraphBehaviour child = node.Behaviour.GetChildAt(i);
					nodeMap[child].SetParent(node);
				}
			}
		}

		public void AddChild(GraphNode parent, GraphNode child)
		{
			child.SetParent(parent);
		}

		public void Remove(GraphNode node)
		{
			if (nodes.Remove(node))
			{
				if (node == Root)
				{
					Root = null;
				}
				
				node.Remove();
			}
		}
		
		public void Remove(Predicate<GraphNode> match)
		{
			List<GraphNode> nodesToDestroy = nodes.FindAll(match);
			nodes.RemoveAll(match);

			// Clear root if removed.
			if (nodesToDestroy.Contains(Root))
			{
				Root = null;
			}

			foreach (GraphNode node in nodesToDestroy)
			{
				node.Remove();
			}
		}
		
		public void SetRoot(GraphNode newRoot)
		{
			if (newRoot.Parent == null)
			{
				Root = newRoot;
			}
			else
			{
				Debug.LogWarning("Root cannot be a child.");
			}
		}
	}
}