using System;
using UnityEngine;

namespace Framework.Core
{
	public static class Context
	{
		public static DiContainer DiContainer { get; private set; }

		public static event Action OnPreInstall;
		public static event Action OnPostInstall;
		
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void Initialize()
		{
			OnPreInstall?.Invoke();
			DiContainer = new DiContainer();
			OnPostInstall?.Invoke();

			SetupBooststrap();
		}
		
		private static void SetupBooststrap()
		{
			DiContainer.BindConfigs();
			
			var bootstrapInstaller = new BootstrapInstaller();
			bootstrapInstaller.InstallBindings(DiContainer);
		}
	}	
}