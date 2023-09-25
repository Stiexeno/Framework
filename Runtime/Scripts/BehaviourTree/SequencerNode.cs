using System;

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
		
		return currentChild == children.Count ? State.Success : State.Running;
	}

	protected override void OnStop()
	{
	}
}