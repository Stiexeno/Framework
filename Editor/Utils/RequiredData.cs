using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Framework.Editor
{
	public class RequiredData : ScriptableObject
	{
		public enum IconType { Error, Warning, Info }

		private static RequiredData data;

		public bool showIcon = true;
		public bool showBorder = true;
		public IconType iconType = IconType.Error;
		public Color borderColor = Color.red;

		[SerializeField, HideInInspector] private List<string> disabledKeys = new List<string>();

		private const string LOCAL_PATH = "Assets/Resources";

		public static RequiredData Data
		{
			get
			{
				if (data == null)
				{
					data = (RequiredData) AssetDatabase.LoadAssetAtPath($"{LOCAL_PATH}/RequiredData.asset",
						typeof(RequiredData));

					if (data == null)
					{
						if (AssetDatabase.IsValidFolder(LOCAL_PATH) == false)
						{
							AssetDatabase.CreateFolder("Assets", "Resources");
						}

						RequiredData dataFile = CreateInstance<RequiredData>();
						string path = $"{LOCAL_PATH}/RequiredData.asset";
						AssetDatabase.CreateAsset(dataFile, path);
						AssetDatabase.SaveAssets();
						AssetDatabase.Refresh();

						data = dataFile;
						return data;
					}
				}

				return data;
			}
		}

		public string GetIconType()
		{
			return iconType switch
			{
				IconType.Error => "d_console.erroricon",
				IconType.Warning => "d_console.warnicon",
				IconType.Info => "d_console.infoicon",
				_ => ""
			};
		}

		public bool TryGetKey(string key)
		{
			return disabledKeys.Contains(key);
		}
		
		public void AddKey(string key)
		{
			if (TryGetKey(key) == false)
			{
				disabledKeys.Add(key);
			}
		}

		public void RemoveKey(string key)
		{
			disabledKeys.Remove(key);
		}
	}
}