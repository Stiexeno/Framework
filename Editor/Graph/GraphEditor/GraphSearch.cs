using System;
using System.Collections.Generic;
using Framework;
using Framework.Editor;
using UnityEditor;
using UnityEngine;
using SF = UnityEngine.SerializeField;

public class GraphSearch
{
	private Rect rect;
	private Vector2 mousePosition;

	private string searchQuerry = "";

	private GraphEditor editor;
	
	// Properties
	
	public bool IsActive { get; set; }
	
	public GraphSearchMenu Menu { get; } = new GraphSearchMenu();

	public GraphSearch(GraphEditor editor)
	{
		this.editor = editor;
		
		editor.Input.MouseDown += OnMouseDown;
		
		Menu.AddItem("WaitNode", null);
		Menu.AddItem("WaitNode2", null);
		Menu.AddItem("WaitNode3", null);
	}

	private void OnMouseDown(object sender, GraphInputEvent e)
	{
		if (rect.Contains(Event.current.mousePosition) == false)
		{
			IsActive = false;
		}
	}

	public void Show(Vector2 mousePosition)
	{
		this.mousePosition = mousePosition;
		IsActive = true;
		
		searchQuerry = "";
	}
	
	public Rect Draw()
	{
		if (IsActive == false)
			return Rect.zero;

		PollInput();

		rect = new Rect(mousePosition, new Vector2(400, 420));
		EditorGUI.DrawRect(rect, new Color(0.1f, 0.1f, 0.1f));
		EditorHelper.DrawBorderRect(rect, new Color(0.5f, 0.5f, 0.5f), 1f);

		var fieldRect = new Rect(rect.x + 5, rect.y + 5, rect.width - 10, 25);
		
		searchQuerry = EditorGUI.TextField(fieldRect, searchQuerry);

		DrawContent(rect);
		return rect;
	}

	private void PollInput()
	{
		if (GraphInput.IsExitAction(Event.current))		
			IsActive = false;
		
		if (rect.Contains(Event.current.mousePosition))
			return;
		
		if (GraphInput.IsClickAction(Event.current))		
			IsActive = false;
	}

	private void DrawContent(Rect rect)
	{
		var buttonRect = new Rect(rect.x + 5, rect.y + 35, rect.width - 10, 15);
		
		for (var i = 0; i < Menu.menuItems.Count; i++)
		{
			var menuItem = Menu.menuItems[i];
			
			if (GUI.Button(buttonRect, menuItem.name, EditorStyles.label))
			{
				menuItem.action?.Invoke();
			}
			
			buttonRect.y += 15;
		}
	}
	
	//private GenericMenu CreateSingleSelectionContextMenu(GraphNode node)
	//{
	//	var menu = new GenericMenu();
	//	menu.AddItem(new GUIContent("Format Subtree"), false, OnNodeAction, NodeContext.FormatTree);
	//	menu.AddSeparator("");
	//	menu.AddItem(new GUIContent("Delete"), false, OnNodeAction, NodeContext.Delete);
	//	return menu;
	//}
}

public class GraphSearchMenu
{
	public List<GraphMenuItem> menuItems = new List<GraphMenuItem>();
	
	public void AddItem(string name, Action action) 
	{
		menuItems.Add(new GraphMenuItem
		{
			name = name,
			action = action,
			isHeader = false,
			isSeparator = false
		});
	}

	public void AddSeparator(string name)
	{
		menuItems.Add(new GraphMenuItem
		{
			name = name,
			action = null,
			isHeader = false,
			isSeparator = true
		});
	}
	
	public void AddHeader(string name)
	{
		menuItems.Add(new GraphMenuItem
		{
			name = name,
			action = null,
			isHeader = true,
			isSeparator = false
		});
	}

	public class GraphMenuItem
	{
		public string name;
		public Action action;
		public bool isHeader;
		public bool isSeparator;
	}
}