using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Framework.Core
{
	public class ConfigSettings : ScriptableObject
	{
		public List<AbstractConfig> configs = new List<AbstractConfig>();

		private const string ASSET_NAME = "ConfigSettings";
		private const string OBJECT_NAME = "com.framework.config-settings";
		private const string PACKAGES_PATH = "Assets/Resources/Settings/";

		private static string DefaultAssetPath => PACKAGES_PATH + "/" + ASSET_NAME + ".asset";

		private static ConfigSettings configSettings;

		public static ConfigSettings Settings
		{
			get
			{
				if (configSettings == null)
				{
#if UNITY_EDITOR
					if (EditorBuildSettings.TryGetConfigObject(OBJECT_NAME, out ConfigSettings settings))
					{
						if (settings != null)
						{
							configSettings = settings;
							return configSettings;
						}

						EditorBuildSettings.RemoveConfigObject(OBJECT_NAME);
					}

					var asset = AssetDatabase.LoadAssetAtPath<ConfigSettings>(DefaultAssetPath);
					if (asset == null)
					{
						var assetInstance = CreateInstance<ConfigSettings>();
						AssetDatabase.CreateAsset(assetInstance, DefaultAssetPath);
						AssetDatabase.SaveAssets();

						EditorBuildSettings.AddConfigObject(OBJECT_NAME, assetInstance, true);
						EditorUtility.SetDirty(assetInstance);

						configSettings = AssetDatabase.LoadAssetAtPath<ConfigSettings>(DefaultAssetPath);
					}

#endif
					if (Application.isPlaying)
					{
						var instance = Resources.Load<ConfigSettings>(DefaultAssetPath);

						if (instance == null)
						{
							throw new NullReferenceException("ConfigSettings not found, please refresh project.");
						}

						configSettings = instance;	
					}
				}
				return configSettings;
			}
		}
	}
}