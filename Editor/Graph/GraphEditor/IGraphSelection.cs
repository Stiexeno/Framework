using System.Collections.Generic;
using Framework.Graph;

namespace Framework.Editor.Graph
{
	public interface IGraphSelection
	{
		IReadOnlyList<GraphNode> SelectedNodes { get; }
		GraphNode SingleSelectedNode { get; }
		IReadOnlyList<GraphBehaviour> Referenced { get; }

		bool IsNodeSelected(GraphNode node);
		bool IsReferenced(GraphNode node);

		int SelectedCount { get; }
		bool IsEmpty { get; }
		bool IsSingleSelection { get; }
		bool IsMultiSelection { get; }
	}
}