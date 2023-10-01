public class BTRoot : BTDecorator
{
	public override NodeType NodeType => NodeType.Root;
    
	public override bool DryRun(BTParams btParams)
	{
		return true;
	}
}