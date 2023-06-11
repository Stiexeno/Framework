using System;
using Framework.Utils;
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

		public static void Exception(string message, string solution = "")
		{
			throw new Exception($"{"[Inject]".SetColor(new Color(0.64f, 0.87f, 0.18f))} {message} \n{solution.SetColor(new Color(0.98f, 0.69f, 0.16f))}");
		}
	}	
}