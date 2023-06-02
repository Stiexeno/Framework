using System;
using System.Collections.Generic;
using Framework.Inspector;
using UnityEditor;
using UnityEngine;
using SF = UnityEngine.SerializeField;

namespace Framework.Core
{
	[CreateAssetMenu(fileName = "ConfigProvider", menuName = "Framework/Core/ConfigProvider")]
	public class ConfigProvider : ScriptableObject
	{
		public List<ConfigBase> configs = new List<ConfigBase>();

		public T GetConfig<T>() where T : ConfigBase
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

		[Button("Build")]
		private void Build()
		{
			foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				foreach (Type tp in assembly.GetTypes())
				{
					if (tp.BaseType == typeof(ConfigBase))
					{
						foreach (var config in configs)
						{
							if (config.GetType() == tp)
								return;
						}
					
						var path = "Assets/Configs/" + tp.Name + ".asset";
						var targetConfig = AssetDatabase.LoadAssetAtPath<ConfigBase>(path);
					
						if (targetConfig == null)
						{
							var instance = CreateInstance(tp);
							AssetDatabase.CreateAsset(instance, path);
							targetConfig = AssetDatabase.LoadAssetAtPath<ConfigBase>(path);
						}
					
						configs.Add(targetConfig);
					
						AssetDatabase.Refresh();
					}
				}
			}
		}
	}
}