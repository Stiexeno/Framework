using System;
using UnityEngine;

public class BTWait : BTLeaf
{
	public float duration;

	[NonSerialized] private float timelpased;
	
	protected override void OnEnter(BTParams btParams)
	{
		timelpased = 0;
	}

	protected override BTStatus OnUpdate(BTParams btParams)
	{
		timelpased += Time.deltaTime;
		
		if (timelpased >= duration)
		{
			return BTStatus.Success;
		}

		return BTStatus.Running;
	}
}