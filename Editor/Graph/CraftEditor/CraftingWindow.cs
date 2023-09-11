using System.Collections.Generic;
using Framework.Editor;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using SF = UnityEngine.SerializeField;

namespace Framework
{
	public class CraftingWindow : GraphWindow
	{
		private static CraftingWindow currentWindow;
		private static CraftingRecipe recipe;
		
		public override GraphBehaviour Root
		{
			get => recipe;
			set => recipe = value as CraftingRecipe;
		}

		[MenuItem("Game/Crafting Graph")]
		private static void Init()
		{
			currentWindow = CreateInstance<CraftingWindow>();
			currentWindow.titleContent = new GUIContent("Crafting Editor");
			currentWindow.Show();
		}

		protected override void Initialize(GraphBehaviour behaviour)
		{
			base.Initialize(behaviour);
			recipe = behaviour as CraftingRecipe;
		}

		protected override void Construct(HashSet<IGUIElement> graphElements)
		{
			//graphElements.Add(new CraftingToolbar());
			//graphElements.Add(new CraftingInspector());
			//graphElements.Add(new GraphOverlay());
		}

		protected override List<GraphNode> GatherBehaviours()
		{
			if (recipe == null)
				return new List<GraphNode>();
			
			List<GraphNode> nodes = new List<GraphNode>
			{
				new RecipeNode(recipe)
			};

			foreach (var ingridient in recipe.recipes)
			{
				nodes.Add(new IngridientNode(ingridient));
			}

			return nodes;
		}

		protected override GenericMenu RegisterContextMenu()
		{
			var menu = new GenericMenu();
			menu.AddItem(new GUIContent("Add Ingredient"), false, () => 
				Editor.CreateNode(typeof(IngridientNode), typeof(CraftingIngridient)));

			return menu;
		}

		//private void OnSelectionChanged()
		//{
		//	if (Selection.activeObject == null)
		//		return;
//
		//	if (Selection.activeObject is CraftingRecipe craftingRecipe)
		//	{
		//		if (craftingRecipe != recipe)
		//		{
		//			Init(craftingRecipe);
		//		}
		//	}
		//}
		
		protected override void OnEnable()
		{
			base.OnEnable();
			//Selection.selectionChanged += OnSelectionChanged;
		}
		
		[OnOpenAsset]
		public static bool OpenAsset(int instanceId, int line)
		{
			var root = EditorUtility.InstanceIDToObject(instanceId) as CraftingRecipe;

			CraftingWindow window = Open<CraftingWindow>(root);
			
			if (window != null)
			{
				window.Initialize(root);
				return true;
			}

			return false;
		}
	}
}