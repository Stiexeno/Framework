using Framework;

public class RootNode : DecoratorNode
{
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

	public override int MaxChildCount()
	{
		return 1;
	}

	public override int ChildCount()
	{
		return child != null ? 1 : 0;
	}

	public override GraphBehaviour GetChildAt(int index)
	{
		return child;
	}
}