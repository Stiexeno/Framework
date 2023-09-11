#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

namespace Framework.Core
{
	public class ResourcesGlobalSetting<T> : ScriptableObject where T : ScriptableObject
	{
		private const string GLOBAL_SETTING_PATH = "GlobalSettings/{0}";
			
		private static T active;
		
		protected static T Active
		{
			get
			{
				if (active == null)
				{
					var targetPath = string.Format(GLOBAL_SETTING_PATH, typeof(T).Name);
					var globalSetting = Resources.Load<T>(targetPath);

					if (globalSetting == null)
					{
						var instance = CreateInstance<T>();
						#if UNITY_EDITOR
						try
						{
							AssetDatabase.CreateAsset(instance, $"Assets/Resources/{targetPath}.asset");
							AssetDatabase.Refresh();
						} catch (System.Exception exception)
						{
							Debug.LogError(exception);
						}
						#endif

						active = instance;
					} else
					{
						active = globalSetting;
					}
				}
				
				return active; 
			}
		}
	}
}