using UnityEditor;
using UnityEngine;

namespace Framework.Editor
{
	public class RecipeNode : GraphNode
	{
		private CraftingRecipe craftingRecipe;
		private Rect rect;

		public override Vector2 Size => new Vector2(150, 200);

		public RecipeNode(CraftingRecipe craftingRecipe) : base(craftingRecipe)
		{
			this.craftingRecipe = craftingRecipe;
			
			GraphInput.DoubleClick += OpenResourceBrowser;
		}

		private void OpenResourceBrowser(object sender, GraphInputEvent e)
		{
			if (GraphInput.IsUnderMouse(e.transform, RectPosition))
			{
				ContentBrowser.Open<ResourceBrowser>((resource) => { craftingRecipe.result = resource as ResourceBaseConfig; }, true);
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
			
			if (craftingRecipe.result != null)
			{
				EditorGUI.DropShadowLabel(selectRect.SetHeight(25), $"{craftingRecipe.result.Name}", BattlepassStyle.middleH0Label);
			}

			EditorGUI.DrawRect(rect.SetHeight(1).AddY(selectRect.height + 1), new Color(0.16f, 0.16f, 0.16f));
			var fieldRect = selectRect.SetHeight(20).AddY(selectRect.height + 5);
			fieldRect.width -= 30f;
			fieldRect.x += 30f;

			GUI.Label(fieldRect.SetWidth(30).AddX(-30), FenrirEditor.CreateNamedIcon("", "d_UnityEditor.ProfilerWindow@2x"));
			GUI.Label(fieldRect.SetWidth(30).AddX(-30).AddY(25), FenrirEditor.CreateNamedIcon("", "Import-Available@2x"));

			craftingRecipe.time = EditorGUI.IntField(fieldRect, craftingRecipe.time);
			craftingRecipe.count = EditorGUI.IntField(fieldRect.AddY(25), craftingRecipe.count);
		}

		private void DrawIcon(Rect rect)
		{
			if (craftingRecipe.result == null)
			{
				GUI.Label(rect, FenrirEditor.CreateNamedIcon("", "Toolbar Plus@2x"), BattlepassStyle.middleH0Label);
				return;
			}

			var resourceIcon = AssetPreview.GetAssetPreview(craftingRecipe.result.Icon);
			GUI.Label(rect, resourceIcon);
		}
	}
}