using UnityEditor;
using UnityEngine;
using SF = UnityEngine.SerializeField;

namespace Framework
{
	public abstract class GraphContext : IGUIElement
	{
		protected readonly GenericMenu contextMenu = new GenericMenu();

		private bool isInitialized = false;

		protected abstract void Initialize(GraphWindow window);

		public void OnGUI(EditorWindow window, Rect rect)
		{
			if (isInitialized == false)
			{
				var editor = window as GraphWindow;
				Initialize(editor);
				isInitialized = true;
			}
			
			if (GraphInput.IsContextAction(Event.current))
			{
				contextMenu.ShowAsContext();
			}
		}
	}
}