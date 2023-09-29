using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Framework
{
	public abstract class GraphWindow : GUIWindow
	{
		// Private fields

		private Rect searchRect;
		
		// Properties
		
		public GraphViewer Viewer { get; private set; }
		public GraphEditor Editor { get; private set; }
		public GraphSaver Saver { get; private set; }
		public GraphSearch Search { get; private set; }
		
		public abstract GraphTree Tree { get; set; }
		public abstract IGraphNodeRules Rules { get; }
		
		private CanvasTransform CanvasTransform =>
			new CanvasTransform
			{
				pan = Viewer.panOffset,
				zoom = Viewer.ZoomScale,
				size = position.size
			};
		
		private GraphSaver.TreeMetaData TreeMetaData
		{
			get
			{
				return new GraphSaver.TreeMetaData
				{
					zoom = Viewer.zoom,
					pan = Viewer.panOffset
				};
			}
		}

		// GraphEditor

		protected override void OnGUI()
		{
			if (Tree == null)
			{
				Viewer.DrawStaticGrid(position.size);
			}
			else
			{
				if (Editor.Canvas == null)
				{
					BuildCanvas();
				}
				
				Editor.PollInput(Event.current, CanvasTransform, CanvasInputRect);
				Editor.UpdateView();
				Viewer.Draw(CanvasTransform);
				searchRect = Search.Draw();
			}
            
			base.OnGUI();
			Repaint();
		}

		protected abstract void Construct(HashSet<IGUIElement> graphElements);
        
		protected static T Open<T>(GraphTree behaviour) where T : GraphWindow
		{
			if (behaviour == null)
				return null;
			
			var windows = Resources.FindObjectsOfTypeAll<T>();
			
			T window = windows.FirstOrDefault();

			if (window == null)
			{
				window = CreateInstance<T>();
				window.Show();
			}
			
			window.SetTree(behaviour);
			return window;
		}

		protected virtual void Initialize(GraphTree root)
		{
		}

		protected override void OnEnable()
		{
			GraphEditor.FetchGraphBehaviours(Rules);

			Saver = new GraphSaver();
			Editor = new GraphEditor();
			Viewer = new GraphViewer();
			Search = new GraphSearch(Editor);
			
			Editor.Viewer = Viewer;
			Editor.Search = Search;
			Editor.CanvasTransform = CanvasTransform;

			Editor.OnCanvasChanged += Repaint;
			Editor.Input.SaveRequest += Save;
			Saver.SaveMessage += (sender, message) => ShowNotification(new GUIContent(message), 0.5f);
			Editor.Input.OnKeySpace += OpenSearch;
			
			EditorApplication.playModeStateChanged += PlayModeStateChanged;
			AssemblyReloadEvents.beforeAssemblyReload += BeforeAssemblyReload;
			Selection.selectionChanged += SelectionChanged;
			
			Construct(graphElements);
		}

		private void OpenSearch(object sender, EventArgs e)
		{
			//Viewer.CustomOverlayDraw += DrawSearch;
			Search.Show(Event.current.mousePosition);
		}

		protected virtual void OnDisable()
		{
			EditorApplication.playModeStateChanged -= PlayModeStateChanged;
			AssemblyReloadEvents.beforeAssemblyReload -= BeforeAssemblyReload;
			Selection.selectionChanged -= SelectionChanged;
			Editor.Input.OnKeySpace -= OpenSearch;
		}
		
		private void OnDestroy()
		{
			OnExit();
		}
		
		public void Load()
		{
			QuickSave();

			var tree = Saver.LoadGraphTree();

			if (tree)
			{
				SetTree(tree);
			}
		}
		
		public void SetTree(GraphTree graphTree)
		{
			Tree = graphTree;
			BuildCanvas();
			
			Initialize(graphTree);
			Construct(graphElements);
		}
		
		private void BuildCanvas()
		{
			if (Tree)
			{
				Editor.SetGraphTree(Tree);
				Repaint();
			}
		}

		private void Save(object sender, EventArgs eventArgs)
		{
			if (Editor.Canvas != null)
			{
				Saver.SaveCanvas(Editor.Canvas, TreeMetaData);
			}
		}
		
		private void QuickSave()
		{
			if (Saver.CanSaveTree(Tree))
			{
				Saver.SaveCanvas(Editor.Canvas, TreeMetaData);
			}
		}
		
		private void SelectionChanged()
		{
		}

		private void BeforeAssemblyReload()
		{
			if (!EditorApplication.isPlayingOrWillChangePlaymode)
			{
				OnExit();
			}
		}

		private void PlayModeStateChanged(PlayModeStateChange state)
		{
			if (state == PlayModeStateChange.ExitingEditMode)
			{
				QuickSave();
			}
		}
		
		private void OnExit()
		{
			Editor.NodeSelection.ClearSelection();
			QuickSave();
		}
		
		private void NicifyTree()
		{
			if (Tree && Editor.Canvas != null)
			{
				if (Editor.Canvas.Root == null)
				{
					ShowNotification(new GUIContent("Set a root to nicely format the tree!"));
				}
				else
				{
					GraphFormatter.PositionNodesNicely(Editor.Canvas.Root, Vector2.zero);
				}
			}
		}
	}
}