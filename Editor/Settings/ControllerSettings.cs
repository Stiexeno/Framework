using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;

namespace Framework.Editor
{
	public class ControllerSettings : ScriptableObject
	{
		private const string ASSET_NAME = "ControllerSettings";
		private const string OBJECT_NAME = "com.framework.controller-settings";
		private const string PATH = "Assets/Resources/Settings/";

		private static string DefaultAssetPath => PATH + "/" + ASSET_NAME + ".asset";

		private static ControllerSettings controllerSettings;

		public static ControllerSettings Settings
		{
			get
			{
				if (controllerSettings == null)
				{
					if (EditorBuildSettings.TryGetConfigObject(OBJECT_NAME, out ControllerSettings settings))
					{
						if (settings != null)
						{
							controllerSettings = settings;
							return controllerSettings;
						}

						EditorBuildSettings.RemoveConfigObject(OBJECT_NAME);
					}

					var asset = AssetDatabase.LoadAssetAtPath<ControllerSettings>(DefaultAssetPath);
					if (asset == null)
					{
						var assetInstance = CreateInstance<ControllerSettings>();

						if (Directory.Exists(PATH) == false)
						{
							if ((Directory.Exists("Assets/Resources") == false))
							{
								AssetDatabase.CreateFolder("Assets", "Resources");
							}

							AssetDatabase.CreateFolder("Assets/Resources", "Settings");
						}

						AssetDatabase.CreateAsset(assetInstance, DefaultAssetPath);
						AssetDatabase.SaveAssets();
						AssetDatabase.Refresh();

						EditorBuildSettings.AddConfigObject(OBJECT_NAME, assetInstance, true);
						EditorUtility.SetDirty(assetInstance);

						controllerSettings = AssetDatabase.LoadAssetAtPath<ControllerSettings>(DefaultAssetPath);
					}
				}

				return controllerSettings;
			}
		}
	}
}