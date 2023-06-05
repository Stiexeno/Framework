using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;

namespace Framework.Core
{
	public class SystemSettings : ScriptableObject
	{
		public bool autoRefreshOnPlay;
		public bool deleteDataConfirmation;
		
		private const string ASSET_NAME = "SystemSettings";
		private const string OBJECT_NAME = "com.framework.system-settings";
		private const string PATH = "Assets/Resources/Settings/";
		
		private static string DefaultAssetPath => PATH + "/" + ASSET_NAME + ".asset";

		private static SystemSettings systemSettings;
		
		public static SystemSettings Settings
		{
			get
			{
				if (systemSettings == null)
				{
					if (EditorBuildSettings.TryGetConfigObject(OBJECT_NAME, out SystemSettings settings))
					{
						if (settings != null)
						{
							systemSettings = settings;
							return systemSettings;
						}
						
						EditorBuildSettings.RemoveConfigObject(OBJECT_NAME);
					}
					
					var asset = AssetDatabase.LoadAssetAtPath<SystemSettings>(DefaultAssetPath);
					if (asset == null)
					{
						var systemInstance = CreateInstance<SystemSettings>();

						if (Directory.Exists(PATH) == false)
						{
							if ((Directory.Exists("Assets/Resources") == false))
							{
								AssetDatabase.CreateFolder("Assets", "Resources");
							}
							
							AssetDatabase.CreateFolder("Assets/Resources", "Settings");
						}
						AssetDatabase.CreateAsset(systemInstance, DefaultAssetPath);
						AssetDatabase.SaveAssets();
						AssetDatabase.Refresh();
							
						EditorBuildSettings.AddConfigObject(OBJECT_NAME, systemInstance, true);
						EditorUtility.SetDirty(systemInstance);

						systemSettings = AssetDatabase.LoadAssetAtPath<SystemSettings>(DefaultAssetPath);
					}
				}

				return systemSettings;
			}
		}
	}
}