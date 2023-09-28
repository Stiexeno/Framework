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

		public GraphViewer Viewer { get; private set; }
		public GraphEditor Editor { get; private set; }
		public GraphSaver Saver { get; private set; }
		
		public abstract GraphTree Tree { get; set; }
		public abstract IGraphNodeRules Rules { get; }

		// Properties

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
			Viewer.Draw(CanvasTransform);
			Editor.PollInput(Event.current, CanvasTransform, CanvasInputRect);
			Editor.UpdateView();

			base.OnGUI();
			Repaint();
		}

		protected abstract void Construct(HashSet<IGUIElement> graphElements);
		protected abstract GenericMenu RegisterContextMenu();
        
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
			
			Construct(graphElements);

			Saver = new GraphSaver();
			Editor = new GraphEditor();
			Viewer = new GraphViewer();
			
			Editor.Viewer = Viewer;
			Editor.CanvasTransform = CanvasTransform;

			Editor.OnCanvasChanged += Repaint;
			Editor.Input.SaveRequest += Save;
			Saver.SaveMessage += (sender, message) => ShowNotification(new GUIContent(message), 0.5f);
			
			EditorApplication.playModeStateChanged += PlayModeStateChanged;
			AssemblyReloadEvents.beforeAssemblyReload += BeforeAssemblyReload;
			Selection.selectionChanged += SelectionChanged;
		}

		protected virtual void OnDisable()
		{
			EditorApplication.playModeStateChanged -= PlayModeStateChanged;
			AssemblyReloadEvents.beforeAssemblyReload -= BeforeAssemblyReload;
			Selection.selectionChanged -= SelectionChanged;
		}
		
		void OnDestroy()
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