public class BTSelector : BTComposite
{
	protected override BTStatus OnUpdate(BTParams btParams)
	{
		var currentStatus = BTStatus.Success;

		if (GetCurrentChild() < children.Length)
		{
			var child = children[GetCurrentChild()];
			currentStatus = child.RunUpdate(btParams);

			if (currentStatus == BTStatus.Failure)
			{
				return BTStatus.Failure;
			}
			
			if (currentStatus == BTStatus.Success)
			{
				SetCurrentChild(GetCurrentChild() + 1);
			}
		}

		return currentStatus;
	}
	
	public override void ChildCompletedRunning(BTParams btParams, BTStatus result)
	{
		if (result == BTStatus.Failure)
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