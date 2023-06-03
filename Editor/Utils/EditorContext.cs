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
			SpawnPrefab<Progressbar>($"Packages/{CoreConstants.PACKAGE_NAME}/Prefabs/Progressbar.prefab");
		}
		
		[MenuItem("GameObject/UI/Button", false, 14)]
		public static void CreateButton(MenuCommand command)
		{
			SpawnPrefab<Button>($"Packages/{CoreConstants.PACKAGE_NAME}/Prefabs/Button.prefab");
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