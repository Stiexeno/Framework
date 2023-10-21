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
			SetupCore();
			
			// Setting new Instantiator
			DiContainer.Resolve<InstantiatorProvider>().instantiator = DiContainer;
			
			SetupBootstrap();
			
			OnPostInstall?.Invoke();
		}

		private static void SetupCore()
		{
			DiContainer.BindConfigs();
			
			var bootstrapInstaller = ScriptableObject.CreateInstance<CoreInstaller>();
			bootstrapInstaller.InstallBindings(DiContainer);
		}

		private static void SetupBootstrap()
		{
			var bootstrapInstaller = ScriptableObject.CreateInstance("BootstrapInstaller") as BootstrapBaseInstaller;

			if (bootstrapInstaller != null)
			{
				bootstrapInstaller.InstallBindings(DiContainer);

				if (bootstrapInstaller is IInitializable initializable)
				{
					initializable.Initialize();
				}
			}
		}

		public static void Exception(string message, string solution = "")
		{
			throw new Exception($"{"<b>[Inject]</b>".SetColor(new Color(0.64f, 0.87f, 0.18f))} {message} \n{solution.SetColor(new Color(0.98f, 0.69f, 0.16f))}");
		}
	}	
}