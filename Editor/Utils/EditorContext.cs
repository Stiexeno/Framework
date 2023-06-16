using Framework.Core;
using Framework.UI;
using Framework.Utils;
using UnityEditor;
using UnityEngine;

namespace Framework.Editor
{
	public static class EditorContext
	{
		[MenuItem("GameObject/UI/ProgressBar", false, 15)]
		public static void CreateProgressbar(MenuCommand command)
		{
			SpawnPrefab<ProgressBar>($"Packages/{CoreConstants.PACKAGE_NAME}/Runtime/Prefabs/Progressbar.prefab");
		}
		
		[MenuItem("GameObject/UI/Button", false, 14)]
		public static void CreateButton(MenuCommand command)
		{
			SpawnPrefab<Button>($"Packages/{CoreConstants.PACKAGE_NAME}/Runtime/Prefabs/Button.prefab");
		}
		
		[MenuItem("CONTEXT/RectTransform/Expand")]
		public static void ExpandRectTransform(MenuCommand command)
		{
			var rect = (RectTransform) command.context;
	        
			rect.anchorMax = Vector2.one;
			rect.anchorMin = Vector2.zero;
	        
			rect.offsetMin = new Vector2(0, rect.offsetMin.y);
			rect.offsetMax = new Vector2(0, rect.offsetMax.y);
			rect.offsetMin = new Vector2(rect.offsetMin.x, 0);
			rect.offsetMax = new Vector2(rect.offsetMax.x, 0);
		}
		
		private static void SpawnPrefab<T>(string path) where T : MonoBehaviour
		{
			var selectedGameObject = Selection.activeTransform;
            
			var prefab = AssetDatabase.LoadAssetAtPath(path, typeof(T));
            
			T instance = PrefabUtility.InstantiatePrefab(prefab, selectedGameObject != null ? selectedGameObject : null) as T;
            
			if (instance == null)
			{
				Debug.LogError($"Cannot create {nameof(T)}!");
				return;
			}
            
			instance.transform.SetAsLastSibling();
            
			PrefabUtility.UnpackPrefabInstance(instance.gameObject, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
            
			Selection.activeObject = instance;
			EditorUtility.SetDirty(instance);
		}
	}
}