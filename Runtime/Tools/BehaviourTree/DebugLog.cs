using UnityEngine;

public class DebugLog : BTLeaf
{
	public string message;

	protected override BTStatus OnUpdate(BTParams btParams)
	{
		Debug.LogError("DebugLogNode.OnUpdatet()	" + message);
		return BTStatus.Success;
	}
}