using Framework.Editor;
using UnityEditor;
using UnityEngine;

namespace Framework
{
	public abstract class GraphInspector : IGUIView
	{
		private float inspectorWidth = 200f;
		private bool isResizing;

		private bool initialized;
		
		private Rect inspectorRect;
		private Vector2 mouseDragStartPosition;
		
		protected EditorWindow window;
		
		protected virtual Color BackgroundColor { get; } = new Color(0.22f, 0.22f, 0.22f);
		protected virtual Vector2 Padding { get; } = new Vector2(10, 10);

		public void OnGUI(EditorWindow window, Rect rect)
		{
			if (initialized == false)
			{
				this.window = window;
				OnEnable();
				initialized = true;
			}
			
			inspectorRect = rect;
			inspectorRect.width = inspectorWidth;
			inspectorRect.y += GUIWindow.ToolbarHeight;
			inspectorRect.height -= GUIWindow.ToolbarHeight;

			EditorGUI.DrawRect(inspectorRect, BackgroundColor);
			
			UpdateDragging(window, rect);
			//Resize(ref inspectorRect);
			
			//GUILayout.BeginArea(inspectorRect);
			OnGUI(inspectorRect);
			//GUILayout.EndArea();
		}

		protected abstract void OnGUI(Rect rect);

		protected virtual void OnEnable()
		{
		}

		private void Resize(ref Rect rect)
		{
			rect.x += Padding.x;
			rect.y += Padding.y;
		}
		
		private void UpdateDragging(EditorWindow window, Rect rect)
		{
			var lineRect = inspectorRect;
			lineRect.width = 1f;
			lineRect.x += inspectorRect.width - 1f;
			EditorGUI.DrawRect(lineRect, new Color(0.14f, 0.14f, 0.14f));

			var handleRect = inspectorRect;
			handleRect.width = 5f;
			handleRect.x += inspectorRect.width - 5f;

			EditorGUIUtility.AddCursorRect(handleRect, MouseCursor.ResizeHorizontal);

			if (Event.current.type == EventType.MouseDown && handleRect.Contains(Event.current.mousePosition))
			{
				isResizing = true;
				mouseDragStartPosition = Event.current.mousePosition;
			}
			else if (Event.current.type == EventType.MouseUp || Event.current.type == EventType.Ignore)
			{
				isResizing = false;
			}

			if (isResizing)
			{
				float deltaX = Event.current.mousePosition.x - mouseDragStartPosition.x;
				inspectorWidth += deltaX;
				inspectorWidth = Mathf.Clamp(inspectorWidth, 100f, rect.width);

				mouseDragStartPosition = Event.current.mousePosition;

				window.Repaint();
			}
		}
	}
}