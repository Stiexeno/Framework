using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
	public abstract class GraphBehaviour : ScriptableObject
	{
		//public abstract List<GraphBehaviour> GetReferencedNodes();

		#if UNITY_EDITOR

		public Vector2 nodePosition;

		#endif
	}
}