using System.Collections.Generic;

namespace Framework
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