using System.Linq;
using Framework.Utils;
using UnityEngine;

namespace Framework.Graph.BT
{
	[CreateAssetMenu(menuName = "Framework/BehaviourTree/BehaviourTree")]
    public class BehaviourTree : GraphTree
    {
    	public BTNode root;
    	
    	public static T GetInstanceVersion<T>(BehaviourTree tree, BTNode original) where T : BTNode
    	{
    		return GetInstanceVersion(tree, original) as T;
    	}
    
    	public static BTNode GetInstanceVersion(BehaviourTree tree, BTNode original)
    	{
    		int index = original.preOrderIndex;
    		return tree.nodes[index] as BTNode;
    	}
    
    	public BehaviourTree Clone()
    	{
    		var clone = this.Clone<BehaviourTree>();
    
    		clone.SetNodes(nodes.Select(node => node.Clone()));
    		
    		for (var i = 0; i < nodes.Count; i++)
    		{
    			var node = nodes[i];
    			clone.nodes[i] = node.Clone();
    
    			if (i == 0)
    			{
    				clone.root = clone.nodes[i] as BTNode;
    			}
    		}
    
    		return clone;
    	}
    	
    	public static BehaviourTree Clone(BehaviourTree blueprint)
    	{
    		var clone = CreateInstance<BehaviourTree>();
    		clone.name = blueprint.name;
    		
    		// Source tree nodes should already be in pre-order.
    		clone.SetNodes(blueprint.nodes.Select(Instantiate));
    
    		// Relink children and parents for the cloned nodes.
    		int maxCloneNodeCount = clone.nodes.Count;
    		for (int i = 0; i < maxCloneNodeCount; ++i)
    		{
    			BTNode nodeSource = blueprint.nodes[i] as BTNode;
    			BTNode copyNode = GetInstanceVersion(clone, nodeSource);
    
    			if (copyNode.NodeType == NodeType.Composite)
    			{
    				var copyComposite = copyNode as BTComposite;
    				copyComposite.SetChildren(
    					Enumerable.Range(0, nodeSource.ChildCount())
    						.Select(childIndex => GetInstanceVersion(clone, nodeSource.GetChildAt(childIndex) as BTNode))
    						.ToArray());
    			}
    
    			else if ((copyNode.NodeType == NodeType.Decorator || copyNode.NodeType == NodeType.Root) && nodeSource.ChildCount() == 1)
    			{
    				var copyDecorator = copyNode as BTDecorator;
    				copyDecorator.SetChild(GetInstanceVersion(clone, nodeSource.GetChildAt(0) as BTNode));
    			}
    		}
    		
    		clone.root = clone.nodes[0] as BTNode;
    
    		return clone;
    	}
    }
}

