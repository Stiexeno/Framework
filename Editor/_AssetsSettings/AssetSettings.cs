using System;
using System.Collections.Generic;
using System.IO;
using Framework.Core;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Framework.Editor
{
	[CreateAssetMenu(fileName = "AssetSettings", menuName = "Framework/AssetSettings", order = 0)]
	public class AssetSettings : ScriptableObject
	{
		public List<Object> assets = new List<Object>();
		
		private const string ASSET_NAME = "AssetSettings";
		private const string OBJECT_NAME = "com.framework.asset-settings";
		private const string PATH = "Assets/Resources/Settings/";
		
		private static string DefaultAssetPath => PATH + "/" + ASSET_NAME + ".asset";

		private static AssetSettings assetSettings;
		
		private static HashSet<Type> excludedTypes = new HashSet<Type>
		{
			typeof(AssetSettings),
			typeof(ConfigSettings),
			typeof(SystemSettings)
		};

		public static AssetSettings Settings
		{
			get
			{
				if (assetSettings == null)
				{
					if (EditorBuildSettings.TryGetConfigObject(OBJECT_NAME, out AssetSettings settings))
					{
						if (settings != null)
						{
							assetSettings = settings;
							return assetSettings;
						}
						
						EditorBuildSettings.RemoveConfigObject(OBJECT_NAME);
					}
					
					var asset = AssetDatabase.LoadAssetAtPath<AssetSettings>(DefaultAssetPath);
					if (asset == null)
					{
						var assetInstance = CreateInstance<AssetSettings>();

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

						assetSettings = AssetDatabase.LoadAssetAtPath<AssetSettings>(DefaultAssetPath);
					}
				}

				return assetSettings;
			}
		}

		[InitializeOnLoadMethod]
		static void RegisterWithAssetPostProcessor()
		{
			AssetPostProcessor.OnPostProcess.Register(OnPostProcessAllAssets, 0);
		}

		private static void OnPostProcessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
		{
			foreach (var movedAsset in movedAssets)
			{
				internalValidate(movedAsset, true);
			}
			
			foreach (var deletedAsset in deletedAssets)
			{
				internalValidate(deletedAsset, false);
			}

			void internalValidate(string path, bool recreate)
			{
				if (excludedTypes.Contains(AssetDatabase.GetMainAssetTypeAtPath(path)))
					return;
				
				for (var i = Settings.assets.Count - 1; i >= 0; i--)
				{
					var asset = Settings.assets[i];
					
					if (asset == null)
					{
						Settings.assets.RemoveAt(i);
						continue;
					}
					
					var assetPath = AssetDatabase.GetAssetPath(asset);
					if (assetPath == path)
					{
						if (IsInResources(assetPath) == false)
						{
							Settings.RemoveEntry(asset);
							break;
						}
					}
				}

				if (recreate == false)
					return;
				
				if (Settings.IsRegistered(path) == false)
				{
					if (IsInResources(path))
					{
						if (AssetDatabase.IsValidFolder(path) == false)
						{
							var asset = AssetDatabase.LoadAssetAtPath<Object>(path);
							Settings.RegisterEntry(asset);	
						}
					}
				}
				
			}
		}
		
		internal void RegisterEntry(Object asset)
		{
			if (asset == null)
				return;

			if (assets.Contains(asset))
				return;

			assets.Add(asset);
		}
		
		internal void RemoveEntry(Object asset)
		{
			if (asset == null)
				return;

			if (!assets.Contains(asset))
				return;

			assets.Remove(asset);
		}

		private bool IsRegistered(Object asset)
		{
			if (asset == null)
				return false;

			return assets.Contains(asset);
		}

		private bool IsRegistered(string path)
		{
			var assetPath = AssetDatabase.LoadAssetAtPath<Object>(path);
			return IsRegistered(assetPath);
		}
		
		internal static bool IsInResources(string path)
		{
			return path.Replace('\\', '/').ToLower().Contains("/resources/");
		}
		
		internal void GenerateAssetsScript(Action callback = null)
		{
			string scriptContent = GenerateScriptContent();

			string outputPath = "Assets/Scripts/Generated/AssetsPath.cs";
			EnsureOutputFolderExists(outputPath);
			File.WriteAllText(outputPath, scriptContent);

			AssetDatabase.Refresh();
			callback?.Invoke();
		}

		private void EnsureOutputFolderExists(string outputPath)
		{
			string outputFolder = Path.GetDirectoryName(outputPath);
			if (!Directory.Exists(outputFolder))
			{
				if (outputFolder != null) 
					Directory.CreateDirectory(outputFolder);
			}
		}

		private string GenerateScriptContent()
		{
			string scriptContent = "namespace Framework.Generated\n";
			scriptContent += "{\n\t" +
			                 "[System.Serializable]\n\t" +
			                 "public class AssetsPath\n\t" +
			                 "{\n";

			foreach (var asset in Settings.assets)
			{
				var file = AssetDatabase.GetAssetPath(asset);
				
				if (!file.EndsWith(".meta"))
				{
					string filePath = Path.GetDirectoryName(file).Replace("\\", "/");
					string resourceName = Path.GetFileNameWithoutExtension(file).Replace(" ", "");

					string[] folders = filePath.Split('/');
					string resourcePath = string.Join("/", folders, 2, folders.Length - 2);

					if (string.IsNullOrEmpty(resourcePath))
					{
						scriptContent += $"\t\tpublic const string {resourceName} = \"{resourceName}\";\n";
					}
					else
					{
						scriptContent += $"\t\tpublic const string {resourceName} = \"{resourcePath}/{resourceName}\";\n";
					}
				}
			}

			scriptContent += "\t}\n}";

			return scriptContent;
		}
	}	
}