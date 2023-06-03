using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Framework.Core;
using Framework.Editor;
using Framework.Utils;
using UnityEditor;
using UnityEngine;

namespace Framework.ContextEditor
{
	[CustomEditor(typeof(SceneContext))]
	public class SceneContextEditor : UnityEditor.Editor
	{
		private enum Tab {Context, Config, Other}
		
		private SceneContext sceneContext;
		private ConfigProvider configProvider;
		
		private Tab tab;

		private const string INSTALLERS_PATH = "Configs/Installers/";

		private static readonly HashSet<Type> excludedInstallers = new HashSet<Type>
		{
			typeof(BootstrapInstaller)
		};

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
			else if(tab == Tab.Config)
			{
				DrawConfig();
			}
			else
			{
				EditorGUILayout.LabelField("Not implemented yet");
			}

			if (Application.isPlaying)
				GUI.enabled = true;
			DrawFooter();
		}

		private void DrawConfig()
		{
			var editor = CreateEditor(configProvider);
			editor.OnInspectorGUI();
		}

		private void DrawRefreshButton()
		{
			if (GUILayout.Button("Refresh"))
			{
				GenerateInstallers();
			}

			EditorGUILayout.HelpBox("Press Refresh button to create instance of missing installers", MessageType.Info);
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
					if (container.Value.Instance is ConfigBase)
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
			tapRect.y -= 4;
			tapRect.width = EditorGUIUtility.currentViewWidth;
			
			// Create a custom GUIStyle for the active button
			var activeButtonStyle = new GUIStyle(EditorStyles.toolbarButton);
			activeButtonStyle.normal.background = EditorHelper.Texture2DColor(new Color(0.17f, 0.36f, 0.53f));
			activeButtonStyle.imagePosition = ImagePosition.ImageLeft;
			
			GUI.Box(tapRect, "", EditorStyles.toolbar);

			tapRect.width /= 3;
			if (GUI.Button(tapRect, EditorHelper.Icon("Context", "d_Navigation"), tab == Tab.Context ? activeButtonStyle : EditorStyles.toolbarButton))
			{
				tab = Tab.Context;
			}
			
			tapRect.x += tapRect.width;
			if (GUI.Button(tapRect, EditorHelper.Icon("Configs", "d_ScriptableObject Icon"), tab == Tab.Config ? activeButtonStyle : EditorStyles.toolbarButton))
			{
				tab = Tab.Config;
			}
			
			tapRect.x += tapRect.width;
			if (GUI.Button(tapRect, "Other", tab == Tab.Other ? activeButtonStyle : EditorStyles.toolbarButton))
			{
				tab = Tab.Other;
			}
		}
		
		private void GenerateInstallers()
		{
			var installerTypes = AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(assembly => assembly.GetTypes())
				.Where(x => !x.IsAbstract)
				.Where(type => type.BaseType == typeof(AbstractInstaller))
				.Where(x => !excludedInstallers.Contains(x))
				.ToArray();

			var configsPath = Path.Combine(Application.dataPath, "Configs");
			var installersPath = Path.Combine(Application.dataPath, INSTALLERS_PATH);

			if (Directory.Exists(configsPath) == false)
			{
				Directory.CreateDirectory(configsPath);
			}

			if (Directory.Exists(installersPath) == false)
			{
				Directory.CreateDirectory(installersPath);
			}

			AssetDatabase.Refresh();

			foreach (var installer in installerTypes)
			{
				var installerName = installer.Name;
				var installerPath = "Assets/" + INSTALLERS_PATH + installerName + ".asset";
				var installerAsset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(installerPath);

				if (installerAsset == null)
				{
					var installerAssets = CreateInstance(installer);
					AssetDatabase.CreateAsset(installerAssets, installerPath);
					AssetDatabase.SaveAssets();
					AssetDatabase.Refresh();
				}
			}
		}

		private void CollectConfig()
		{
			configProvider = Resources.Load<ConfigProvider>("ConfigProvider");
			
			if (configProvider == null)
			{
				var configInstance = CreateInstance<ConfigProvider>();
				AssetDatabase.CreateAsset(configInstance, "Assets/Resources/ConfigProvider.asset");
				
				configProvider = Resources.Load<ConfigProvider>("ConfigProvider");
			}
		}

		private void OnEnable()
		{
			sceneContext = (SceneContext)target;

			var comp = sceneContext.GetComponent<Transform>();
			comp.hideFlags = HideFlags.HideInInspector;

			CollectConfig();
			
			
		}
	}
}