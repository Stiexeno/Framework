using System.Collections.Generic;
using UnityEngine;

namespace Framework.Core
{
    public static class InjectExtensions
    {
        public static void GetSceneMonoBehaviours(ref List<MonoBehaviour> monoBehaviours)
        {
            var sceneGameObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();

            foreach (GameObject sceneGameObject in sceneGameObjects)
            {
                var components = sceneGameObject.GetComponentsInChildren<MonoBehaviour>(true);
                monoBehaviours.AddRange(components);
            }

            var uiManager = monoBehaviours.Find(x => x is UIManager) as UIManager;
            if (uiManager != null)
            {
                monoBehaviours.Remove(uiManager);
                monoBehaviours.Add(uiManager);
            }
        }
    }
}