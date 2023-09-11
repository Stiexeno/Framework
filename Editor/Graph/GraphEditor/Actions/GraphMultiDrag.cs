using System.Collections.Generic;
using UnityEngine;
using SF = UnityEngine.SerializeField;

namespace Framework
{
	public static class GraphMultiDrag
	{
		public struct DraggingNode
		{
			public GraphNode node;
			public Vector2 offset;
		}
		
		public static IReadOnlyList<DraggingNode> StartDrag(IReadOnlyList<GraphNode> nodes, Vector2 dragStartPosition)
		{
			var draggingSubroots = new List<DraggingNode>();

			// Find the selected roots to apply dragging.
			foreach (GraphNode node in nodes)
			{
				// Unparented nodes are roots.
				// Isolated nodes are their own roots.
				if (node.IsParentless())
				{
					// Calculate the relative position from the node for dragging.
					var draggingRoot = new DraggingNode
					{
						node = node,
						offset = dragStartPosition - node.Center
					};

					draggingSubroots.Add(draggingRoot);
				}

				// Nodes that have a selected parent are not selected roots.
				else if (!nodes.Contains(node.Parent))
				{
					// Calculate the relative position from the node for dragging.
					var draggingRoot = new DraggingNode
					{
						node = node,
						offset = dragStartPosition - node.Center
					};

					draggingSubroots.Add(draggingRoot);
				}
			}

			return draggingSubroots;
		}

		public static void Drag(Vector2 dragPosition, IReadOnlyList<DraggingNode> nodes)
		{
			foreach (DraggingNode root in nodes)
			{
				GraphSingleDrag.SetSubtreePosition(root.node, dragPosition, root.offset);
			}
		}
	}
}