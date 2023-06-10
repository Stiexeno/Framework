using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            
            if (IsSceneInBuildSettings(EditorSceneManager.GetActiveScene()) == false)
            {
                if (GUILayout.Button(EditorHelper.Icon("Add Open Scene", "Warning"), EditorStyles.toolbarButton, GUILayout.Width(130.0f)))
                {
                    EditorBuildSettings.scenes = EditorBuildSettings.scenes.Append(new EditorBuildSettingsScene(EditorSceneManager.GetActiveScene().path, true)).ToArray();
                }
            }
        }
        
        public static bool IsSceneInBuildSettings(Scene scene)
        {
            string scenePath = scene.path;

            for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
            {
                if (EditorBuildSettings.scenes[i].path == scenePath)
                {
                    return true;
                }
            }

            return false;
        }
    }
}