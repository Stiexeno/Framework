using System;
using Framework;

public abstract class BTComposite : BTNode
{
	public BTNode[] children = Array.Empty<BTNode>();
	
	public int currentChildIndex = 0;
	
	public override NodeType NodeType => NodeType.Composite;
	public override int MaxChildCount => int.MaxValue;

	protected override void OnEnter(BTParams btParams)
	{
		SetCurrentChild(0);
	}
	
	public override void OnReset(BTParams btParams)
	{
		base.OnReset(btParams);
		
		OnExit(btParams);

		for (int i = 0; i < children.Length; i++)
		{
			children[i].OnReset(btParams);
		}
	}

	public void SetChildren(BTNode[] nodes)
	{
		children = nodes;

		for (int i = 0; i < children.Length; i++)
		{
			children[i].indexOrder = i;
		}

		foreach (BTNode child in children)
		{
			child.Parent = this;
		}
	}

	protected int GetCurrentChild()
	{
		return currentChildIndex;	
	}

	protected void SetCurrentChild(int index)
	{
		currentChildIndex = index;
	}
	
	public override int ChildCount()
	{
		return children.Length;
	}

	public override GraphBehaviour GetChildAt(int index)
	{
		return children[index];
	}

	public virtual void ChildCompletedRunning(BTParams btParams, BTStatus result)
	{
	}
}