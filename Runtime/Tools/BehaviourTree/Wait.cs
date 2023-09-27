using UnityEngine;

public class Wait : BTLeaf
{
	public float duration;

	private float timelpased;
	
	protected override void OnEnter(BTParams btParams)
	{
		timelpased = 0;
		Debug.LogError("Start");
	}

	protected override BTStatus OnUpdate(BTParams btParams)
	{
		timelpased += Time.deltaTime;
		
		Debug.LogError(timelpased);
		if (timelpased >= duration)
		{
			return BTStatus.Success;
		}

		return BTStatus.Running;
	}
}