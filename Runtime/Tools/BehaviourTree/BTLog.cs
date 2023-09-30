using UnityEngine;

public class BTLog : BTLeaf
{
	public string message;

	protected override BTStatus OnUpdate(BTParams btParams)
	{
		Debug.LogError("DebugLogNode.OnUpdatet()	" + message);
		return BTStatus.Success;
	}
}