using UnityEngine;

public class BTParallel : BTComposite
{
	private BTStatus parallelStatus = BTStatus.Success;
	
	protected override BTStatus OnUpdate()
	{
		if (GetCurrentChild() < children.Length)
		{
			var child = children[GetCurrentChild()];
			var currentStatus = child.RunUpdate();
            
			if (currentStatus == BTStatus.Failure)
			{
				parallelStatus = BTStatus.Failure;
			}
            
			if (currentStatus == BTStatus.Success || currentStatus == BTStatus.Failure)
			{
				SetCurrentChild(GetCurrentChild() + 1);
				return BTStatus.Running;
			}

			return BTStatus.Running;
		}
		
		return parallelStatus;
	}
}