using System.Collections.Generic;

public abstract class CompositeNode : Node
{
	public List<Node> children = new List<Node>();
}