using System;
using System.Collections.Generic;
using System.Linq;
using Framework.Graph;
using Framework.Graph.BT;
using UnityEditor;
using Object = UnityEngine.Object;

namespace Framework.Editor.Graph
{
	public class GraphSelection : IGraphSelection
	{
		private readonly List<GraphNode> selectedNodes = new();
		private readonly List<GraphBehaviour> referencedNodes = new List<GraphBehaviour>();

		public GraphNode SingleSelectedNode => SelectedNodes.FirstOrDefault();
		public IReadOnlyList<GraphNode> SelectedNodes => selectedNodes;
		public IReadOnlyList<GraphBehaviour> Referenced => referencedNodes;

		public int SelectedCount => SelectedNodes.Count;

		public bool IsEmpty => SelectedNodes.Count == 0;
		public bool IsSingleSelection => SelectedNodes.Count == 1;
		public bool IsMultiSelection => SelectedNodes.Count > 1;
		
		public event EventHandler<GraphNode> SingleSelected;

		public void SetSingleSelection(GraphNode selectedNode)
		{
			ClearSelection();
			selectedNodes.Add(selectedNode);
            
			selectedNode.Selected = true;

			Selection.objects = null;
			Selection.activeObject = selectedNode.Behaviour;
			SingleSelected?.Invoke(this, selectedNode);
			SetReferenced(selectedNode.Behaviour);
		}
		
		public void SetMultiSelection(List<GraphNode> newSelection)
		{
			if (newSelection.Count == 1)
			{
				SetSingleSelection(newSelection[0]);
			}
			else
			{
				ClearSelection();

				if (newSelection.Count > 0)
				{
					selectedNodes.AddRange(newSelection);
					Selection.objects = newSelection.Select(node => node.Behaviour).Cast<Object>().ToArray();
					
					newSelection.ForEach(node => node.Selected = true);
				}
			}
		}

		public void ClearSelection()
		{
			foreach (var selectedNode in selectedNodes)
			{
				selectedNode.Selected = false;
			}

			selectedNodes.Clear();
			Selection.objects = null;
			Selection.activeObject = null;
		}

		public bool IsNodeSelected(GraphNode node)
		{
			return SelectedNodes.Contains(node);
		}

		public bool IsReferenced(GraphNode node)
		{
			return Referenced.Contains(node.Behaviour);
		}
		
		public void SetReferenced(GraphBehaviour node)
		{
			//referencedNodes.Clear();
			//List<GraphBehaviour> refs = node.GetReferencedNodes();
			//if (refs != null && refs.Count != 0)
			//{
			//	referencedNodes.AddRange(refs);
			//}
		}

		public void SetTreeSelection(GraphTree tree)
		{
			referencedNodes.Clear();
			ClearSelection();
			Selection.activeObject = tree;
		}
	}
}