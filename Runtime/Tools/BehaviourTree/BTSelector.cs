public class BTSelector : BTComposite
{
	protected override BTStatus OnUpdate(BTParams btParams)
	{
		BTStatus currentStatus;
        
		if (GetCurrentChild() < children.Length)
		{
			var child = children[GetCurrentChild()];
			currentStatus = child.RunUpdate(btParams);

			if (currentStatus == BTStatus.Success)
			{
				return BTStatus.Success;
			}
			
			if (currentStatus == BTStatus.Failure)
			{
				SetCurrentChild(GetCurrentChild() + 1);
			}
		}
		else
		{
			return BTStatus.Failure;
		}

		return currentStatus;
	}
	
	public override void ChildCompletedRunning(BTParams btParams, BTStatus result)
	{
		if (result == BTStatus.Failure || result == BTStatus.Success)
		{
			SetCurrentChild(GetCurrentChild() + 1);
		}
		else
		{
			SetCurrentChild(children.Length);
			status = BTStatus.Success;
		}
	}
}