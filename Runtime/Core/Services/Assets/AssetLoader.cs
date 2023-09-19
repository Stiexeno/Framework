using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Framework.Core
{
	public class AssetLoader : IAssets
	{
		private IInstantiator instantiator;

		public AssetLoader(IInstantiator instantiator)
		{
			this.instantiator = instantiator;
		}
		
		public T Instantiate<T>(GameObject prefab) where T : MonoBehaviour
		{
			var instantiated = instantiator.InstantiatePrefab(prefab);
			instantiated.name = instantiated.name.Replace("(Clone)", string.Empty);
			var transform = instantiated.transform;
			transform.rotation = Quaternion.identity;
			return instantiated.GetComponent<T>();
		}

		public T Instantiate<T>(string path) where T : MonoBehaviour
		{
			var instantiated = instantiator.InstantiatePrefab(GetPrefab<T>(path).gameObject);
			instantiated.name = instantiated.name.Replace("(Clone)", string.Empty);
			var transform = instantiated.transform;
			transform.rotation = Quaternion.identity;
			return instantiated.GetComponent<T>();
		}

		public T InstantiateType<T>(string path) where T : class
		{
			var instantiated = instantiator.InstantiatePrefab(GetPrefabByType<T>(path));
            
			var instance = instantiated as GameObject;
            
			instance.name = instance.name.Replace("(Clone)", string.Empty);
			var transform = instance.transform;
			transform.rotation = Quaternion.identity;

			if (typeof(T) == typeof(GameObject))
			{
				return instance as T;
			}
			
			return instance.GetComponent<T>();
		}
        
		public T Instantiate<T>(string path, Transform parent) where T : MonoBehaviour
		{
			var instantiated = Instantiate<T>(path);
			instantiated.transform.SetParent(parent);

			return instantiated;
		}

		public T Instantiate<T>(string path, Vector3 at) where T : MonoBehaviour
		{
			var instantiated = Instantiate<T>(path);
			instantiated.transform.position = at;

			return instantiated;
		}

		public T Instantiate<T>(GameObject prefab, Vector3 at, Transform parent = null) where T : MonoBehaviour
		{
			var instantiated = instantiator.InstantiatePrefab(prefab);
			instantiated.transform.SetParent(parent, true);
			instantiated.name = instantiated.name.Replace("(Clone)", string.Empty);
			var transform = instantiated.transform;
			transform.position = at;
			transform.rotation = Quaternion.identity;
			return instantiated.GetComponent<T>();
		}

		public T GetPrefab<T>(string path) where T : MonoBehaviour
		{
			var prefab = Resources.Load<T>(path);

			if (prefab == null)
			{
				Debug.LogError($"Couldn't load prefab from resources by path: {path}");
			}
            
			return prefab;
		}
		
		public T GetPrefabByType<T>(string path) where T : class
		{
			var prefab = Resources.Load(path);

			if (prefab == null)
			{
				Debug.LogError($"Couldn't load prefab from resources by path: {path}");
			}
            
			return prefab as T;
		}

		public T GetScriptableObject<T>(string path) where T : ScriptableObject
		{
			var prefab = Resources.Load<T>(path);

			if (prefab == null)
			{
				Debug.LogError($"Couldn't load scriptable object from path: {path}");
			}

			return prefab;
		}

		[Obsolete("Not implemented yet")]
		public T[] GetPrefabs<T>(string path) where T : MonoBehaviour
		{
			return null;
		}
	}
}