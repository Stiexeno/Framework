using Framework.Editor;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class AssetInspectorGUI
{
	static AssetInspectorGUI()
	{
		Editor.finishedDefaultHeaderGUI += OnPostHeaderGUI;
	}

	private static void OnPostHeaderGUI(Editor editor)
	{
		if (editor.target != null)
		{
			var target = editor.target;
			if (AssetDatabase.IsMainAsset(target))
			{
				using (var toggle = new EditorGUI.ChangeCheckScope())
				{
					var assetSettings = AssetSettings.Settings;

					var isGenerated = assetSettings.assets.Contains(target);
					var newIsGenerated = GUILayout.Toggle(isGenerated, "Generated Asset");

					if (toggle.changed)
					{
						if (newIsGenerated)
						{
							if (Validate(target))
							{
								assetSettings.RegisterEntry(target);
							}
						}
						else
						{
							assetSettings.RemoveEntry(target);
						}
					}
				}
			}
		}
	}

	private static bool Validate(Object obj)
	{
		const string resourcePath = "Assets/Resources/";

		string assetPath = AssetDatabase.GetAssetPath(obj);

		if (AssetSettings.IsInResources(assetPath) == false)
		{
			var confirm = EditorUtility.DisplayDialog("Move To Resources",
				"Asset must be moved to Resource folder \n\nAre you sure you want to proceed?", "Yes", "No");

			if (confirm)
			{
				AssetDatabase.MoveAsset(assetPath, resourcePath + obj.name + ".asset");
				EditorGUIUtility.PingObject(obj);

				return true;
			}

			return false;
		}

		return true;
	}
}