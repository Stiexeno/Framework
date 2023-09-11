using Framework.Editor;
using UnityEditor;
using UnityEngine;
using SF = UnityEngine.SerializeField;

namespace Framework
{
	public class IngridientNode : GraphNode
	{
		private CraftingIngridient ingridient;
		
		public override Vector2 Size => new Vector2(150, 175);
		
		public IngridientNode(CraftingIngridient ingridient) : base(ingridient)
		{
			this.ingridient = ingridient;
			GraphInput.DoubleClick += OpenResourceBrowser;
		}
		
		private void OpenResourceBrowser(object sender, GraphInputEvent e)
		{
			if (GraphInput.IsUnderMouse(e.transform, RectPosition))
			{
				ContentBrowser.Open<ResourceBrowser>((resource) => { ingridient.resource = resource as ResourceBaseConfig; }, true);
			}
		}

		public override void OnGUI(Rect rect)
		{
			var selectRect = rect;
			selectRect.height = 140f;
			selectRect.y += 5f;
			selectRect.width -= 10f;
			selectRect.x += 5f;

			DrawIcon(selectRect);
			
			if (ingridient.resource != null)
			{
				EditorGUI.DropShadowLabel(selectRect.SetHeight(25), $"{ingridient.resource.Name}", BattlepassStyle.middleH0Label);
			}
			
			EditorGUI.DrawRect(rect.SetHeight(1).AddY(selectRect.height + 1), new Color(0.16f, 0.16f, 0.16f));
			var fieldRect = selectRect.SetHeight(20).AddY(selectRect.height + 5);
			fieldRect.width -= 30f;
			fieldRect.x += 30f;
			
			GUI.Label(fieldRect.SetWidth(30).AddX(-30), FenrirEditor.CreateNamedIcon("", "Import-Available@2x"));
			ingridient.count = EditorGUI.IntField(fieldRect, ingridient.count);
		}
		
		private void DrawIcon(Rect rect)
		{
			if (ingridient.resource == null)
			{
				GUI.Label(rect, FenrirEditor.CreateNamedIcon("", "Toolbar Plus@2x"), BattlepassStyle.middleH0Label);
				return;
			}

			var resourceIcon = AssetPreview.GetAssetPreview(ingridient.resource.Icon);
			GUI.Label(rect, resourceIcon);
		}
	}
}