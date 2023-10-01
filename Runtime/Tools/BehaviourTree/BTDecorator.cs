using Framework;

public abstract class BTDecorator : BTNode
{
	public BTNode child;
	
	public override NodeType NodeType => NodeType.Decorator;
	public override int MaxChildCount => 1;

	protected override BTStatus OnUpdate(BTParams btParams)
	{
		if (DryRun(btParams))
		{
			if (child == null)
				return BTStatus.Success;
			
			var childStatus = child.RunUpdate(btParams);
			return childStatus;
		}

		return BTStatus.Failure;
	}

	public abstract bool DryRun(BTParams btParams);
	
	public override void OnReset(BTParams btParams)
	{
		base.OnReset(btParams);
		
		OnExit(btParams);

		if (child != null)
		{
			child.OnReset(btParams);
		}
	}
	
	public void SetChild(BTNode btNode)
	{
		child = btNode;
		if (child != null)
		{
			child.Parent = this;
			child.indexOrder = 0;
		}
	}
	
	public override GraphBehaviour GetChildAt(int index)
	{
		return child;
	}

	public override int ChildCount()
	{
		return child != null ? 1 : 0;
	}
}
