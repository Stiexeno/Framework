using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Framework
{
	public class GraphEditor
	{
		private GraphNode lastCreatedNode;
		
		public GraphViewer Viewer { get; set; }
		public GraphSelection NodeSelection { get; } = new GraphSelection();
		public GraphInput Input { get; } = new GraphInput();
		public GraphCanvas Canvas { get; private set; }
		
		public CanvasTransform CanvasTransform { get; set; }

		public event Action OnCanvasChanged;
		private Action<CanvasTransform> MotionAction;
		private Action<GraphInputEvent> ApplyAction;

		public GraphEditor(GenericMenu registerContextMenu)
		{
			Input.ContextMenu = registerContextMenu;

			Input.selection = NodeSelection;
			Input.MouseDown += BeginOnMouseDown;
			Input.Click += Clicked;
			Input.MouseUp += MouseUp;
		}

		public void BuildCanvas(List<GraphNode> nodes)
		{
			Canvas = new GraphCanvas(nodes);
			Canvas.CanvasTransform = CanvasTransform;
		}

		public void UpdateView()
		{
			Canvas.OnGUI();
		}

		public void CreateNode(Type nodeType, Type behaviourType)
		{
			lastCreatedNode = Canvas.CreateNode(nodeType, behaviourType);
		}
		
		public void CreateNode(Type nodeType, GraphBehaviour behavipur)
		{
			lastCreatedNode = Canvas.CreateNode(nodeType, behavipur);
		}

		public void PollInput(Event e, CanvasTransform canvas, Rect inputRect)
		{
			if (lastCreatedNode != null)
			{
				lastCreatedNode.Center = GraphInput.MousePosition(canvas);
				lastCreatedNode = null;
			}
			
			if (e.type == EventType.MouseDrag)
			{
				if (MotionAction != null)
				{
					MotionAction(canvas);
					OnCanvasChanged();
				}
			}

			if (GraphInput.IsPanAction(e))
			{
				Pan(e.delta);
				OnCanvasChanged();
			}

			if (GraphInput.IsZoomAction(e))
			{
				Zoom(e.delta.y);
				OnCanvasChanged();
			}

			Input.HandleMouseEvents(e, canvas, Canvas.Nodes, inputRect);
		}

		private void Pan(Vector2 delta)
		{
			Viewer.panOffset += delta * Viewer.ZoomScale * GraphViewer.PanSpeed;

			Viewer.panOffset.x = Mathf.Round(Viewer.panOffset.x);
			Viewer.panOffset.y = Mathf.Round(Viewer.panOffset.y);
		}

		private void Zoom(float zoomDirection)
		{
			float scale = (zoomDirection < 0f) ? (1f - GraphViewer.ZoomDelta) : (1f + GraphViewer.ZoomDelta);
			Viewer.zoom *= scale;

			float cap = Mathf.Clamp(Viewer.zoom.x, GraphViewer.MinZoom, GraphViewer.MaxZoom);
			Viewer.zoom.Set(cap, cap);
		}

		private void StartDrag(GraphInputEvent e)
		{
			if (NodeSelection.IsSingleSelection)
			{
				StartSingleDrag(e);
			}
			else if (NodeSelection.IsMultiSelection)
			{
			    StartMultiDrag(e);
			}
		}

		private void StartSingleDrag(GraphInputEvent e)
		{
			GraphNode node = e.node;
			Vector2 offset = GraphSingleDrag.StartDrag(node, e.canvasMousePostion);
			MotionAction = (canvasTransform) => GraphSingleDrag.Drag(node, GraphInput.MousePosition(canvasTransform), offset);
		}
		
		private void StartMultiDrag(GraphInputEvent e)
		{
			var nodes = GraphMultiDrag.StartDrag(NodeSelection.SelectedNodes, e.canvasMousePostion);
			MotionAction = canvasTransform => GraphMultiDrag.Drag(GraphInput.MousePosition(canvasTransform), nodes);
		}

		private void BeginOnMouseDown(object sender, GraphInputEvent inputEvent)
		{
			if (MotionAction != null)
				return;

			if (inputEvent.IsPortFocused())
			{
				StartConnection(inputEvent);
			}
			else if (inputEvent.IsNodeFocused())
			{
				if (NodeSelection.IsNodeSelected(inputEvent.node) == false)
				{
					NodeSelection.SetSingleSelection(inputEvent.node);
				}
				
				StartDrag(inputEvent);
			}
			else
			{
				StartAreaSelection(inputEvent);
			}
		}

		private void Clicked(object sender, GraphInputEvent inputEvent)
		{
			if (Event.current.control && inputEvent.node != null)
			{
				NodeSelection.SetSingleSelection(inputEvent.node);
			}
		}

		private void MouseUp(object sender, GraphInputEvent e)
		{
			ApplyAction?.Invoke(e);
			ClearActions();
		}

		public void ClearActions()
		{
			ApplyAction = null;
			MotionAction = null;
			Viewer.CustomDraw = null;
			Viewer.CustomOverlayDraw = null;
		}

		private void StartAreaSelection(GraphInputEvent startEvent)
		{
			Vector2 startScreenSpace = Event.current.mousePosition;
			Vector2 start = startEvent.canvasMousePostion;

			ApplyAction = applyEvent =>
			{
				Vector2 end = applyEvent.canvasMousePostion;
				var areaSelection = GraphAreaSelect.NodesUnderArea(Canvas.Nodes, start, end);
				NodeSelection.SetMultiSelection(areaSelection.ToList());
			};

			Viewer.CustomOverlayDraw = () =>
			{
				// Construct and display the rect.
				Vector2 endScreenSpace = Event.current.mousePosition;
				Rect selectionRect = GraphAreaSelect.SelectionArea(startScreenSpace, endScreenSpace);
				Color selectionColor = new Color(0f, 0.42f, 0.89f, 0.1f);
				Handles.DrawSolidRectangleWithOutline(selectionRect, selectionColor, new Color(0f, 0f, 1f, 0.61f));
				OnCanvasChanged();
			};
		}

		private void StartConnection(GraphInputEvent startEvent)
		{
			//if (startEvent.node.HasOutput == false)
			//	return;
			
			bool isOutputFocused = startEvent.isOutputFocused;

			GraphNode parent = isOutputFocused ? startEvent.node : GraphConnection.StartConnection(startEvent.node);

			if ((isOutputFocused && startEvent.node.HasOutput == false))
				return;
			
			if (parent != null)
			{
				ApplyAction = (applyEvent) =>
				{
					if (applyEvent.node != null)
					{
						GraphConnection.FinishConnection(Canvas, parent, applyEvent.node);
					}
					else if (isOutputFocused)
					{
					}
				};

				Viewer.CustomDraw = (canvasTransform) =>
				{
					var start = canvasTransform.CanvasToScreenSpace(parent.OutputRect.center);
					start.y -= GraphPreferences.Instance.portHeight;
					var end = Event.current.mousePosition;

					GraphDrawer.DrawRectConnectionScreenSpace(start, end, new Color(0.98f, 0.78f, 0.05f));

					OnCanvasChanged();
				};
			}
		}
	}
}