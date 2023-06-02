using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Framework.ContextEditor;
using Framework.Editor;
using UnityEditor;
using UnityEngine;

namespace Framework.Core
{
	[CustomPropertyDrawer(typeof(InstallerInfo))]
	public class InstallerInfoDrawer : PropertyDrawer
	{
		private List<InstallerInfo> installerInfos;
		
		private static readonly HashSet<Type> excludedInstallers = new HashSet<Type>
		{
			typeof(BootstrapInstaller)
		};

		private void Initialize()
		{
			if (installerInfos != null)
				return;

			installerInfos = new List<InstallerInfo>();
			
			var installerTypes = AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(assembly => assembly.GetTypes())
				.Where(x => !x.IsAbstract)
				.Where(type => type.GetInterface(nameof(IBindingInstaller)) != null)
				.Where(x => !excludedInstallers.Contains(x))
				.ToArray();

			// TODO: Add support for multiple assemblies
			foreach (var intaller in installerTypes)
			{
				var installerInfo = new InstallerInfo(intaller.Name, intaller.ToString(), GetScriptPath(intaller), "Assembly-CSharp");
				installerInfos.Add(installerInfo);
			}
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			Initialize();

			var initialRect = position;
			EditorGUI.BeginProperty(position, label, property);

			position.height = 21;
			position.width -= 23;
			position.y += 1;

			var nameProperty = property.FindPropertyRelative("name");
			var pathProperty = property.FindPropertyRelative("path");

			var indent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;

			GUIStyle objectFieldStyle = GUI.skin.FindStyle("ObjectField");

			GUI.Label(position, "", objectFieldStyle);

			var iconRect = position;
			iconRect.width = 18;
			iconRect.x += 2;

			GUI.Label(iconRect, EditorHelper.CreateNamedIcon("", "d_TextScriptImporter Icon"));

			iconRect.x += 12;
			iconRect.width = position.width - 23;
			GUI.Label(iconRect, nameProperty.stringValue,
				ContextStyle.TextStyle(12, new Color(0.74f, 0.74f, 0.74f), fontStyle: FontStyle.Normal, anchor: TextAnchor.MiddleLeft));

			position.x = position.width + 48;
			position.height = 21;
			position.width = 21;

			// Draw the button with a circle background
			if (GUI.Button(position, "", objectFieldStyle))
			{
				OpenContext(property);
			}

			position.height = 17;
			position.y += 2;
			position.x += 1;

			GUI.Label(position, EditorHelper.CreateNamedIcon("", "Record Off"));

			initialRect.width -= 23;
			
			if (GUI.Button(initialRect, "", GUIStyle.none))
			{
				Process.Start("rider", pathProperty.stringValue);
			}
			
			EditorGUI.indentLevel = indent;

			EditorGUI.EndProperty();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return 23;
		}

		private void OpenContext(SerializedProperty property)
		{
			var nameProperty = property.FindPropertyRelative("name");
			var typeProperty = property.FindPropertyRelative("typeName");
			var pathProperty = property.FindPropertyRelative("path");
			var assemblyProperty = property.FindPropertyRelative("assembly");

			var installerMenu = new GenericMenu();
			foreach (var installer in installerInfos)
			{
				installerMenu.AddItem(new GUIContent($"{installer.name}"), false, () =>
				{
					nameProperty.stringValue = installer.name;
					pathProperty.stringValue = installer.path;
					typeProperty.stringValue = installer.typeName;
					assemblyProperty.stringValue = installer.assembly;

					property.serializedObject.ApplyModifiedProperties();
				});
			}

			installerMenu.ShowAsContext();
		}

		private static string GetScriptPath(Type type)
		{
			string scriptName = type.Name;
			string[] guids = AssetDatabase.FindAssets(scriptName + " t:MonoScript");
			if (guids.Length > 0)
			{
				string assetPath = AssetDatabase.GUIDToAssetPath(guids[0]);
				return assetPath;
			}

			return null;
		}
	}
}