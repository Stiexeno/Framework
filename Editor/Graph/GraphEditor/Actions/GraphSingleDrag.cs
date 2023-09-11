using System.Linq;
using UnityEngine;
using SF = UnityEngine.SerializeField;

namespace Framework
{
	public static class GraphSingleDrag
	{
		public static Vector2 StartDrag(GraphNode node, Vector2 mousePosition)
		{
			return mousePosition - node.Center;
		}
		
		public static void Drag(GraphNode node, Vector2 position, Vector2 offset)
		{
			SetSubtreePosition(node, position, offset);
		}

		public static void SetSubtreePosition(GraphNode root, Vector2 dragPosition, Vector2 offset)
		{
			float min = float.MinValue;

			if (!root.IsParentless())
			{
				const float minOffset = 10;
				float nodeTop = root.RectPosition.yMin - (root.RectPosition.height / 2) - minOffset;
				float parentBottom = root.Parent.RectPosition.yMax + (root.Parent.RectPosition.height / 2) + minOffset;

				// The root cannot be above its parent.
				if (nodeTop < parentBottom)
				{
					min = parentBottom;
				}
			}

			// Record the old position to later determine the translation delta to move children.
			Vector2 oldPosition = root.Center;

			// Clamp the position so it does not go above the parent.
			Vector2 newPosition = dragPosition - offset;
			newPosition.y = Mathf.Clamp(newPosition.y, min, float.MaxValue);

			float snap = GraphPreferences.Instance.snapThreshold; // TODO: Add snap threshold into preferences

			root.Center = GraphMathUtility.SnapPosition(newPosition, snap);

			// Calculate the change of position of the root.
			Vector2 pan = root.Center - oldPosition;

			// Move the entire subtree of the root.
			// For all children, pan by the same amount that the parent changed by.
			foreach (GraphNode node in GraphTraversal.PreOrder(root).Skip(1))
			{
				node.Center = GraphMathUtility.SnapPosition(node.Center + pan, snap);
			}
		}
	}
}