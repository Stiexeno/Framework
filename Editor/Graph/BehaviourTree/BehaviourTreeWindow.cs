using System;
using System.Collections.Generic;
using Framework;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

public class BehaviourTreeWindow : GraphWindow
{
	// Private fields
	
	private static BehaviourTreeWindow window;
	private static BehaviourTree behaviourTree;
	
	//BehaviourTreeWindow
	
	public override GraphBehaviour Root { get; set; }

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

	protected override void Initialize(GraphBehaviour behaviour)
	{
		behaviourTree = behaviour as BehaviourTree;
		base.Initialize(behaviour);
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

	protected override List<GraphNode> GatherBehaviours()
	{
		if (Root == null)
			return new List<GraphNode>();

		List<GraphNode> nodes = new List<GraphNode>
		{
			new RootGraphNode(behaviourTree.Root)
		};

		return nodes;
	}

	protected override GenericMenu RegisterContextMenu()
	{
		var menu = new GenericMenu();
		menu.AddItem(new GUIContent("Create/Sequencer"), false, () => CreateNode(typeof(SequencerNode)));
		menu.AddItem(new GUIContent("Create/ActionNode"), false, () => CreateNode(typeof(ActionNode)));

		return menu;
	}

	private void CreateNode(Type type)
	{
		var node = behaviourTree.CreateNode(type);

		if (node is RootNode root)
		{
			Root = root;
		}

		if (node is SequencerNode sequencerNode)
		{
			Editor.CreateNode(typeof(SequencerGraphNode), sequencerNode);
		}
		
		if (node is ActionNode actionNode)
		{
			Editor.CreateNode(typeof(ActionGraphNode), actionNode);
		}
	}
	
	private void OpenSearch(object sender, EventArgs eventArgs)
	{
		Debug.LogError("@!#?!@?#?!@#?");
	}
    
	[OnOpenAsset]
	public static bool OpenAsset(int instanceId, int line)
	{
		var root = EditorUtility.InstanceIDToObject(instanceId) as BehaviourTree;

		if (root == null)
			return false;

		BehaviourTreeWindow behaviourWindow = Open<BehaviourTreeWindow>(root);
			
		if (behaviourWindow != null)
		{
			behaviourWindow.Initialize(root);
			return true;
		}

		return false;
	}
}
