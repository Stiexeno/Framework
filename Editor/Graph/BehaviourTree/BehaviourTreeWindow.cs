using System;
using System.Collections.Generic;
using Framework;
using Framework.Editor;
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
	
	protected override void OnEnable()
	{
		base.OnEnable();
		
		graphElements.Add(new BehaviourTreeInspector(this));
		PopulateSearch();
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
        
		BehaviourTreePreferences.Instance.SaveGraph(AssetDatabase.GetAssetPath(behaviourTree));
		
		
	}

	private void PopulateSearch()
	{
		var menu = Editor.Search.Menu;
		menu.AddHeader("Behaviour Tree");
		menu.AddItem("Sequencer", () => RequestCreateNode(typeof(BTSequence)));
		menu.AddItem("Selector", () => RequestCreateNode(typeof(BTSelector)));
		menu.AddItem("Wait", () => RequestCreateNode(typeof(BTWait)));
		menu.AddHeader("Leaf");

		foreach (var behaviour in GraphEditor.Behaviours)
		{
			var nodeType = behaviour.Key;

			if (nodeType.IsSubclassOf(typeof(BTLeaf)))
			{
				if (behaviour.Key == typeof(BTWait) ||
				behaviour.Key == typeof(BTLog))
					continue;
				
				menu.AddItem($"{behaviour.Key}", () => RequestCreateNode(nodeType));
			}
		}
	}
	
	private void RequestCreateNode(Type nodeType)
	{
		Editor.CreateNodeFromType(nodeType);
		Editor.Search.Close();
	}

	[OnOpenAsset]
	public static bool OpenAsset(int instanceId, int line)
	{
		var root = EditorUtility.InstanceIDToObject(instanceId) as GraphTree;

		if (root == null)
			return false;

		BehaviourTreeWindow behaviourWindow = Open<BehaviourTreeWindow>(root);
		behaviourWindow.titleContent = new GUIContent("Behaviour Tree");
		
		if (behaviourWindow != null)
		{
			return true;
		}
//
		return false;
	}
}
