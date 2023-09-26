using System;

public abstract class CompositeNode : Node
{
	public Node[] children = Array.Empty<Node>();
	
	public void SetChildren(Node[] nodes)
	{
		children = nodes;

		for (int i = 0; i < children.Length; i++)
		{
			children[i].indexOrder = i;
		}

		foreach (Node child in children)
		{
			child.Parent = this;
		}
	}
}