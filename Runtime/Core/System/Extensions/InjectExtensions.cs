using System;
using System.Collections.Generic;
using System.Reflection;
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
        
        public static IEnumerable<MethodInfo> GetInjectableMethods(Type type)
        {
            MethodInfo[] methods = GetAllMethods(type);
            Array.Reverse(methods);
            
            return Array.FindAll(methods, HasInjectMethods);
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