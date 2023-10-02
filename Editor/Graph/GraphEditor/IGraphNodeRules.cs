using System;
using System.Collections.Generic;

namespace Framework.Editor.Graph
{
	public interface IGraphNodeRules
	{
		Dictionary<Type, NodeProperties> FetchGraphBehaviours();
	}
}
