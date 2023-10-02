namespace Framework.Graph.BT
{
	public class BTRoot : BTDecorator
	{
		public override NodeType NodeType => NodeType.Root;

		protected override bool DryRun()
		{
			return true;
		}
	}
}