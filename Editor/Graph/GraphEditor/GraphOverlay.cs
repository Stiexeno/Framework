using UnityEditor;
using UnityEngine;
using SF = UnityEngine.SerializeField;

namespace Framework
{
	public class GraphOverlay : IGUIView
	{
		public void OnGUI(EditorWindow window, Rect rect)
		{
			var overlayRect = rect;
			overlayRect.width = 200;
			overlayRect.height = 200;
			
			overlayRect.x += rect.width - overlayRect.width - 5;
			overlayRect.y += rect.height - overlayRect.height - 5;
			
			GUI.DrawTexture(
				overlayRect, GraphPreferences.Instance.defaultNodeBackground,
				ScaleMode.StretchToFill,
				true,
				0,
				new Color(0.08f, 0.07f, 0.07f),
				0,
				6f);
		}
	}
}