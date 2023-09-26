using Framework;

public class ActionNode : Node
{
	protected override void OnStart()
	{
	}

	protected override State OnUpdate()
	{
		return State.Failure;
	}

	protected override void OnStop()
	{
	}

	public override int MaxChildCount()
	{
		return 0;
	}

	public override int ChildCount()
	{
		return 0;
	}

	public override GraphBehaviour GetChildAt(int index)
	{
		return null;
	}
}