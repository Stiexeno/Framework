using UnityEngine;
using SF = UnityEngine.SerializeField;

namespace Framework
{
	public class GraphInputEvent
	{
		public CanvasTransform transform;
		public Vector2 canvasMousePostion;
		public GraphNode node;
		public bool isInputFocused;
		public bool isOutputFocused;

		public bool IsPortFocused()
		{
			return isInputFocused || isOutputFocused;
		}

		public bool IsNodeFocused()
		{
			return node != null;
		}
	}
}