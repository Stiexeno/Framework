using System;
using UnityEngine.SceneManagement;

namespace Framework.Core
{
    public interface ISceneManager
    {
        event Action<Scene, Scene> OnSceneChanged;
        float SceneLoadProgress { get; }
        Scene ActiveScene { get; }
        void SwitchLoadedScene();
        void LoadScene(int sceneIndex, LoadSceneMode mode = LoadSceneMode.Single, bool switchLoadedScene = true, float delay = 0f,
            Action done = null);
    }
}
