using System;
using System.Collections.Generic;
using System.Linq;
using Framework;

public abstract class GraphNodeRules<T> : IGraphNodeRules where T: GraphBehaviour
{
	protected abstract NodeProperties GatherNodeRules(Type behaviour);

	public Dictionary<Type, NodeProperties> FetchGraphBehaviours()
	{
		var behaviourNodes = new Dictionary<Type, NodeProperties>();
		
		var behaviourTypes = AppDomain.CurrentDomain.GetAssemblies()
			.Where(assembly => !assembly.FullName.StartsWith("Unity") && !assembly.FullName.StartsWith("Editor"))
			.SelectMany(assembly => assembly.GetTypes())
			.Where(t => t.IsSubclassOf(typeof(T)) && !t.IsAbstract);

		foreach (var behaviourType in behaviourTypes)
		{
			behaviourNodes.Add(behaviourType, GatherNodeRules(behaviourType));
		}
        
		return behaviourNodes;
	}
}

public class NodeProperties
{
	public Type nodeType;
	public bool hasOutput;
	
	public NodeProperties(Type nodeType, bool hasOutput)
	{
		this.nodeType = nodeType;
		this.hasOutput = hasOutput;
	}
}