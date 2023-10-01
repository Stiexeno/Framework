using System;
using UnityEngine;

public class BTWait : BTLeaf
{
	public float duration;

	public float timelpased;
	
	protected override void OnEnter(BTParams btParams)
	{
		timelpased = 0;
	}

	protected override BTStatus OnUpdate(BTParams btParams)
	{
		timelpased += Time.deltaTime;
		
		if (timelpased >= duration)
		{
			Debug.LogError("Success");
			return BTStatus.Success;
		}

		Debug.LogError("Running");
		return BTStatus.Running;
	}
}