using Framework.Core;
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
					ProjectContext.GenerateInstallers();
					ConfigSettings.Settings.GenerateConfigs();
					AssetSettings.Settings.GenerateAssetsScript();	
				}
			}
		}
	}	
}