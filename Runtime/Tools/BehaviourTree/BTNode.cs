using System;
using Framework;
using Framework.Utils;
using UnityEngine;

public enum BTStatus { Inactive, Success, Failure, Running}
public enum NodeType { Composite, Decorator, Leaf, Root }

public abstract class BTNode : GraphBehaviour
{
	protected BTStatus status = BTStatus.Inactive;
	
	public abstract NodeType NodeType { get; }
	public abstract int MaxChildCount { get; }

	/// <summary>
	/// Called every tick while this Node is part of the current sub-tree.
	/// </summary>
	/// <param name="btParams"></param>
	/// <returns></returns>
	protected abstract BTStatus OnUpdate(BTParams btParams);

	/// <summary>
	/// Called once, for every node, when the tree is first created.
	/// </summary>
	/// <param name="agent"></param>
	/// <param name="blackboard"></param>
	public virtual void Init(BTAgent agent, Blackboard blackboard)
	{
		
	}

	/// <summary>
	/// Called whenever the execution of this Node is interrupted
	/// </summary>
	/// <param name="btParams"></param>
	protected virtual void OnEnter(BTParams btParams)
	{
	}
	
	/// <summary>
	/// Called when traversing up the tree after node is already finished with its job.
	/// </summary>
	/// <param name="btParams"></param>
	public virtual void OnExit(BTParams btParams)
	{
	}
	
	/// <summary>
	/// Called when getting out of a sub-branch and this node is being discarded
	/// </summary>
	/// <param name="btParams"></param>
	public virtual void OnReset(BTParams btParams)
	{
		status = BTStatus.Inactive;
	}

	/// <summary>
	/// Used by Decorators to evaluate if a condition succeeds or not.
	/// </summary>
	/// <param name="btParams"></param>
	/// <returns></returns>
	public virtual bool DryRun(BTParams btParams)
	{
		return false;
	}

	public BTStatus RunUpdate(BTParams btParams)
	{
		if (status == BTStatus.Success || status == BTStatus.Failure)
			return status;

		Debug.LogError(status);
		if (status == BTStatus.Inactive)
		{
			OnEnter(btParams);
		}

		BTStatus newStatus;

		try
		{
			newStatus = OnUpdate(btParams);
		}
		catch (Exception e)
		{
			throw new Exception($"{"<b>[BT]</b>".SetColor(new Color(0.36f, 0.64f, 0.87f))} {e.Message} \n{e.StackTrace.SetColor(new Color(0.98f, 0.69f, 0.16f))}");
		}
		
		status = newStatus;

		return newStatus;
	}
}