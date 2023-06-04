using System;
using System.Collections.Generic;
using Framework.Core;
using Framework.Utils;
using UnityEditor;
using UnityEngine;

namespace Framework.Editor
{
	[CustomEditor(typeof(SceneContext))]
	public class SceneContextEditor : UnityEditor.Editor
	{
		private enum Tab { Context, Config, Assets, Settings }

		private SceneContext sceneContext;
		private ConfigSettings configSettings;
		private AssetSettings assetsSettings;

		private Tab tab;

		private MonoScript assetsScript;

		public override void OnInspectorGUI()
		{
			DrawNavigation();

			if (Application.isPlaying)
				GUI.enabled = false;

			if (tab == Tab.Context)
			{
				DrawDefaultInspector();
				DrawRefreshButton();
			}
			else if (tab == Tab.Config)
			{
				DrawConfig();
			}
			else if(tab == Tab.Assets)
			{
				DrawAssets();
			}
			else
			{
				DrawSettings();
			}

			if (Application.isPlaying)
				GUI.enabled = true;

			DrawFooter();
		}

		private void DrawSettings()
		{
			EditorGUILayout.Toggle("Refresh on play", true);
		}

		private void DrawConfig()
		{
			if (configSettings == null)
				configSettings = ConfigSettings.Settings;
			
			var editor = CreateEditor(configSettings);
			editor.OnInspectorGUI();

			EditorGUILayout.HelpBox("AbstractConfigs will be automatically created as ScriptableObjects instances and saved to ConfigSettings", MessageType.Info);
			if (GUILayout.Button("Refresh"))
			{
				ConfigSettings.Settings.GenerateConfigs();
			}
		}

		private void DrawAssets()
		{
			if (assetsSettings == null)
				assetsSettings = AssetSettings.Settings;

			var editor = CreateEditor(assetsSettings);
			editor.OnInspectorGUI();

			if (assetsScript != null)
			{
				GUILayout.Space(5f);
				GUI.enabled = false;
				EditorGUILayout.ObjectField(assetsScript, typeof(MonoScript), false);
				
				if (Application.isPlaying == false)
					GUI.enabled = true;
				
				EditorGUILayout.HelpBox("Marked Assets will be generated to script as constants", MessageType.Info);
			}

			if (GUILayout.Button("Refresh"))
			{
				AssetSettings.Settings.GenerateAssetsScript(() =>
				{
					string outputPath = "Assets/Scripts/Generated/AssetsPath.cs";
					assetsScript = AssetDatabase.LoadAssetAtPath<MonoScript>(outputPath);
				});
			}
		}

		private void DrawRefreshButton()
		{
			EditorGUILayout.HelpBox("Press Refresh button to create instance of missing installers", MessageType.Info);
			if (GUILayout.Button("Refresh"))
			{
				Context.GenerateInstallers();
			}
		}

		private void DrawFooter()
		{
			if (Application.isPlaying)
			{
				EditorGUILayout.LabelField($"Scene load time: {Context.TimeTookToInstall:0.00}ms", EditorStyles.boldLabel);

				EditorHelper.DrawHorizontalLine(new Color(0.1f, 0.1f, 0.1f), 1, 2);

				GUILayout.Space(15f);
				GUILayout.Label($"Registered Services: {sceneContext.DiContainer.Container.Count}", EditorStyles.boldLabel);

				EditorGUILayout.BeginVertical("HelpBox");

				foreach (var container in sceneContext.DiContainer.Container)
				{
					if (container.Value.Instance is AbstractConfig)
						continue;

					var interfaces = "";

					if (container.Value.Interfaces != null)
					{
						foreach (var iInterface in container.Value.Interfaces)
						{
							if (iInterface == null)
								continue;

							var splitInterfaces = iInterface.ToString().Split('.');
							interfaces += $"{splitInterfaces[^1]} ";
						}
					}

					var type = container.Value.ContractType.ToString().Replace("Framework.", "").Replace("Core.", "");

					var style = new GUIStyle();
					style.richText = true;
					style.fontSize = 12;
					style.normal.textColor = new Color(0.74f, 0.74f, 0.74f);
					style.margin = new RectOffset(10, 10, 5, 5);
					GUILayout.Label($"• <b>{type}</b> {interfaces.SetColor(new Color(0.05f, 0.83f, 0.55f))}", style);
				}

				EditorGUILayout.EndVertical();
			}
		}

		private void DrawNavigation()
		{
			var tapRect = EditorGUILayout.GetControlRect();

			tapRect.x = 0;
			tapRect.y -= 5;
			tapRect.width = EditorGUIUtility.currentViewWidth;

			// Create a custom GUIStyle for the active button
			var activeButtonStyle = new GUIStyle(EditorStyles.toolbarButton);
			activeButtonStyle.normal.background = EditorHelper.Texture2DColor(new Color(0.17f, 0.36f, 0.53f));
			activeButtonStyle.imagePosition = ImagePosition.ImageLeft;

			GUI.Box(tapRect, "", EditorStyles.toolbar);

			tapRect.width /= 3;
			tapRect.width -= 15;
			if (GUI.Button(tapRect, EditorHelper.Icon("Context", "d_Navigation"),
				    tab == Tab.Context ? activeButtonStyle : EditorStyles.toolbarButton))
			{
				tab = Tab.Context;
			}

			tapRect.x += tapRect.width;
			if (GUI.Button(tapRect, EditorHelper.Icon("Configs", "d_ScriptableObject Icon"),
				    tab == Tab.Config ? activeButtonStyle : EditorStyles.toolbarButton))
			{
				tab = Tab.Config;
			}

			tapRect.x += tapRect.width;
			if (GUI.Button(tapRect, EditorHelper.Icon("Assets", "PreviewPackageInUse@2x"),
				    tab == Tab.Assets ? activeButtonStyle : EditorStyles.toolbarButton))
			{
				tab = Tab.Assets;
			}

			tapRect.x += tapRect.width;
			tapRect.width = 45;
			if (GUI.Button(tapRect, EditorHelper.Icon("", "d_Settings@2x"),
				    tab == Tab.Settings ? activeButtonStyle : EditorStyles.toolbarButton))
			{
				tab = Tab.Settings;
			}
			
			
			tapRect.width = EditorGUIUtility.currentViewWidth;
			tapRect.x = 0;
			tapRect.height = 1;
			EditorGUI.DrawRect(tapRect, new Color(0.1f, 0.1f, 0.1f));
		}

		private void OnEnable()
		{
			sceneContext = (SceneContext)target;

			var comp = sceneContext.GetComponent<Transform>();
			comp.hideFlags = HideFlags.HideInInspector;

			if (assetsSettings == null)
				assetsSettings = AssetSettings.Settings;

			if (configSettings == null)
				configSettings = ConfigSettings.Settings;

			string outputPath = "Assets/Scripts/Generated/AssetsPath.cs";
			assetsScript = AssetDatabase.LoadAssetAtPath<MonoScript>(outputPath);
		}
	}
}