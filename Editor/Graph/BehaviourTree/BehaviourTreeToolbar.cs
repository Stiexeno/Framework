using Framework.Graph.BT;

namespace Framework.Editor.Graph.BT
{
	public class BehaviourTreeToolbar : GraphToolbar
	{
		private BehaviourTree behaviourTree;

		public BehaviourTreeToolbar(BehaviourTree behaviourTree)
		{
			this.behaviourTree = behaviourTree;
		}

		protected override void OnGUI()
		{
		}
	}
}

