using System;
using UnityEngine;

public class BTAgent : MonoBehaviour
{
	public BehaviourTree behaviourTree;

	private BTParams btParams = new BTParams();

	public BehaviourTree treeInstance;
	
	public BehaviourTree Tree => treeInstance;

	private void Awake()
	{
		treeInstance = BehaviourTree.Clone(behaviourTree);
		
		foreach (var graphBehaviour in treeInstance.nodes)
		{
			var node = (BTNode)graphBehaviour;
			node.Init(this, null);
		}
	}

	private void Update()
	{
		var rootNode = treeInstance.root;
		UpdateSubtree(btParams, rootNode);
	}

	private void UpdateSubtree(BTParams btParams, BTNode node)
	{
		var result = node.RunUpdate(btParams);
		node.EditorStatus = (BTNode.BTEditorStatus) result;
		
		if (result == BTStatus.Success || result == BTStatus.Failure)
		{
			node.OnReset(btParams);
		}

		//if (result != BTStatus.Running)
		//{
		//	node.OnExit(btParams);
//
		//	node = node;
//
		//	if (node.NodeType == NodeType.Composite)
		//	{
		//		((BTComposite)node).ChildCompletedRunning(btParams, result);
		//		result = node.RunUpdate(btParams);
		//	}
        //    
		//	node.OnReset(btParams);
		//}
	}
}