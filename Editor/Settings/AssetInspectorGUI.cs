using System;
using System.Collections.Generic;
using Framework.Core;
using Framework.Editor;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

[InitializeOnLoad]
public static class AssetInspectorGUI
{
	private static HashSet<Type> excludedTypes = new HashSet<Type>
	{
		typeof(AssetSettings),
		typeof(ConfigSettings),
		typeof(SystemSettings),
		typeof(ControllerSettings),
		typeof(RequiredData)
	};
	
	static AssetInspectorGUI()
	{
		Editor.finishedDefaultHeaderGUI += OnPostHeaderGUI;
	}

	private static void OnPostHeaderGUI(Editor editor)
	{
		if (editor.target != null)
		{
			var target = editor.target;
			if (AssetDatabase.IsMainAsset(target) && excludedTypes.Contains(target.GetType()) == false)
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