using UnityEditor;

namespace Framework.Editor
{
	public static class ControllerShortcut
	{
		[MenuItem("Framework/Controller %e")]
		private static void OpenController()
		{
			Selection.activeObject = ControllerSettings.Settings;
		}
	}
   
}