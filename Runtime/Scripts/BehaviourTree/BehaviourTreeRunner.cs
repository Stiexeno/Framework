using System;
using UnityEngine;

public class BehaviourTreeRunner : MonoBehaviour
{
	public BehaviourTree tree;

	public void Start()
	{
		tree = ScriptableObject.CreateInstance<BehaviourTree>();
		tree.root = ScriptableObject.CreateInstance<DebugLogNode>();
	}

	private void Update()
	{
		tree.Update();
	}
}