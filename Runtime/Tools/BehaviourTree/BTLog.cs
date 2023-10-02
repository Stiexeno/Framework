using UnityEngine;

namespace Framework.Graph.BT
{
	public class BTLog : BTLeaf
	{
		public string message;

		protected override BTStatus OnUpdate()
		{
			Debug.LogError("DebugLogNode.OnUpdatet()	" + message);
			return BTStatus.Success;
		}
	}
}