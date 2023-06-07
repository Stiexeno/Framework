using System;
using System.Collections.Generic;
using System.IO;
#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Assertions;

namespace Framework.Core
{
	public static class ProjectContext
	{
		public static DiContainer DiContainer { get; private set; }

		public static float TimeTookToInstall { get; set; }

		public static event Action OnPreInstall;
		public static event Action OnPostInstall;
		
		private const string INSTALLERS_PATH = "Configs/Installers/";
		private static readonly HashSet<Type> excludedInstallers = new HashSet<Type>
		{
			typeof(BootstrapInstaller)
		};
		
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void Initialize()
		{
			Assert.IsNotNull(GameObject.FindAnyObjectByType<SceneContext>(), 
				"Could not find SceneContext in scene");
			
			OnPreInstall?.Invoke();

			InternalInitialize();

			OnPostInstall?.Invoke();

			SetupBooststrap();
		}

		private static void InternalInitialize()
		{
			DiContainer = new DiContainer();
			
			// Add here InjectableMonoBehaviours
			// And queue it for injection
			
			// After setup BootstrapInstaller
			
			// Resolve roots
		}
		
		private static void SetupBooststrap()
		{
			DiContainer.BindConfigs();
			
			var bootstrapInstaller = ScriptableObject.CreateInstance<BootstrapInstaller>();
			bootstrapInstaller.InstallBindings(DiContainer);
		}
		
		#if UNITY_EDITOR
		public static void GenerateInstallers()
		{
			var installerTypes = AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(assembly => assembly.GetTypes())
				.Where(x => !x.IsAbstract)
				.Where(type => type.IsSubclassOf(typeof(AbstractInstaller)))
				.Where(x => !excludedInstallers.Contains(x))
				.ToArray();

			var configsPath = Path.Combine(Application.dataPath, "Configs");
			var installersPath = Path.Combine(Application.dataPath, INSTALLERS_PATH);

			if (Directory.Exists(configsPath) == false)
			{
				Directory.CreateDirectory(configsPath);
			}

			if (Directory.Exists(installersPath) == false)
			{
				Directory.CreateDirectory(installersPath);
			}

			AssetDatabase.Refresh();

			foreach (var installer in installerTypes)
			{
				var installerName = installer.Name;
				var installerPath = "Assets/" + INSTALLERS_PATH + installerName + ".asset";
				var installerAsset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(installerPath);

				if (installerAsset == null)
				{
					var installerAssets = ScriptableObject.CreateInstance(installer);
					AssetDatabase.CreateAsset(installerAssets, installerPath);
					AssetDatabase.SaveAssets();
					AssetDatabase.Refresh();
				}
			}
		}
		#endif
	}	
}