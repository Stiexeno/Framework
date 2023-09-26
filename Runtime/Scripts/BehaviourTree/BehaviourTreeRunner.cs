using System;
using UnityEngine;
using UnityEngine.Serialization;

public class BehaviourTreeRunner : MonoBehaviour
{
	[FormerlySerializedAs("tree")] public BehaviourTree BehaviourTree;

	public void Start()
	{
		BehaviourTree = ScriptableObject.CreateInstance<BehaviourTree>();
		BehaviourTree.root = ScriptableObject.CreateInstance<DebugLogNode>();
	}

	private void Update()
	{
		BehaviourTree.Update();
	}
}