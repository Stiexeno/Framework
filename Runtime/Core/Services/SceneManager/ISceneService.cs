using System;
using UnityEngine.SceneManagement;

namespace Framework.Core
{
    public interface ISceneService
    {
        event Action<Scene, Scene> OnSceneChanged;
        float SceneLoadProgress { get; }
        Scene ActiveScene { get; }
        void SwitchLoadedScene();
        void LoadScene(string sceneName, LoadSceneMode mode = LoadSceneMode.Single, bool switchLoadedScene = true, float delay = 0f,
            Action done = null);

        void LoadScene(string name, Action onLoadded = null);
    }
}
