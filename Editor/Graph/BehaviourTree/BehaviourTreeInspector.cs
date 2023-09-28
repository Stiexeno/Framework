using Framework;
using Framework.Editor;
using UnityEditor;
using UnityEngine;
using SF = UnityEngine.SerializeField;

public class BehaviourTreeInspector : IGUIView
{
	private BTNode node;
	private BehaviourTree tree;

	private float inspectorWidth = 200f;
	private bool isResizing;

	private bool initialized;

	private Rect inspectorRect;
	private Vector2 mouseDragStartPosition;
    
	private Rect rect;
	private GraphEditor editor;

	protected Color BackgroundColor { get; } = new Color(0.2f, 0.2f, 0.2f);

	public BehaviourTreeInspector(BehaviourTree tree, GraphEditor editor)
	{
		this.editor = editor;
		this.tree = tree;
	}

	public void OnEnable()
	{
		Selection.selectionChanged -= SelectionChanged;
		Selection.selectionChanged += SelectionChanged;
	}

	private void SelectionChanged()
	{
		if (Selection.activeObject != null && Selection.activeObject is BTNode btNode)
		{
			node = btNode;
		}
	}

	public void OnGUI(EditorWindow window, Rect rect)
	{
		this.rect = rect;
		if (initialized == false)
		{
			OnEnable();
			initialized = true;
		}

		inspectorRect = rect;
		//inspectorRect.width = inspectorWidth;
		//inspectorRect.y += GUIWindow.ToolbarHeight;
		//inspectorRect.height -= GUIWindow.ToolbarHeight;
//
		//EditorGUI.DrawRect(inspectorRect, BackgroundColor);

		//UpdateDragging(window, rect);
		//DrawContent(inspectorRect);
		DrawToolbar();
		DrawZoom();
	}

	private void DrawContent(Rect rect)
	{
		var header = new Rect(rect.x, rect.y, rect.width - 1f, 30);

		EditorGUI.DrawRect(header, new Color(0.24f, 0.24f, 0.24f));
		EditorGUI.LabelField(header, $"{tree.name}");

		EditorHelper.DrawHorizontalLine(header.SetY(30));

		if (node != null)
		{
			var contentRect = rect;
			GUILayout.BeginArea(contentRect.AddX(15).AddWidth(-20).AddY(40));
			var serialziedObjhect = new SerializedObject(node);
			serialziedObjhect.DrawInspectorExcept("m_Script");
			GUILayout.EndArea();
		}
	}

	private void DrawToolbar()
	{
		var toolbarRect = rect.SetHeight(20f);
		EditorGUI.DrawRect(toolbarRect, new Color(0.1f, 0.1f, 0.1f));

		DrawButtons();
	}

	private void DrawButtons()
	{
		var toolbarRect = rect.SetHeight(35f).AddY(20f);
		EditorHelper.DrawHorizontalLine(toolbarRect, color: new Color(0.25f, 0.25f, 0.25f));

		toolbarRect = toolbarRect.AddY(1f);
		EditorGUI.DrawRect(toolbarRect, new Color(0.1f, 0.1f, 0.1f));

		GUILayout.BeginArea(toolbarRect.AddY(2.5f));

		GUILayout.BeginHorizontal();
		if (GUILayout.Button("+", GUILayout.Width(30f), GUILayout.Height(30f)))
		{
		}

		if (GUILayout.Button("+", GUILayout.Width(30f), GUILayout.Height(30f)))
		{
		}

		if (GUILayout.Button("+", GUILayout.Width(30f), GUILayout.Height(30f)))
		{
		}

		if (GUILayout.Button("+", GUILayout.Width(30f), GUILayout.Height(30f)))
		{
		}

		GUILayout.EndHorizontal();
		GUILayout.EndArea();
	}

	private void DrawZoom()
	{
		var zoomRect = rect;
		zoomRect = zoomRect.SetWidth(100f).SetHeight(20f).AddX(rect.width - 80f).SetY(rect.height - 30f);

		var zoom = 100 - (editor.Viewer.zoom.x - GraphPreferences.Instance.minZoom) /
			(GraphPreferences.Instance.maxZoom - GraphPreferences.Instance.minZoom) * 100;

		EditorGUI.LabelField(zoomRect, $"Zoom: {Mathf.Round(zoom)}%");
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