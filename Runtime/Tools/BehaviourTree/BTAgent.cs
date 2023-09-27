using UnityEngine;

public class BTAgent : MonoBehaviour
{
	public BehaviourTree behaviourTree;

	private BTParams btParams = new BTParams();
	
	public void Start()
	{
		behaviourTree = ScriptableObject.CreateInstance<BehaviourTree>();
		behaviourTree.root = ScriptableObject.CreateInstance<DebugLog>();
	}
	
	private void Update()
	{
		var rootNode = behaviourTree.root;
		UpdateSubtree(btParams, rootNode);
	}

	private void UpdateSubtree(BTParams btParams, BTNode node)
	{
		var result = node.RunUpdate(btParams);

		if (result != BTStatus.Running && node.Parent != null)
		{
			node.OnExit(btParams);
			
			node = node.Parent as BTNode;

			if (node.NodeType == NodeType.Composite)
			{
				((BTComposite)node).ChildCompletedRunning(btParams, result);
				result = node.RunUpdate(btParams);
			}

			return;
		}

		if (result != BTStatus.Running)
		{
			node.OnReset(btParams);
		}
	}
}