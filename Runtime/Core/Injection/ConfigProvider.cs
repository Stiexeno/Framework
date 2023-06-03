using System;
using System.Collections.Generic;
using Framework.Inspector;
using UnityEditor;
using UnityEngine;

namespace Framework.Core
{
	[CreateAssetMenu(fileName = "ConfigProvider", menuName = "Framework/Core/ConfigProvider")]
	public class ConfigProvider : ScriptableObject
	{
		public List<AbstractConfig> configs = new List<AbstractConfig>();

		public T GetConfig<T>() where T : AbstractConfig
		{
			foreach (var config in configs)
			{
				if (config is T)
				{
					return (T)config;
				}
			}

			return null;
		}

		[Button("Refresh")]
		private void Refresh()
		{
			foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				foreach (Type tp in assembly.GetTypes())
				{
					if (tp.BaseType == typeof(AbstractConfig))
					{
						if (configs.Contains(x => x.GetType() == tp))
							continue;

						var path = "Assets/Configs/" + tp.Name + ".asset";
						var targetConfig = AssetDatabase.LoadAssetAtPath<AbstractConfig>(path);

						if (targetConfig == null)
						{
							var instance = CreateInstance(tp);
							AssetDatabase.CreateAsset(instance, path);
							targetConfig = AssetDatabase.LoadAssetAtPath<AbstractConfig>(path);
						}

						configs.Add(targetConfig);

						AssetDatabase.Refresh();
					}
				}
			}
		}
	}
}