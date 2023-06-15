using Framework.Editor;
using UnityEditor;
using UnityEngine;
using SF = UnityEngine.SerializeField;

    public static class ControllerShortcut
    {
        [MenuItem("Framework/Controller %e")]
        private static void OpenController()
        {
            Selection.activeObject = ControllerSettings.Settings;
        }
    }
