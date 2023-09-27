public class Root : BTDecorator
{
	public override NodeType NodeType => NodeType.Root;
	
	protected override BTStatus OnUpdate(BTParams btParams)
	{
		if (child != null)
		{
			return child.RunUpdate(btParams);
		}

		return BTStatus.Success;
	}
}