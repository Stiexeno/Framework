using Framework;

public abstract class BTLeaf : BTNode
{
	public override NodeType NodeType => NodeType.Leaf;
	public override int MaxChildCount => 0;

	public override int ChildCount() => 0;
	public override GraphBehaviour GetChildAt(int index) => null;
	
	public override void OnReset()
	{
		base.OnReset();
		OnExit();
	}
}