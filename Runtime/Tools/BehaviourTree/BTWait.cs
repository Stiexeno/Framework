using UnityEngine;

public class BTWait : BTLeaf
{
	public float duration;

	private float timelpased;
	
	public float Timelasped => timelpased;
	
	protected override void OnEnter()
	{
		timelpased = 0;
	}
	
	public override void OnExit()
	{
		base.OnExit();
		timelpased = 0;
	}

	protected override BTStatus OnUpdate()
	{
		timelpased += Time.deltaTime;
		
		if (timelpased >= duration)
		{
			return BTStatus.Success;
		}
        
		return BTStatus.Running;
	}
}