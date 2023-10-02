using Framework.Graph.BT;
using UnityEngine;

namespace Framework.Graph
{
	public abstract class GraphBehaviour : ScriptableObject, IIterableNode<GraphBehaviour>
	{
		// Private fields
		
		internal GraphTree treeOwner = null;
		protected internal int indexOrder = 0;
		
		[SerializeField, HideInInspector]
		internal int preOrderIndex = kInvalidOrder;
		
		public const int kInvalidOrder = -1;
		
		// Properties

		public GraphBehaviour Parent { get; set; }
		public abstract int MaxChildCount { get; }
		
		public int PreOrderIndex => preOrderIndex;
		public GraphTree Tree => treeOwner;

		public abstract GraphBehaviour GetChildAt(int index);
		public abstract int ChildCount();
		
		#if UNITY_EDITOR
		[SerializeField, HideInInspector] public Vector2 nodePosition;
		#endif
	}
}