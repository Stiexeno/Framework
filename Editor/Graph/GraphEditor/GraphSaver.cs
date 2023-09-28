using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Framework
{
	using FilePanelResult = Framework.Graph.Result<FilePanelError, string>;
	public enum FilePanelError { Cancel, InvalidPath }

	public class GraphSaver
	{
		public struct TreeMetaData
		{
			public Vector2 zoom;
			public Vector2 pan;
		}

		public event EventHandler<string> SaveMessage;

		public bool CanSaveTree(GraphTree tree)
		{
			return tree && AssetDatabase.Contains(tree);
		}

		public GraphTree LoadGraphTree()
		{
			var path = GetCanvasOpenFilePath();

			if (path.Success)
			{
				var tree = LoadGraphTree(path.Value);

				if (tree == null)
				{
					OnLoadFailure();
				}
				else
				{
					OnLoadSuccess();
				}

				return tree;
			}

			OnInvalidPathError(path.Error);
			return null;
		}

		public void SaveCanvas(GraphCanvas canvas, TreeMetaData treeMetaData)
		{
			if (AssetDatabase.Contains(canvas.Tree) == false)
			{
				GetSaveFilePath()
					.OnSuccess(savePath =>
					{
						SaveNewTree(savePath, treeMetaData, canvas);
					})
					.OnFailure(OnInvalidPathError);
			}
			else
			{
				SaveTree(treeMetaData, canvas);
				OnTreeSaved();
			}
		}
		
		private void SaveNewTree(string path, TreeMetaData meta, GraphCanvas canvas)
		{
			// Save tree and black board assets
			AssetDatabase.CreateAsset(canvas.Tree, path);
			AssetDatabase.AddObjectToAsset(canvas.Tree.blackboard, canvas.Tree);

			// Save nodes.
			SaveTree(meta, canvas);
		}

		private void SaveTree(TreeMetaData meta, GraphCanvas canvas)
		{
			AddBlackboardIfMissing(canvas.Tree);

			var canvasBehaviours = canvas.Nodes.Select(node => node.Behaviour);

			AddNodeAsset(canvas.Tree, canvasBehaviours);

			canvas.Tree.ClearStructure();

			foreach (var node in canvas.Nodes.Where(node => node.ChildCount() > 1))
			{
				node.SortChildren();
			}
			
			SetCompositeChildren(canvas);
			SetDecoratorChildren(canvas);
            
			if (canvas.Root != null)
			{
				canvas.Tree.SetNodes(canvas.Root.Behaviour);
			}

			canvas.Tree.unusedNodes = canvasBehaviours.Where(b => b.PreOrderIndex == GraphBehaviour.kInvalidOrder).ToList();
			
			SaveTreeMetaData(meta, canvas);
			AssetDatabase.SaveAssets();
		}
		
		private void SetCompositeChildren(GraphCanvas canvas)
		{
			IEnumerable<GraphNode> compositeNodes = canvas.Nodes.Where(n => n.Behaviour is BTComposite);
			foreach (GraphNode node in compositeNodes)
			{
				var compositeBehaviour = node.Behaviour as BTComposite;
				compositeBehaviour.SetChildren(node.Children.Select(ch => ch.Behaviour).Cast<BTNode>().ToArray());
			}
		}

		private void SetDecoratorChildren(GraphCanvas canvas)
		{
			IEnumerable<GraphNode> decoratorNodes = canvas.Nodes
				.Where(n => n.Behaviour is BTDecorator && n.ChildCount() == 1);

			foreach (GraphNode node in decoratorNodes)
			{
				var decoratorBehaviour = node.Behaviour as BTDecorator;
				decoratorBehaviour.SetChild(node.GetChildAt(0).Behaviour as BTNode);
			}
		}
		
		private void AddNodeAsset(GraphTree tree, IEnumerable<GraphBehaviour> behaviours)
		{
			foreach (var node in behaviours)
			{
				if (!AssetDatabase.Contains(node))
				{
					node.name = node.GetType().Name;
					//node.hideFlags = HideFlags.HideInHierarchy;
					AssetDatabase.AddObjectToAsset(node, tree);
				}
			}
		}
		
		public static void AddBlackboardIfMissing(GraphTree tree)
		{
			if (tree && (tree.blackboard == null || !AssetDatabase.Contains(tree.blackboard)))
			{
				if (tree.blackboard == null)
				{
					tree.blackboard = CreateBlackboard();
				}

				AssetDatabase.AddObjectToAsset(tree.blackboard, tree);
			}
		}
		
		private static Blackboard CreateBlackboard()
		{
			var bb = ScriptableObject.CreateInstance<Blackboard>();
			bb.hideFlags = HideFlags.HideInHierarchy;
			return bb;
		}
        
		private void SaveTreeMetaData(TreeMetaData meta, GraphCanvas canvas)
		{
			foreach (var editorNode in canvas.Nodes)
			{
				editorNode.Behaviour.nodePosition = editorNode.Position;
			}

			canvas.Tree.panPosition = meta.pan;
			canvas.Tree.zoomPosition = meta.zoom;
		}
		
		private static BehaviourTree LoadGraphTree(string absolutePath)
		{
			string path = AssetPath(absolutePath);
			var tree = AssetDatabase.LoadAssetAtPath<BehaviourTree>(path);
			return tree;
		}

		private FilePanelResult GetCanvasOpenFilePath()
		{
			string path = EditorUtility.OpenFilePanel("Open Graph", "Assets/", "asset");

			if (string.IsNullOrEmpty(path))
			{
				return FilePanelResult.Fail(FilePanelError.Cancel);
			}
            
			if (!path.Contains(Application.dataPath))
			{
				return FilePanelResult.Fail(FilePanelError.InvalidPath);
			}


			return FilePanelResult.Ok(path);
		}
		
		private FilePanelResult GetSaveFilePath()
		{
			string path = EditorUtility.SaveFilePanelInProject("Save Graph Canvas", "NewGraphTree", "asset", "Select a destination to save the canvas.");

			if (string.IsNullOrEmpty(path))
			{
				return FilePanelResult.Fail(FilePanelError.Cancel);
			}

			return FilePanelResult.Ok(path);
		}
		
		private static string AssetPath(string absolutePath)
		{
			int assetIndex = absolutePath.IndexOf("/Assets/", StringComparison.Ordinal);
			return absolutePath.Substring(assetIndex + 1);
		}
		
		private void OnInvalidPathError(FilePanelError pathError)
		{
			if (pathError == FilePanelError.InvalidPath)
			{
				SaveMessage?.Invoke(this, "Please select a GraphTree asset within the project's Asset folder.");
			}
		}

		private void OnLoadSuccess()
		{
			SaveMessage?.Invoke(this, "Tree loaded");
		}

		private void OnLoadFailure()
		{
			SaveMessage?.Invoke(this, "Failed to load tree.");
		}
		
		private void OnTreeSaved()
		{
			SaveMessage?.Invoke(this, "Tree saved");
		}
	}
}