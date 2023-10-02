using UnityEngine;

namespace Framework.Editor.Graph.BT
{
	public class ParallelGraphNode : CompositeGraphNode
	{
		public override string Header => "Parallel";
		public override Texture2D Icon => BehaviourTreePreferences.Instance.parallelcon;
	}
}
