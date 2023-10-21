using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Framework.Core;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;
using SF = UnityEngine.SerializeField;

namespace Framework.Editor
{
	public class AssetSettings : ScriptableObject
	{
		[SF] internal List<Object> assets = new List<Object>();
		
		private const string ASSET_NAME = "AssetSettings";
		private const string OBJECT_NAME = "com.framework.asset-settings";
		private const string PATH = "Assets/Resources/Settings/";
		
		private static string DefaultAssetPath => PATH + "/" + ASSET_NAME + ".asset";

		private static AssetSettings assetSettings;
		
		private static HashSet<Type> excludedTypes = new HashSet<Type>
		{
			typeof(AssetSettings),
			typeof(ConfigSettings),
			typeof(SystemSettings),
			typeof(ControllerSettings),
			typeof(RequiredData),
			typeof(BootstrapBaseInstaller)
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
			foreach (var importedAsset in importedAssets)
			{
				internalValidate(importedAsset, true);
			}
			
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

				if (path.Contains("BootstrapInstaller"))
					return;
				
				for (var i = Settings.assets.Count - 1; i >= 0; i--)
				{
					var asset = Settings.assets[i];
					
					if (asset == null)
					{
						Settings.assets.RemoveAt(i);
						EditorUtility.SetDirty(Settings);
						continue;
					}
					
					var assetPath = AssetDatabase.GetAssetPath(asset);
					if (assetPath == path)
					{
						if (IsInResources(assetPath) == false)
						{
							Settings.RemoveEntry(asset);
							EditorUtility.SetDirty(Settings);
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
							EditorUtility.SetDirty(Settings);
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
			EditorUtility.SetDirty(this);
		}
		
		internal void RemoveEntry(Object asset)
		{
			if (asset == null)
				return;

			if (!assets.Contains(asset))
				return;

			assets.Remove(asset);
			EditorUtility.SetDirty(this);
		}
		
		internal static bool IsInResources(string path)
		{
			return path.Replace('\\', '/').ToLower().Contains("assets/resources/");
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

		internal void GenerateAssetsScript(Action callback = null)
		{
			string scriptContent = GenerateScriptContent();
            
			string outputPath = SystemSettings.Settings.defaultGeneratedAssetFolder != null ? 
				AssetDatabase.GetAssetPath(SystemSettings.Settings.defaultGeneratedAssetFolder) + "/Assets.cs" : 
				"Assets/Scripts/Generated/Assets.cs";
			
			EnsureOutputFolderExists(outputPath);
			File.WriteAllText(outputPath, scriptContent);
			EditorUtility.SetDirty(this);

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
			var foldersWithAssets = new List<FolderWithAssets>();
			var assetsWithoutFolder = new List<Object>();
			
			var folders = Settings.assets.Where(x => AssetDatabase.IsValidFolder(AssetDatabase.GetAssetPath(x))).ToList();

			foreach (var folder in folders)
			{
				foldersWithAssets.Add(new FolderWithAssets
				{
					folder = folder,
					assets = new List<Object>()
				});
			}

			foreach (var asset in Settings.assets)
			{
				var assetPath = AssetDatabase.GetAssetPath(asset);

				if (AssetDatabase.IsValidFolder(assetPath))
					continue;
				
				var folder = foldersWithAssets.FirstOrDefault(x => assetPath.StartsWith(AssetDatabase.GetAssetPath(x.folder)));

				if (folder.folder != null)
				{
					Object assetInstance = AssetDatabase.LoadAssetAtPath<Object>(assetPath);
					folder.assets.Add(assetInstance);
					
					continue;
				}

				assetsWithoutFolder.Add(asset);	
			}
			
			StringBuilder scriptBuilder = new StringBuilder();
			
			scriptBuilder.AppendLine("namespace Framework.Generated");
			scriptBuilder.AppendLine("{");
			scriptBuilder.AppendLine("\tpublic static class Assets");
			scriptBuilder.AppendLine("\t{");

			foreach (var folder in foldersWithAssets)
			{
				scriptBuilder.AppendLine("\t\tpublic static class " + folder.folder.name.Replace(" ", ""));
				scriptBuilder.AppendLine("\t\t{");

				foreach (var asset in folder.assets)
				{
					var assetPath = AssetDatabase.GetAssetPath(asset);
					string filePath = Path.GetDirectoryName(assetPath).Replace("\\", "/");
					
					var assetName = Path.GetFileNameWithoutExtension(assetPath).Replace(" ", "");
					
					string[] assetSplitPath = filePath.Split('/');
					string resourcePath = string.Join("/", assetSplitPath, 2, assetSplitPath.Length - 2);
					
					scriptBuilder.AppendLine($"\t\t\tpublic const string {assetName} = \"{resourcePath}/{assetName}\";");
				}
				
				scriptBuilder.AppendLine("\t\t}");
				scriptBuilder.AppendLine("");
			}
			
			foreach (var assetWithoutFolder in assetsWithoutFolder)
			{
				var assetPath = AssetDatabase.GetAssetPath(assetWithoutFolder);
				string filePath = Path.GetDirectoryName(assetPath).Replace("\\", "/");
					
				var assetName = Path.GetFileNameWithoutExtension(assetPath).Replace(" ", "");
					
				string[] assetSplitPath = filePath.Split('/');
				string resourcePath = string.Join("/", assetSplitPath, 2, assetSplitPath.Length - 2);
					
				scriptBuilder.AppendLine($"\t\tpublic const string {assetName} = \"{resourcePath}/{assetName}\";");
			}

			scriptBuilder.AppendLine("\t}");
			scriptBuilder.AppendLine("}");
			
			return scriptBuilder.ToString();
		}

		private struct FolderWithAssets
		{
			public Object folder;
			public List<Object> assets;
		}
	}	
}