using System;
using System.Collections.Generic;

public interface IGraphNodeRules
{
	Dictionary<Type, NodeProperties> FetchGraphBehaviours();
}