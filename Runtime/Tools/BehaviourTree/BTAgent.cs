using UnityEngine;

namespace Framework.Graph.BT
{
	public class BTAgent : MonoBehaviour
    {
    	public BehaviourTree behaviourTree;
    
    	public BehaviourTree treeInstance;
    	
    	public BehaviourTree Tree => treeInstance;
    
    	private void Awake()
    	{
    		treeInstance = BehaviourTree.Clone(behaviourTree);
    		Initialize();
    	}
    
    	protected virtual void Initialize()
    	{
    		foreach (var graphBehaviour in treeInstance.nodes)
    		{
    			var node = (BTNode)graphBehaviour;
    			node.Init(this, null);
    		}
    	}
    
    	private void Update()
    	{
    		var rootNode = treeInstance.root;
    		UpdateSubtree(rootNode);
    	}
    
    	private void UpdateSubtree(BTNode node)
    	{
    		var result = node.RunUpdate();
    		
    		#if UNITY_EDITOR
    		node.EditorStatus = (BTNode.BTEditorStatus) result;
    		#endif
    		
    		if (result == BTStatus.Success || result == BTStatus.Failure)
    		{
    			node.OnReset();
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
}
