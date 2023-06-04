using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Framework.Editor
{
    [InitializeOnLoad]
    public static class ToolbarScene
    {
        private static ScriptableObject toolbar;
        private static string[] scenePaths;
        private static string[] sceneNames;

        static ToolbarScene()
        {
            ToolbarExtender.RegisterRightEntry(OnToolbarGUI, 2);
        }

        static void OnToolbarGUI()
        {
            if (scenePaths == null || scenePaths.Length != EditorBuildSettings.scenes.Length)
            {
                var paths = new List<string>();
                var names = new List<string>();

                foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
                {
                    if (scene.path == null || scene.path.StartsWith("Assets") == false)
                        continue;

                    string scenePath = Application.dataPath + scene.path.Substring(6);

                    paths.Add(scenePath);
                    names.Add(Path.GetFileNameWithoutExtension(scenePath));
                }

                scenePaths = paths.ToArray();
                sceneNames = names.ToArray();
            }
            
            string sceneName = EditorSceneManager.GetActiveScene().name;
            int sceneIndex = -1;

            for (int i = 0; i < sceneNames.Length; ++i)
            {
                if (sceneName == sceneNames[i])
                {
                    sceneIndex = i;
                    break;
                }
            }

            int newSceneIndex = EditorGUILayout.Popup(sceneIndex, sceneNames, EditorStyles.toolbarPopup,GUILayout.Width(200.0f));

            if (newSceneIndex != sceneIndex)
            {
                if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                {
                    EditorSceneManager.OpenScene(scenePaths[newSceneIndex], OpenSceneMode.Single);
                }
            }
        }
    }
}