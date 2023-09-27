using UnityEngine;

namespace Framework
{
	public abstract class GraphBehaviour : ScriptableObject, IIterableNode<GraphBehaviour>
	{
		internal GraphTree treeOwner = null;
		protected internal int indexOrder = 0;
		
		[SerializeField, HideInInspector]
		internal int preOrderIndex = 0;
		
		public GraphBehaviour Parent { get; set; }
		
		public GraphTree Tree => treeOwner;

		public abstract GraphBehaviour GetChildAt(int index);
		public abstract int ChildCount();
		
		#if UNITY_EDITOR
		[SerializeField, HideInInspector] public Vector2 nodePosition;
		#endif
	}
}