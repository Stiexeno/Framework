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
	private Vector2 position;

	private string searchQuerry = "";

	private GraphEditor editor;
	
	// Properties
	
	public bool IsActive { get; set; }
	
	public GraphSearchMenu Menu { get; } = new GraphSearchMenu();

	public GraphSearch(GraphEditor editor)
	{
		this.editor = editor;
		
		editor.Input.MouseDown += OnMouseDown;
	}

	private void OnMouseDown(object sender, GraphInputEvent e)
	{
		if (rect.Contains(Event.current.mousePosition) == false)
		{
			IsActive = false;
		}
	}

	public void Open(Vector2 mousePosition, Rect rect)
	{
		var clampedRect = new Rect(mousePosition, new Vector2(350, 380));
		clampedRect = clampedRect.ClampToRect(rect, 5);
		this.position = new Vector2(clampedRect.x, clampedRect.y);
		
		IsActive = true;
		searchQuerry = "";
	}

	public void Close()
	{
		IsActive = false;
	}
	
	public Rect Draw()
	{
		if (IsActive == false)
			return Rect.zero;

		PollInput();

		rect = new Rect(position, new Vector2(400, 420));
		EditorGUI.DrawRect(rect, new Color(0.1f, 0.1f, 0.1f));
		EditorHelper.DrawBorderRect(rect, new Color(0.5f, 0.5f, 0.5f), 1f);

		var fieldRect = new Rect(rect.x + 5, rect.y + 5, rect.width - 10, 25);
		
		GUI.SetNextControlName("SearchField");
		searchQuerry = EditorGUI.TextField(fieldRect, searchQuerry);
		EditorGUI.FocusTextInControl("SearchField");
		
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

			if (menuItem.isHeader)
			{
				EditorGUI.LabelField(buttonRect, menuItem.name, GraphStyle.SearchHeader);
			}
			else
			{
				if (GUI.Button(buttonRect, menuItem.name, GraphStyle.SearchItem))
				{
					menuItem.action?.Invoke();
				}	
			}

			buttonRect.y += 15;
		}
	}
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