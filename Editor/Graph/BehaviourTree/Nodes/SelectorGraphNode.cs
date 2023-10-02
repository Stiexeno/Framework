using Framework.Graph.BT;
using UnityEngine;

namespace Framework.Editor.Graph.BT
{
	public class SelectorGraphNode : CompositeGraphNode
	{
		private BTSequence btSequence;

		public override string Header => "Selector";
		public override Texture2D Icon => BehaviourTreePreferences.Instance.selectorIcon;
	}
}