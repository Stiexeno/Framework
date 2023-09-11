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
		
		public abstract GraphBehaviour Root { get; set; }

		// Properties

		private CanvasTransform CanvasTransform =>
			new CanvasTransform
			{
				pan = Viewer.panOffset,
				zoom = Viewer.ZoomScale,
				size = position.size
			};

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
		protected abstract List<GraphNode> GatherBehaviours();
		protected abstract GenericMenu RegisterContextMenu();

		private void Save(object sender, EventArgs eventArgs)
		{
			Saver.SaveCanvas(Editor.Canvas);
		}

		protected static T Open<T>(GraphBehaviour behaviour) where T : GraphWindow
		{
			var windows = Resources.FindObjectsOfTypeAll<T>();
			
			T window = windows.FirstOrDefault();

			if (window == null)
			{
				window = CreateInstance<T>();
				window.Show();
			}
			
			window.Initialize(behaviour);

			return window;
		}

		protected virtual void Initialize(GraphBehaviour root)
		{
			Root = root;
			BuildCanvas();
		}

		private void BuildCanvas()
		{
			var nodes = GatherBehaviours();
			Editor.BuildCanvas(nodes);
			Viewer.Canvas = Editor.Canvas;
		}

		protected override void OnEnable()
		{
			Construct(graphElements);

			Saver = new GraphSaver();
			Editor = new GraphEditor(
				RegisterContextMenu()
			);

			Viewer = new GraphViewer();
			Editor.Viewer = Viewer;
			Editor.CanvasTransform = CanvasTransform;

			Editor.OnCanvasChanged += Repaint;
			Editor.Input.SaveRequest += Save;
			Saver.SaveMessage += (sender, message) => ShowNotification(new GUIContent(message), 0.5f);
		}
	}
}