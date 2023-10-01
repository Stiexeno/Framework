using UnityEngine;

public class BTSequence : BTComposite
{
	protected override BTStatus OnUpdate(BTParams btParams)
	{
		var currentStatus = BTStatus.Success;
		
		if (GetCurrentChild() < children.Length)
		{
			var child = children[GetCurrentChild()];
			currentStatus = child.RunUpdate(btParams);
			Debug.LogError(child.name + " " + currentStatus);

			if (currentStatus == BTStatus.Failure)
			{
				return BTStatus.Failure;
			}
			
			if (currentStatus == BTStatus.Success)
			{
				SetCurrentChild(GetCurrentChild() + 1);
				return BTStatus.Running;
			}
		}

		Debug.LogError( currentStatus);
		
		return currentStatus;
	}

	public override void ChildCompletedRunning(BTParams btParams, BTStatus result)
	{
		if (result == BTStatus.Failure)
		{
			SetCurrentChild(children.Length);
			status = BTStatus.Failure;
		}
		else
		{
			SetCurrentChild(GetCurrentChild() + 1);
		}
	}
}