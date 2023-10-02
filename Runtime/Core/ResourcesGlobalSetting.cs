using UnityEngine;

namespace Framework.Core
{
	public class ResourcesGlobalSetting<T> : ScriptableObject where T : ScriptableObject
	{
		private static T active;

		protected static T Active
		{
			get
			{
				if (active == null)
				{
					var globalSetting = Resources.Load<T>(typeof(T).Name);

					if (globalSetting == null)
					{
						Debug.LogWarning($"Failed to load {typeof(T).Name}");
						var instance = CreateInstance<T>();

						active = instance;
					}
					else
					{
						active = globalSetting;
					}
				}

				return active;
			}
		}
	}
}