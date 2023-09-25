public class RootNode : Node
{
	public Node child;
	
	protected override void OnStart()
	{
	}

	protected override State OnUpdate()
	{
		return child.Update();
	}

	protected override void OnStop()
	{
	}
}