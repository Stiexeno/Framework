using UnityEngine;
using SF = UnityEngine.SerializeField;

public abstract class DecoratorNode : Node
{
	public Node child;
	
	public void SetChild(Node node)
	{
		child = node;
		if (child != null)
		{
			child.Parent = this;
			child.indexOrder = 0;
		}
	}
}
