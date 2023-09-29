using Framework.Editor;
using UnityEditor;
using UnityEngine;

namespace Framework
{
	public static class GraphDrawer
	{
		public static void DrawGrid(Rect canvas, Texture texture, float zoom, Vector2 pan)
		{
			const float scale = 1f;

			var size = canvas.size;
			var center = size / 2f;

			var tile = new Vector2(texture.width * scale, texture.height * scale);

			float xOffset = -(center.x * zoom + pan.x) / tile.x;
			float yOffset = ((center.y - size.y) * zoom + pan.y) / tile.y;

			Vector2 tileOffset = new Vector2(xOffset, yOffset);

			float tileAmountX = Mathf.Round(size.x * zoom) / tile.x;
			float tileAmountY = Mathf.Round(size.y * zoom) / tile.y;

			Vector2 tileAmount = new Vector2(tileAmountX, tileAmountY);

			GUI.DrawTextureWithTexCoords(canvas, texture, new Rect(tileOffset, tileAmount));
		}
		
		public static void DrawStaticGrid(Rect canvas, Texture2D texture)
		{
			var size = canvas.size;
			var center = size / 2f;

			float xOffset = -center.x / texture.width;
			float yOffset = (center.y - size.y) / texture.height;

			// Offset from origin in tile units
			Vector2 tileOffset = new Vector2(xOffset, yOffset);

			float tileAmountX = Mathf.Round(size.x) / texture.width;
			float tileAmountY = Mathf.Round(size.y) / texture.height;

			// Amount of tiles
			Vector2 tileAmount = new Vector2(tileAmountX, tileAmountY);

			// Draw tiled background
			GUI.DrawTextureWithTexCoords(canvas, texture, new Rect(tileOffset, tileAmount));
		}

		public static void DrawNode(
			CanvasTransform t,
			GraphNode node,
			Color statusColor)
		{
			Rect screenRect = node.RectPosition;
			screenRect.position = t.CanvasToScreenSpace(screenRect.position);

			Color originalColor = GUI.color;

			DrawNodeGradient(screenRect.AddHeight(6f).AddWidth(6f).AddX(-3f), new Color(0f, 0f, 0f, 0.28f));
			if (node.Selected)
			{
				DrawSelectedOutline(screenRect.AddHeight(4f).AddY(-2f), new Color(0.27f, 0.85f, 1f), t);
			}
			else if (screenRect.Contains(Event.current.mousePosition))
			{
				DrawSelectedOutline(screenRect.AddHeight(4f).AddY(-2f), new Color(0.21f, 0.75f, 0.94f, 0.69f), t);
			}
			
			//DrawOutline(screenRect.AddHeight(6f).AddY(-4f).AddWidth(2), new Color(0.16f, 0.16f, 0.16f));
			DrawOutline(screenRect.AddHeight(2f).AddY(-1f).AddWidth(-2).AddX(1), node.Outline);
			DrawNodeBackground(screenRect, statusColor);
			DrawPorts(t, node);

			GUI.BeginGroup(screenRect);

			Rect localRect = node.RectPosition;
			localRect.position = Vector2.zero;

			GUILayout.BeginArea(localRect, GUIStyle.none);

			node.OnGUI(node.ContentRect);

			GUILayout.EndArea();

			GUI.EndGroup();
			GUI.color = originalColor;
		}

		public static void DrawNodeBackground(Rect screenRect, Color color)
		{
			GUI.DrawTexture(
				screenRect, GraphPreferences.Instance.defaultNodeBackground,
				ScaleMode.StretchToFill,
				true,
				0,
				color,
				0,
				5f);
		}
		
		public static void DrawNodeGradient(Rect screenRect, Color color)
		{
			GUI.DrawTexture(
				screenRect, GraphPreferences.Instance.defaultNodeGadient,
				ScaleMode.StretchToFill,
				true,
				0,
				color,
				0,
				5f);
		}

		public static void DrawOutline(Rect screenRect, Color color)
		{
			screenRect.x -= 1;
			screenRect.y -= 1;
			screenRect.width += 2;
			screenRect.height += 2;

			GUI.DrawTexture(
				screenRect, GraphPreferences.Instance.defaultNodeBackground,
				ScaleMode.StretchToFill,
				true,
				0,
				color,
				0,
				6f);
		}

		public static void DrawSelectedOutline(Rect screenRect, Color color, CanvasTransform canvasTransform)
		{
			var size = Mathf.Clamp(2 * Mathf.Clamp(canvasTransform.zoom, 1f, 2f), 1, float.MaxValue);
			var offset = size * 2;
			screenRect.x -= size;
			screenRect.y -= size;
			screenRect.width += offset;
			screenRect.height += offset;

			GUI.DrawTexture(
				screenRect, GraphPreferences.Instance.defaultNodeBackground,
				ScaleMode.StretchToFill,
				true,
				0,
				color,
				0,
				8f);
		}

		public static void DrawPorts(CanvasTransform t, GraphNode node)
		{
			var input = node.InputRect;
			input.position = t.CanvasToScreenSpace(node.InputRect.position);
			input.y -= 3;
			input.height -= 3;
			
			if (input.Contains(Event.current.mousePosition) && node.IsParentless() == false)
			{
				GUI.DrawTexture(
					input, GraphPreferences.Instance.defaultNodeBackground,
					ScaleMode.StretchToFill,
					true,
					0,
					new Color(0.4f, 0.4f, 0.4f, 0.56f),
					0,
					5f);
				
				GUI.Label(input, $"x", GraphStyle.Header0Center);
			}
			
			var output = node.OutputRect;
			output.position = t.CanvasToScreenSpace(node.OutputRect.position);
			output.y += 3;
			output.height -= 3;
			
			if (output.Contains(Event.current.mousePosition) && node.HasOutput)
			{
				GUI.DrawTexture(
					output, GraphPreferences.Instance.defaultNodeBackground,
					ScaleMode.StretchToFill,
					true,
					0,
					new Color(0.4f, 0.4f, 0.4f, 0.56f),
					0,
					5f);
				
				GUI.Label(output, $"+", GraphStyle.Header0Center);
			}
		}

		// Ports

		public static void DrawRectConnectionScreenSpace(Vector2 start, Vector2 end, Color color)
		{
			var originalColor = Handles.color;
			Handles.color = color;

			float halfDist = (start - end).magnitude / 2f;

			Vector2 directionToEnd = (end - start).normalized;
			Vector2 directionToStart = (start - end).normalized;

			Vector2 axisForTipAlignment = Vector3.up;

			Vector2 startTip = Vector3.Project(directionToEnd, axisForTipAlignment) * halfDist + (Vector3)start;
			Vector2 endTip = Vector3.Project(directionToStart, axisForTipAlignment) * halfDist + (Vector3)end;

			if (startTip == endTip)
			{
				Handles.DrawLine(start, end);
			}

			else
			{
				Handles.DrawAAPolyLine(GraphPreferences.Instance.defaultNodeBackground, 3, start, startTip);
				Handles.DrawAAPolyLine(GraphPreferences.Instance.defaultNodeBackground, 3, end, endTip);
				Handles.DrawAAPolyLine(GraphPreferences.Instance.defaultNodeBackground, 3, startTip, endTip);
			}
			
			Handles.color = originalColor;
		}

		public static void DrawNodeConnections(CanvasTransform t, GraphNode node)
		{
			if (node.ChildCount() == 0)
			{
				return;
			}

			var prefs = GraphPreferences.Instance;

			Color connectionColor = new Color(0.98f, 0.78f, 0.05f);
			float connectionWidth = 3;

			// Start the Y anchor coord at the tip of the Output port.
			float yoffset = node.RectPosition.yMax;

			// Calculate the anchor position.
			float anchorX = node.RectPosition.center.x;
			float anchorY = (yoffset + node.GetNearestInputY()) / 2f;

			// Anchor line, between the first and last child.

			// Find the min and max X coords between the children and the parent.
			node.GetBoundsX(out float anchorLineStartX, out float anchorLineEndX);

			// Get start and end positions of the anchor line (The common line where the parent and children connect).
			var anchorLineStart = new Vector2(anchorLineStartX, anchorY);
			var anchorLineEnd = new Vector2(anchorLineEndX, anchorY);

			// The tip where the parent starts its line to connect to the anchor line.
			var parentAnchorTip = new Vector2(anchorX, yoffset);

			// The point where the parent connects to the anchor line.
			var parentAnchorLineConnection = new Vector2(anchorX, anchorY);

			//foreach (var child in node.Children)
			//{
			//	// Get the positions to draw a line between the node and the anchor line.
			//	Vector2 center = child.InputRect.center;
			//	center.y += GraphPreferences.Instance.portHeight;
			//	var anchorLineConnection = new Vector2(center.x, anchorY);
			//	
			//	if (IsHovered(t, center, anchorLineConnection))
			//	{
			//		connectionColor = new Color(0.98f, 0f, 0.01f);
			//	}
			//}

			//if (IsHovered(t, parentAnchorTip, parentAnchorLineConnection))
			//{
			//	connectionColor = new Color(0.98f, 0f, 0.01f);
			//}
			//
			//if (IsHovered(t, anchorLineStart, anchorLineEnd))
			//{
			//	connectionColor = new Color(0.98f, 0f, 0.01f);
			//}

			// Draw the lines from the calculated positions.
			DrawLineCanvasSpace(
				t,
				parentAnchorTip,
				parentAnchorLineConnection,
				connectionColor,
				connectionWidth);

			DrawLineCanvasSpace(
				t,
				anchorLineStart,
				anchorLineEnd,
				connectionColor,
				connectionWidth);

			foreach (GraphNode child in node.Children)
			{
				// Get the positions to draw a line between the node and the anchor line.
				Vector2 center = child.InputRect.center;
				center.y += GraphPreferences.Instance.portHeight;
				var anchorLineConnection = new Vector2(center.x, anchorY);

				// The node is not running, draw a default connection.
				DrawLineCanvasSpace(
					t,
					anchorLineConnection,
					center,
					connectionColor,
					connectionWidth);
				
				DrawEdgeArrow(t, center);

				if (IsHovered(t, 
					    parentAnchorTip, 
					    parentAnchorLineConnection,
					    new Vector2(parentAnchorLineConnection.x, anchorLineStart.y),
					    new Vector2(anchorLineConnection.x, anchorLineEnd.y),
					    anchorLineConnection,
					    center))
				{
					DrawHoveredConnections(t, 
						parentAnchorTip, 
						parentAnchorLineConnection,
						new Vector2(parentAnchorLineConnection.x, anchorLineStart.y),
						new Vector2(anchorLineConnection.x, anchorLineEnd.y),
						anchorLineConnection,
						center);
				}
			}
		}

		private static void DrawEdgeArrow(CanvasTransform t, Vector2 position) 
		{
			position = t.CanvasToScreenSpace(position);

			position.y -= 37;
			position.x -= 16;
			GUI.DrawTexture(
				position.ToRect(16, 16), GraphPreferences.Instance.edgeArrow,
				ScaleMode.StretchToFill,
				true,
				0,
				new Color(0.98f, 0.78f, 0.05f),
				0,
				0f);
		}

		private static void DrawHoveredConnections(CanvasTransform t, params Vector2[] points)
		{
			for (int i = 0; i < points.Length - 1; i++)
			{
				DrawLineCanvasSpace(t, points[i], points[i + 1], new Color(0.33f, 0.74f, 0.93f), 4);
			}
		}
		
		private static bool IsHovered(CanvasTransform t, params Vector2[] points)
		{
			for (int i = 0; i < points.Length - 1; i++)
			{
				var start = t.CanvasToScreenSpace(points[i]);
				var end = t.CanvasToScreenSpace(points[i + 1]);

				if (HandleUtility.DistanceToLine(start, end) < 5f)
				{
					return true;
				}
			}

			return false;
		}

		public static void DrawLineCanvasSpace(CanvasTransform t, Vector2 start, Vector2 end, Color color, float width)
		{
			start = t.CanvasToScreenSpace(start);
			end = t.CanvasToScreenSpace(end);
			
			if (t.IsScreenAxisLineInView(start, end))
			{
				DrawLineScreenSpace(start, end, color, width);
			}
		}

		public static void DrawLineScreenSpace(Vector2 start, Vector2 end, Color color)
		{
			var originalColor = Handles.color;
			Handles.color = color;
			Handles.DrawLine(start, end);
			Handles.color = originalColor;
		}

		public static void DrawLineScreenSpace(Vector2 start, Vector2 end, Color color, float width)
		{
			var originalColor = Handles.color;
			Handles.color = color;
			Handles.DrawAAPolyLine(GraphPreferences.Instance.defaultNodeBackground, width, start, end);
			Handles.color = originalColor;
		}
	}
}