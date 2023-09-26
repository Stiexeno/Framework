using System;
using Framework;
using UnityEditor;
using UnityEngine;
using SF = UnityEngine.SerializeField;

[CreateAssetMenu(menuName = "Framework/BehaviourTree/BehaviourTree")]
public class BehaviourTree : GraphTree
{
	public Node root;
	public State state = State.Running;
	
	public State Update()
	{
		if (root.state == State.Running)
		{
			state = root.Update();
		}
		
		return state;
	}

	public void AddChild(Node parent, Node child)
	{
		if (parent is DecoratorNode decoratorNode)
		{
			decoratorNode.child = child;
		}
		
		if (parent is CompositeNode compositeNode)
		{
			compositeNode.children.Add(child);
		}
		
		if (parent is RootNode rootNode)
		{
			rootNode.child = child;
		}
	}
	
	public void RemoveChild(Node parent, Node child)
	{
		if (parent is DecoratorNode decoratorNode)
		{
			decoratorNode.child = null;
		}
		
		if (parent is CompositeNode compositeNode)
		{
			compositeNode.children.Remove(child);
		}
		
		if (parent is RootNode rootNode)
		{
			rootNode.child = null;
		}
	}
	
	public Node[] GetChildren(Node parent)
	{
		//var children = new List<Node>();
		//
		//if (parent is DecoratorNode decoratorNode)
		//{
		//	if (decoratorNode.child != null)
		//	{
		//		children.Add(decoratorNode.child);
		//	}
		//}
		//
		//if (parent is RootNode rootNode)
		//{
		//	if (rootNode.child != null)
		//	{
		//		children.Add(rootNode.child);
		//	}
		//}
		//
		//if (parent is CompositeNode compositeNode)
		//{
		//	return compositeNode.children;
		//}
		
		return null;
	}
}
