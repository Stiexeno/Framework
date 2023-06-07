using System;
using System.Collections.Generic;
using System.IO;
using Framework.Utils;
#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Assertions;

namespace Framework.Core
{
	public static class Context
	{
		public static DiContainer DiContainer { get; private set; }
		public static SceneContext SceneContext { get; set; }

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
			Assert.IsNotNull(GameObject.FindObjectOfType<SceneContext>(), 
				"Could not find SceneContext in scene");
			
			OnPreInstall?.Invoke();

			DiContainer = new DiContainer();
			SetupBooststrap();
			OnPostInstall?.Invoke();
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
		
		public static void Exception(string message, string solution = "")
		{
			throw new Exception($"{"[Inject]".SetColor(new Color(0.64f, 0.87f, 0.18f))} {message} \n{solution.SetColor(new Color(0.98f, 0.69f, 0.16f))}");
		}
	}	
}