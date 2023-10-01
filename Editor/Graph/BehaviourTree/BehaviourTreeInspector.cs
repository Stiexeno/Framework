using System.Collections.Generic;
using Framework;
using Framework.Editor;
using UnityEditor;
using UnityEngine;
using SF = UnityEngine.SerializeField;

public class BehaviourTreeInspector : IGUIView
{
	private BTNode node;

	private float inspectorWidth = 200f;
	private bool isResizing;

	private bool initialized;

	private Rect inspectorRect;
	private Vector2 mouseDragStartPosition;
    
	private Rect rect;
	private GraphWindow window;
	
	private List<string> savedGraphs;

	private GUIContent loadIcon = new GUIContent("", BehaviourTreePreferences.Instance.loadIcon);
	private GUIContent saveIcon = new GUIContent("", BehaviourTreePreferences.Instance.saveIcon);
	private GUIContent formatIcon = new GUIContent("", BehaviourTreePreferences.Instance.formatIcon);
	private GUIContent createIcon = new GUIContent("", BehaviourTreePreferences.Instance.createIcon);
	
	protected Color BackgroundColor { get; } = new Color(0.2f, 0.2f, 0.2f);

	public BehaviourTreeInspector(GraphWindow graphWindow)
	{
		this.window = graphWindow;
		
		savedGraphs = BehaviourTreePreferences.Instance.GetSavedGraphs();
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
		DrawTabs();
		DrawNavigation();
		DrawLayers();
		DrawZoom();
	}

	private void DrawContent(Rect rect)
	{
		var header = new Rect(rect.x, rect.y, rect.width - 1f, 30);

		EditorGUI.DrawRect(header, new Color(0.24f, 0.24f, 0.24f));
		//EditorGUI.LabelField(header, $"{tree.name}");

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

	private void DrawTabs()
	{
		var toolbarRect = rect.SetHeight(23f);
		EditorGUI.DrawRect(toolbarRect, new Color(0.1f, 0.1f, 0.1f));
		
		GUILayout.BeginArea(toolbarRect.AddX(5));
		GUILayout.BeginHorizontal();

		for (int i = 0; i < savedGraphs.Count; i++)
		{
			var graphNames = savedGraphs[i].Split("/");
			var graphName = $"{graphNames[^1].Replace(".asset", "")}";
			var style = window.Tree != null && graphName == window.Tree.name ? GraphStyle.ToolbarTabActive : GraphStyle.ToolbarTab;
			if (GUILayout.Button(graphName, style, GUILayout.Height(23f)))
			{
				var graph = AssetDatabase.LoadAssetAtPath<BehaviourTree>(savedGraphs[i]);
				if (graph != null)
				{
					window.SetTree(graph);
				}
			}
            
			if (GUILayout.Button(new GUIContent("", BehaviourTreePreferences.Instance.closeIcon), GraphStyle.IconCenter, GUILayout.Width(15), GUILayout.Height(23)))
			{
				BehaviourTreePreferences.Instance.RemoveSavedGraph(savedGraphs[i]);
			}
			
			GUILayout.Space(5);
		}
		
		GUILayout.FlexibleSpace();
		
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
	}

	private void DrawNavigation()
	{
		const float buttonWidth = 30f;
		
		var toolbarRect = rect.SetHeight(35f).AddY(23f);
		EditorHelper.DrawHorizontalLine(toolbarRect, color: new Color(0.25f, 0.25f, 0.25f));

		toolbarRect = toolbarRect.AddY(1f);
		EditorGUI.DrawRect(toolbarRect, new Color(0.1f, 0.1f, 0.1f));

		GUILayout.BeginArea(toolbarRect.AddY(2.5f).AddX(5));

		GUILayout.BeginHorizontal();
		if (GUILayout.Button(loadIcon, GUILayout.Width(30f), GUILayout.Height(buttonWidth)))
		{
			window.Load();
		}
        
		if (GUILayout.Button(saveIcon, GUILayout.Width(30f), GUILayout.Height(buttonWidth)))
		{
			window.QuickSave();
		}
        
		if (GUILayout.Button(formatIcon, GUILayout.Width(30f), GUILayout.Height(buttonWidth)))
		{
			window.NicifyTree();
		}

		if (GUILayout.Button(createIcon, GUILayout.Width(30f), GUILayout.Height(buttonWidth)))
		{
			window.CreateNew<BehaviourTree>();
		}

		GUILayout.EndHorizontal();
		GUILayout.EndArea();
	}

	private void DrawLayers()
	{
		var toolbarRect = rect.SetHeight(20f).AddY(58f);
		EditorHelper.DrawHorizontalLine(toolbarRect, color: new Color(0.25f, 0.25f, 0.25f));
		
		toolbarRect = toolbarRect.AddY(1f);
		EditorGUI.DrawRect(toolbarRect, new Color(0.1f, 0.1f, 0.1f));
	}

	private void DrawZoom()
	{
		var zoomRect = rect;
		zoomRect = zoomRect.SetWidth(100f).SetHeight(20f).AddX(rect.width - 80f).SetY(rect.height - 30f);

		var zoom = 100 - (window.Viewer.zoom.x - GraphPreferences.Instance.minZoom) /
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