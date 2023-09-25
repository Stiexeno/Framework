using UnityEngine;

public class DebugLogNode : ActionNode
{
	public string message;
	
	protected override void OnStart()
	{
		Debug.LogError("DebugLogNode.OnStart()	" + message);
	}
	
	protected override State OnUpdate()
	{
		Debug.LogError("DebugLogNode.OnUpdatet()	" + message);
		return State.Success;
	}
	
	protected override void OnStop()
	{
		Debug.LogError("DebugLogNode.OnStop()	" + message);
	}
}