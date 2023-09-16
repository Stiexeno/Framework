using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Framework.Core;
using UnityEditor;
using UnityEngine;
using Directory = UnityEngine.Windows.Directory;

namespace Framework.Editor
{
	public static class SystemGenerator
	{
		private static readonly HashSet<Type> excludedInstallers = new HashSet<Type>
		{
			typeof(CoreInstaller),
			typeof(SceneInstaller)
		};

		public static void GenerateInstallers()
		{
			SceneContext sceneContext = Context.SceneContext;
			if (sceneContext == null)
			{
				sceneContext = GameObject.FindObjectOfType<SceneContext>();
			}
			
			if (sceneContext != null && sceneContext.Installers != null)
			{
				for (int i = sceneContext.Installers.Length - 1; i >= 0; i--)
				{
					var sceneContextInstallers = sceneContext.Installers;
					if (sceneContextInstallers[i] == null)
					{
						ArrayUtility.RemoveAt(ref sceneContextInstallers, i);
					}
				}
			}
			
			var installerTypes = AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(assembly => assembly.GetTypes())
				.Where(x => !x.IsAbstract)
				.Where(type => type.IsSubclassOf(typeof(AbstractInstaller)))
				.Where(x => !excludedInstallers.Contains(x))
				.ToArray();
			
			var folderPath = "Assets/Configs/Installers";
			var createDefaultFolder = true;
			
			if (SystemSettings.Settings.defaultInstallerFolder != null)
			{
				folderPath = AssetDatabase.GetAssetPath(SystemSettings.Settings.defaultInstallerFolder);
				createDefaultFolder = false;
			}

			if (AssetDatabase.IsValidFolder("Assets/Configs") == false)
			{
				AssetDatabase.CreateFolder("Assets", "Configs");
				EditorUtility.SetDirty(SystemSettings.Settings);
			}
			
			if (createDefaultFolder && AssetDatabase.IsValidFolder("Assets/Configs/Installers") == false)
			{
				AssetDatabase.CreateFolder("Assets/Configs", "Installers");
				EditorUtility.SetDirty(SystemSettings.Settings);
			}

			AssetDatabase.Refresh();

			foreach (var installer in installerTypes)
			{
				if (installer.Name == "BootstrapInstaller")
					continue;
				
				if (installer.Name == "BootstrapBaseInstaller")
					continue;
				
				var installerName = installer.Name;
				var installerPath = $"{folderPath}/{installerName}.asset";
				var installerAsset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(installerPath);

				if (installerAsset == null)
				{
					var installerAssets = ScriptableObject.CreateInstance(installer);
					AssetDatabase.CreateAsset(installerAssets, installerPath);
					AssetDatabase.SaveAssets();
					EditorUtility.SetDirty(installerAssets);
					AssetDatabase.Refresh();
				}
			}
		}

		public static void GenerateAssetsScript()
		{
			AssetSettings.Settings.GenerateAssetsScript();
		}

		public static void GenerateConfigs(Action callback = null)
		{
			var configsSettings = ConfigSettings.Settings;
			var configs = configsSettings.configs;
			
			for (int i = configs.Count - 1; i >= 0; i--)
			{
				if (configs[i] == null)
				{
					configs.RemoveAt(i);
					EditorUtility.SetDirty(configsSettings);
				}
			}
			
			foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				foreach (Type tp in assembly.GetTypes())
				{
					if (tp.BaseType == typeof(AbstractConfig))
					{
						if (configs.Contains(x => x.GetType() == tp))
							continue;

						var folderPath = "Assets/Configs";

						if (SystemSettings.Settings.defaultConfigsFolder != null)
						{
							folderPath = AssetDatabase.GetAssetPath(SystemSettings.Settings.defaultConfigsFolder);
						}
						
						var path =  $"{folderPath}/{tp.Name}.asset";
						var targetConfig = AssetDatabase.LoadAssetAtPath<AbstractConfig>(path);

						if (targetConfig == null)
						{
							var instance = ScriptableObject.CreateInstance(tp);
							AssetDatabase.CreateAsset(instance, path);
							targetConfig = AssetDatabase.LoadAssetAtPath<AbstractConfig>(path);
						}

						configs.Add(targetConfig);
						EditorUtility.SetDirty(configsSettings);

						AssetDatabase.Refresh();
						callback?.Invoke();
					}
				}
			}
			
			ConfigSettings.Settings.OnResfresh.Invoke();

			//? This is way to execute UnityEvent in EditorMode
			var evnt = ConfigSettings.Settings.OnResfresh;
			for (int i = 0; i < evnt.GetPersistentEventCount(); i++)
			{
				evnt.GetPersistentTarget(i)
					.GetType()
					.GetMethod(evnt.GetPersistentMethodName(i))
					.Invoke(evnt.GetPersistentTarget(i), null);
			}
		}

		public static void CheckForInstallers()
		{
			return;
			if (EditorPrefs.GetBool("RequestBootstrap"))
			{
				var bootstrap = ScriptableObject.CreateInstance("BootstrapInstaller");
				string resourcesFolder = Path.Combine(Application.dataPath, "Resources");

				if (Directory.Exists(resourcesFolder) == false)
				{
					Directory.CreateDirectory(resourcesFolder);
				}

				string bootstrapAssetPath = "Assets/Resources/BootstrapInstaller.asset";
				AssetDatabase.CreateAsset(bootstrap, bootstrapAssetPath);
				
				EditorPrefs.SetBool("RequestBootstrap", false);
			}
		}
	}	
}