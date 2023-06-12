using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Framework.Core
{
    public static class InjectExtensions
    {
        public static void GetSceneMonoBehaviours(ref List<MonoBehaviour> monoBehaviours)
        {
            foreach (var rootObject in GetRooGameObjects())
            {
                if (rootObject != null)
                {
                    GetInjectableMonoBehavioursUnderGameObjectInternal(rootObject, ref monoBehaviours);
                }
            }

            var uiManager = monoBehaviours.Find(x => x is UIManager) as UIManager;
            if (uiManager != null)
            {
                monoBehaviours.Remove(uiManager);
                monoBehaviours.Add(uiManager);
            }
        }
        
        public static IEnumerable<MethodInfo> GetInjectableMethods(Type type)
        {
            MethodInfo[] methods = GetAllMethods(type);
            Array.Reverse(methods);
            
            return Array.FindAll(methods, HasInjectMethods);
        }
        
        private static void GetInjectableMonoBehavioursUnderGameObjectInternal(GameObject gameObject, ref List<MonoBehaviour> monoBehaviours)
        {
            if (gameObject == null)
                return;
            
            var foundBehaviours = gameObject.GetComponents<MonoBehaviour>();

            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                var child = gameObject.transform.GetChild(i);

                if (child != null)
                {
                    GetInjectableMonoBehavioursUnderGameObjectInternal(child.gameObject, ref monoBehaviours);
                }
            }
            
            for (int i = 0; i < foundBehaviours.Length; i++)
            {
                if (foundBehaviours[i] != null)
                {
                    monoBehaviours.Add(foundBehaviours[i]);
                }
            }
        }

        private static IEnumerable<GameObject> GetRooGameObjects()
        {
            var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();

            if (scene.isLoaded)
            {
                return scene.GetRootGameObjects();
            }

            return Resources.FindObjectsOfTypeAll<GameObject>()
                .Where(x => x.transform.parent == null && x.scene == scene);
        }

        private static MethodInfo[] GetAllMethods(Type type)
        {
            if (type == null || type == typeof(MonoBehaviour))
                return Array.Empty<MethodInfo>();
            
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy;
            MethodInfo[] methods = type.GetMethods(flags);

            // Get methods from base types recursively
            Type baseType = type.BaseType;
            MethodInfo[] baseMethods = GetAllMethods(baseType);

            MethodInfo[] allMethods = new MethodInfo[methods.Length + baseMethods.Length];
            methods.CopyTo(allMethods, 0);
            baseMethods.CopyTo(allMethods, methods.Length);
            
            return allMethods;
        }
        
        private static bool HasInjectMethods(MethodInfo methodInfo) =>
            methodInfo.GetCustomAttributes(typeof(InjectAttribute), false).Length > 0;
    }
}