using UnityEngine;

public class BTWait : BTLeaf
{
	public float duration;

	private float timelpased;
	
	public float Timelasped => timelpased;
	
	protected override void OnEnter(BTParams btParams)
	{
		timelpased = 0;
	}
	
	public override void OnExit(BTParams btParams)
	{
		base.OnExit(btParams);
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