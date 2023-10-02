using System.Collections.Generic;
using Framework.Graph;
using UnityEngine;

namespace Framework.Editor.Graph
{
	public class GraphNode : IIterableNode<GraphNode>, IGUIElement
	{
		private readonly List<GraphNode> children = new List<GraphNode>();
		public IReadOnlyList<GraphNode> Children => children;

		// Private fields

		private Rect rectPosition;
		private Rect contentRect;

		private GraphBehaviour behaviour;

		// Properties

		public GraphNode Parent { get; private set; }
		public Rect RectPosition => rectPosition;
		public Rect ContentRect => contentRect;

		public Vector2 DynamicSize { get; set; }
		public Vector2 Position
		{
			get => rectPosition.position;
			set => rectPosition.position = value;
		}

		public virtual Vector2 Size { get; protected set; } = new Vector2(100, 100);
		public virtual Color Outline { get; protected set; } = Color.clear;

		public bool HasOutput { get; set; }

		public Vector2 Center
		{
			get => rectPosition.center;
			set
			{
				rectPosition.center = value;
				behaviour.nodePosition = value;
			}
		}
		
		public bool Selected { get; set; }

		public GUIStyle HeaderStyle { get; } = CreateHeaderStyle();
		public GUIStyle BodyStyle { get; } = CreateBodyStyle();
		public GUIContent HeaderContent { get; } = new GUIContent();
		public GUIContent BodyContent { get; } = new GUIContent();
		
		public string Name => behaviour.name;

		public GraphBehaviour Behaviour
		{
			get => behaviour;
			set
			{
				behaviour = value;
				UpdateGUI();
			}
		}

		public Rect InputRect
		{
			get
			{
				float x = rectPosition.x;
				float y = rectPosition.y - 2 - GraphPreferences.Instance.portHeight;
				float width = rectPosition.width;
				float height = GraphPreferences.Instance.portHeight;
				return new Rect(x, y, width, height);
			}
		}
		
		public Rect OutputRect
		{
			get
			{
				float x = rectPosition.x;
				float y = rectPosition.y + rectPosition.height + 2;
				float width = rectPosition.width;
				float height = GraphPreferences.Instance.portHeight;
				return new Rect(x, y, width, height);
			}
		}

		public GraphNode()
		{
		}

		// Virtual methods

		public virtual void OnGUI(Rect rect)
		{
			
		}

		public virtual void OnInputConnect()
		{
			
		}

		public virtual void OnOutputConnect()
		{
			
		}

		public void Remove()
		{
			SetParent(null);
			OrphanChildren();
			Object.DestroyImmediate(behaviour, true);
		}

		private void OrphanChildren()
		{
			foreach (var child in children)
			{
				child.Parent = null;
			}
			
			children.Clear();
		}
		
		public void SortChildren()
		{
			children.Sort((left, right) => left.Center.x.CompareTo(right.Center.x));
		}
		
		public void UpdateGUI()
		{
			HeaderContent.text = HeaderText();
			BodyContent.text = "Null";
			ResizeToFitContent();
		}

		public bool IsParentless()
		{
			return Parent == null;
		}
		
		public void SetParent(GraphNode newParent)
		{
			// Remove from previous parent.
			if (Parent != null)
			{
				Parent.children.Remove(this);
			}

			if (newParent != null)
			{
				if (newParent.Behaviour.MaxChildCount == 1)
				{
					newParent.OrphanChildren();
				}
				
				newParent.children.Add(this);
			}

			// Set new parent
			Parent = newParent;
		}

		public GraphNode GetChildAt(int index)
		{
			return children.Count != 0 ? children[index] : null;
		}

		public int ChildCount()
		{
			return children.Count;
		}
		
		public float GetNearestInputY()
		{
			float nearestY = float.MaxValue;
			float nearestDist = float.MaxValue;

			foreach (GraphNode child in children)
			{
				Vector2 childPosition = child.RectPosition.position;
				Vector2 toChild = childPosition - Position;

				float yDist = Mathf.Abs(toChild.y);

				if (yDist < nearestDist)
				{
					nearestDist = yDist;
					nearestY = childPosition.y;
				}
			}

			return nearestY;
		}

		public void GetBoundsX(out float minX, out float maxX)
		{
			minX = Center.x;
			maxX = Center.x;

			foreach (GraphNode child in children)
			{
				float x = child.Center.x;

				if (x < minX)
				{
					minX = x;
				}

				else if (x > maxX)
				{
					maxX = x;
				}
			}
		}
		
		private void ResizeToFitContent()
		{
			var prefs = GraphPreferences.Instance;

			float portHeights = 2f * prefs.portHeight;
			Vector2 contentSize = MinimumRequiredContentSize();
            
			//rectPosition.size = contentSize + 2f * prefs.nodeSizePadding + Vector2.right * (2f * prefs.nodeWidthPadding) + Vector2.up * portHeights;

			rectPosition.size = DynamicSize;
			contentRect.width = rectPosition.width - 2f * prefs.nodeWidthPadding;
			contentRect.height = rectPosition.height - portHeights;
			contentRect.x = prefs.nodeWidthPadding;
			contentRect.y = 0;

			// Place content relative to the content rect.
			Vector2 contentOffset = contentRect.position + prefs.nodeSizePadding;
			HeaderStyle.contentOffset = GraphMathUtility.Round(contentOffset);
			BodyStyle.contentOffset = GraphMathUtility.Round(contentOffset);

			// Round for UI Sharpness.
			contentRect = GraphMathUtility.Round(contentRect);
			rectPosition = GraphMathUtility.Round(rectPosition);
		}

		private Vector2 MinimumRequiredContentSize()
		{
			Vector2 headerSize = HeaderContentSize();
			Vector2 bodySize = BodyContentSize();
			float maxContentWidth = Mathf.Max(headerSize.x, bodySize.x);
			float totalContentHeight = headerSize.y + bodySize.y;
			return new Vector2(maxContentWidth, totalContentHeight);
		}

		private Vector2 HeaderContentSize()
		{
			float iconSize = GraphPreferences.Instance.iconSize;
			Vector2 size = HeaderStyle.CalcSize(new GUIContent(HeaderText()));
			return new Vector2(size.x + iconSize, Mathf.Max(size.y, iconSize));
		}

		private Vector2 BodyContentSize()
		{
			return BodyStyle.CalcSize(BodyContent);
		}

		private string HeaderText()
		{
			string text = behaviour.ToString();

			if (string.IsNullOrEmpty(text))
			{
				text = "Empty";
			}

			return text;
		}

		private static GUIStyle CreateHeaderStyle()
		{
			var style = new GUIStyle();
			style.normal.textColor = Color.white;
			style.fontSize = 15;
			style.fontStyle = FontStyle.Bold;
			style.imagePosition = ImagePosition.ImageLeft;
			return style;
		}

		private static GUIStyle CreateBodyStyle()
		{
			var style = new GUIStyle();
			style.normal.textColor = Color.white;
			return style;
		}
	}
}