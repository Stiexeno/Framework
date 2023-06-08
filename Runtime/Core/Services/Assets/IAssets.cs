using UnityEngine;
using SF = UnityEngine.SerializeField;

namespace Framework.Core
{
	public interface IAssets
	{
		T Instantiate<T>(GameObject prefab) where T : MonoBehaviour;
		T Instantiate<T>(string path) where T : MonoBehaviour;
		T Instantiate<T>(string path, Transform parent) where T : MonoBehaviour;
		T Instantiate<T>(string path, Vector3 at) where T : MonoBehaviour;
		T Instantiate<T>(GameObject prefab, Vector3 at, Transform parent = null) where T : MonoBehaviour;
		T GetPrefab<T>(string path) where T : MonoBehaviour;
		T GetScriptableObject<T>(string path) where T : ScriptableObject;
		T[] GetPrefabs<T>(string path) where T : MonoBehaviour;
	}
}