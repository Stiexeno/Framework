using UnityEngine;

public class Wait : BTLeaf
{
	public float duration;

	private float startTime;
	
	protected override void OnEnter(BTParams btParams)
	{
		startTime = Time.time;
	}

	protected override BTStatus OnUpdate(BTParams btParams)
	{
		if (Time.time - startTime >= duration)
		{
			return BTStatus.Success;
		}

		return BTStatus.Running;
	}
}