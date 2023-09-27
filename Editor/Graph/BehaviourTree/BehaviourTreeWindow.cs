using System;
using System.Collections.Generic;
using Framework;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

public class BehaviourTreeWindow : GraphWindow
{
	// Private fields
	
	private BehaviourTree behaviourTree;
	
	private static BehaviourTreeWindow window;
	
	//BehaviourTreeWindow
	
	public override GraphTree Tree { get; set; }
	public override IGraphNodeRules Rules { get; } = new BehaviourTreeGraphNodeRules();

	[MenuItem("Framework/Behaviour Tree")]
	private static void Init()
	{
		window = GetWindow<BehaviourTreeWindow>();
		window.titleContent = new GUIContent("Behaviour Tree");
		window.Show();
	}
	
	protected override void Construct(HashSet<IGUIElement> graphElements)
	{
	}

	protected override void Initialize(GraphTree behaviour)
	{
		behaviourTree = behaviour as BehaviourTree;

		if (behaviourTree.root == null)
		{
			var node = Editor.Canvas.CreateNode(typeof(Root));
			behaviourTree.root = Editor.Canvas.Nodes[0].Behaviour as BTNode;
			
			Editor.Canvas.SetRoot(node);
		}
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		Editor.Input.OnKeySpace += OpenSearch;
	}

	private void OnDisable()
	{
		Editor.Input.OnKeySpace -= OpenSearch;
	}

	//protected override List<GraphNode> GatherBehaviours()
	//{
	//	if (Tree == null)
	//		return new List<GraphNode>();
//
	//	List<GraphNode> nodes = new List<GraphNode>
	//	{
	//		new RootGraphNode(behaviourTree.Root)
	//	};
//
	//	return nodes;
	//}

	protected override GenericMenu RegisterContextMenu()
	{
		var menu = new GenericMenu();
		return menu;
	}
	
	private void OpenSearch(object sender, EventArgs eventArgs)
	{
		
	}
    
	[OnOpenAsset]
	public static bool OpenAsset(int instanceId, int line)
	{
		var root = EditorUtility.InstanceIDToObject(instanceId) as GraphTree;

		if (root == null)
			return false;

		BehaviourTreeWindow behaviourWindow = Open<BehaviourTreeWindow>(root);
			
		if (behaviourWindow != null)
		{
			return true;
		}

		return false;
	}
}
