using System;
using Framework;
using Framework.Utils;
using UnityEngine;

public enum BTStatus { Inactive, Success, Failure, Running}
public enum NodeType { Composite, Decorator, Leaf, Root }

public abstract class BTNode : GraphBehaviour
{
	public BTStatus status = BTStatus.Inactive;
	private BTAgent agent;

	public abstract NodeType NodeType { get; }
	public BTStatus Status => status;
	public BTAgent Agent => agent;

	/// <summary>
	/// Called every tick while this Node is part of the current sub-tree.
	/// </summary>
	/// <returns></returns>
	protected abstract BTStatus OnUpdate();

	/// <summary>
	/// Called once, for every node, when the tree is first created.
	/// </summary>
	/// <param name="agent"></param>
	/// <param name="blackboard"></param>
	public virtual void Init(BTAgent agent, Blackboard blackboard)
	{
		this.agent = agent;
	}

	/// <summary>
	/// Called whenever the execution of this Node is interrupted
	/// </summary>
	protected virtual void OnEnter()
	{
	}
	
	/// <summary>
	/// Called when traversing up the tree after node is already finished with its job.
	/// </summary>
	public virtual void OnExit()
	{
		#if UNITY_EDITOR
		if (status != BTStatus.Failure)
		{
			EditorStatus = BTEditorStatus.Inactive;
		}
		#endif
	}
	
	/// <summary>
	/// Called when getting out of a sub-branch and this node is being discarded
	/// </summary>
	public virtual void OnReset()
	{
		status = BTStatus.Inactive;
	}

	/// <summary>
	/// Used by Decorators to evaluate if a condition succeeds or not.
	/// </summary>
	/// <returns></returns>
	public virtual bool DryRun()
	{
		return false;
	}

	public BTStatus RunUpdate()
	{
		//if (status == BTStatus.Success || status == BTStatus.Failure)
		//	return status;
        
		if (status == BTStatus.Inactive)
		{
			OnEnter();
		}

		BTStatus newStatus;

		try
		{
			newStatus = OnUpdate();
		}
		catch (Exception e)
		{
			throw new Exception($"{e.Message} \n{e.StackTrace.SetColor(new Color(0.98f, 0.69f, 0.16f))}");
		}
		
		status = newStatus;
		
		#if UNITY_EDITOR
		EditorStatus = (BTEditorStatus) newStatus;
		#endif
		
		return newStatus;
	}
	
	#if UNITY_EDITOR
	
	public enum BTEditorStatus { Inactive, Success, Failure, Running}
	
	public BTEditorStatus EditorStatus  = BTEditorStatus.Inactive;
	
	#endif
}