using System;
using Framework;

public class SequencerNode : CompositeNode
{
	private int currentChild = 0;
	
	protected override void OnStart()
	{
		currentChild = 0;
	}

	protected override State OnUpdate()
	{
		var child = children[currentChild];

		switch (child.Update())
		{
			case State.Success:
				currentChild++;
				break;
			case State.Failure:
				return State.Failure;
			case State.Running:
				return State.Running;
		}
		
		return currentChild == children.Length ? State.Success : State.Running;
	}

	protected override void OnStop()
	{
	}

	public override int MaxChildCount()
	{
		return int.MaxValue;
	}

	public override int ChildCount()
	{
		return children.Length;
	}

	public override GraphBehaviour GetChildAt(int index)
	{
		return children[index];
	}
}