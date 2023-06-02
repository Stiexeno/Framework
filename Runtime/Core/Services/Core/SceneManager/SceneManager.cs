using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using SF = UnityEngine.SerializeField;

namespace Framework.Core
{
    public class SceneManager : MonoBehaviour, ISceneManager
    {
        private AsyncOperation loadSceneOperation;
        
        public event Action<Scene, Scene> OnSceneChanged;
        
        public float SceneLoadProgress { get; private set; }
        public Scene ActiveScene { get; private set; }

        // ISceneManager

        public void SwitchLoadedScene()
        {
            loadSceneOperation.allowSceneActivation = true;
        }

        public void LoadScene(int sceneIndex, LoadSceneMode mode = LoadSceneMode.Single, bool switchLoadedScene = true, float delay = 0f, Action done = null)
        {
            StartCoroutine(LoadSceneCoroutine(sceneIndex, mode, switchLoadedScene, delay, done));
        }
        
        private IEnumerator LoadSceneCoroutine(int sceneIndex, LoadSceneMode mode, bool switchLoadedScene, float delay, Action done)
        {
            SceneLoadProgress = 0;
            
            loadSceneOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneIndex, mode);

            loadSceneOperation.allowSceneActivation = switchLoadedScene;
            
            while (loadSceneOperation.isDone == false && loadSceneOperation.progress < 0.9f)
            {
                yield return null;
                SceneLoadProgress = loadSceneOperation.progress;
            }

            SceneLoadProgress = 1;
            
            yield return new WaitForSeconds(delay);

            done?.Invoke();
        }
        
        private void OnActiveSceneChanged(Scene oldScene, Scene newScene)
        {
            ActiveScene = newScene;
            OnSceneChanged?.Invoke(oldScene, newScene);
        }

        private void Awake()
        {
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += OnActiveSceneChanged;
        }
    }
}
