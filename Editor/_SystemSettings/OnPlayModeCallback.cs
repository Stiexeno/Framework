using UnityEditor;

namespace Framework.Editor
{
	[InitializeOnLoad]
	public class OnPlayModeCallback
	{
		static OnPlayModeCallback()
		{
			EditorApplication.update += OnEditorUpdate;
		}

		private static void OnEditorUpdate()
		{
			if (EditorApplication.isPlayingOrWillChangePlaymode && !EditorApplication.isPlaying)
			{
				if (SystemSettings.Settings.autoRefreshOnPlay)
				{
					SystemGenerator.GenerateInstallers();
					SystemGenerator.GenerateConfigs();
					SystemGenerator.GenerateAssetsScript();
				}
			}
		}
	}	
}