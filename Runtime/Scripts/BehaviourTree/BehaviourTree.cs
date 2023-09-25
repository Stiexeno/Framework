using System;
using System.Collections.Generic;
using Framework;
using UnityEditor;
using UnityEngine;
using SF = UnityEngine.SerializeField;

[CreateAssetMenu(menuName = "Framework/BehaviourTree/BehaviourTree")]
public class BehaviourTree : GraphBehaviour
{
	public Node root;
	public State state = State.Running;
	
	public List<Node> nodes = new List<Node>();

	public RootNode Root
	{
		get
		{
			if (root == null)
			{
				root = CreateNode(typeof(RootNode)) as RootNode;
			}

			return root as RootNode;
		}
	}
	
	public State Update()
	{
		if (root.state == State.Running)
		{
			state = root.Update();
		}
		
		return state;
	}

	public Node CreateNode(Type type)
	{
		var node = CreateInstance(type) as Node;
		node.name = type.Name;
		node.guid = Guid.NewGuid().ToString();
		nodes.Add(node);
		
		AssetDatabase.AddObjectToAsset(node, this);
		AssetDatabase.SaveAssets();
		return node;
	}

	public void DeleteNode(Node node)
	{
		nodes.Remove(node);
		AssetDatabase.RemoveObjectFromAsset(node);
		AssetDatabase.SaveAssets();
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
	
	public List<Node> GetChildren(Node parent)
	{
		var children = new List<Node>();
		
		if (parent is DecoratorNode decoratorNode)
		{
			if (decoratorNode.child != null)
			{
				children.Add(decoratorNode.child);
			}
		}
		
		if (parent is RootNode rootNode)
		{
			if (rootNode.child != null)
			{
				children.Add(rootNode.child);
			}
		}
		
		if (parent is CompositeNode compositeNode)
		{
			return compositeNode.children;
		}
		
		return children;
	}
}
