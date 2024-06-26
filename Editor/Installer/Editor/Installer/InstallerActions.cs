using System.IO;
using System.Linq;
using Framework.Core;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using SceneManager = UnityEngine.SceneManagement.SceneManager;

namespace Framework.Installer
{
	public static class InstallerActions
	{
		private static bool setupInProgress;

		private static readonly string[] folders =
		{
			"Scripts",
			"Editor",
			"Resources",
			"Configs",
			"Scenes",
			"Prefabs",
			"Visuals",
			"Visuals/Materials",
			"Visuals/Textures",
			"Visuals/Assets",
			"Visuals/Models",
			"Visuals/Animations",
			"Visuals/ThirdParty",
			"Visuals/Shaders"
		};

		public static void CreateFolders()
		{
			foreach (var folder in folders)
			{
				var folderPath = folder.Split("/");
				var path = "Assets";
				if (folderPath.Length > 1)
				{
					path += $"/{folderPath[0]}";
				}

				if (AssetDatabase.IsValidFolder($"Assets/{folder}") == false)
				{
					AssetDatabase.CreateFolder(path, folderPath[^1]);
				}
			}
		}

		public static void CreateUIManager(bool vertical)
		{
			var foundUiManager = GameObject.FindObjectOfType<UIManager>();

			if (foundUiManager != null)
			{
				Object.DestroyImmediate(foundUiManager.gameObject);
			}

			var uiManager = new GameObject("UIManager");
			uiManager.layer = 5;
			uiManager.AddComponent<UIManager>();

			uiManager.AddComponent<EventSystem>();
			uiManager.AddComponent<StandaloneInputModule>();

			var uiCamera = new GameObject("UICamera");
			uiCamera.transform.SetParent(uiManager.transform);
			uiCamera.layer = LayerMask.NameToLayer("UI");

			var cam = uiCamera.AddComponent<Camera>();

			cam.cullingMask = (1 << LayerMask.NameToLayer("UI"));
			cam.clearFlags = CameraClearFlags.Depth;
			cam.orthographic = true;

			var canvasObject = new GameObject("UICanvas");

			var canvas = canvasObject.AddComponent<Canvas>();
			canvas.planeDistance = 10;

			canvasObject.AddComponent<GraphicRaycaster>();
			canvasObject.transform.SetParent(uiManager.transform);
			canvasObject.layer = 5;

			var scalar = canvasObject.AddComponent<CanvasScaler>();
			scalar.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
			scalar.matchWidthOrHeight = 1f;
			scalar.referenceResolution = vertical ? new Vector2(1440, 2960) : new Vector2(2960, 1440);

			canvas.renderMode = RenderMode.ScreenSpaceCamera;
			canvas.worldCamera = cam;

			EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
		}

		public static void Setup()
		{
			EditorUtility.DisplayProgressBar("Installer", $"Creating SceneContext to current scene", 0);

			var sceneContext = GameObject.FindObjectOfType<SceneContext>();

			if (sceneContext == null)
			{
				var sceneContextInstace = new GameObject("SceneContext");
				sceneContext = sceneContextInstace.AddComponent<SceneContext>();

				sceneContext.transform.SetAsFirstSibling();
			}

			var cameras = GameObject.FindObjectsOfType<Camera>();
			var lighting = GameObject.FindObjectOfType<Light>();

			var camera = cameras.FirstOrDefault(x => x.gameObject.layer != LayerMask.NameToLayer("UI"));

			camera.transform.SetParent(sceneContext.transform);
			lighting.transform.SetParent(sceneContext.transform);

			SetupBootstrap();

			EditorUtility.ClearProgressBar();

			//EditorUtility.DisplayProgressBar("Installer", $"Cloning {App.MANIFEST_PATH}.cs", 0);
			//
			//string customModulesDir = Path.Combine(Application.dataPath, "Scripts/Core/");
			//
			//if (Directory.Exists(customModulesDir) == false)
			//{
			//    Directory.CreateDirectory(customModulesDir);
			//}
//
			//var srcManifestFullPath = Path.GetFullPath($"Assets/Installer/{App.MANIFEST_PATH}.txt");
			//var manifestPath = $"Assets/Scripts/Core/{App.MANIFEST_PATH}.cs";
			//var manifestFullPath = Path.GetFullPath(manifestPath);
//
			//File.WriteAllText(manifestFullPath, File.ReadAllText(srcManifestFullPath));
//
			//SetupManifest();
			////SetupGitIgnore();
			//
			//AssetDatabase.ImportAsset(manifestPath, ImportAssetOptions.ForceSynchronousImport | ImportAssetOptions.ForceUpdate);
			//AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport | ImportAssetOptions.ForceUpdate);
//
			//EditorUtility.DisplayProgressBar("Installer", $"Done...", 1f);
			//AssetDatabase.Refresh();
			//AssetDatabase.SaveAssets();
			//
			//CompilationPipeline.RequestScriptCompilation();
			//
			//CompilationPipeline.compilationFinished -= CompileFinished;
			//CompilationPipeline.compilationFinished += CompileFinished;
		}

		private static void SetupGitIgnore()
		{
			var srcGitIgnorePath = Path.GetFullPath($"Packages/{CoreConstants.PACKAGE_NAME}/Editor/Installer/GitIgnore.txt");
			var gitIgnorePath = Path.GetFullPath(".gitignore");
			File.WriteAllText(gitIgnorePath, File.ReadAllText(srcGitIgnorePath));
		}

		private static void SetupBootstrap()
		{
			string scriptDir = Path.Combine(Application.dataPath, "Scripts/Installers/");

			if (Directory.Exists(scriptDir) == false)
			{
				Directory.CreateDirectory(scriptDir);
			}

			var sourceBootstrapFullPath = Path.GetFullPath($"Packages/{CoreConstants.PACKAGE_NAME}/Editor/Installer/BootstrapInstaller.txt");
			var bootstrapPath = $"Assets/Scripts/Installers/BootstrapInstaller.cs";
			var bootstrapFullPath = Path.GetFullPath(bootstrapPath);

			File.WriteAllText(bootstrapFullPath, File.ReadAllText(sourceBootstrapFullPath));

			SetupGitIgnore();

			AssetDatabase.ImportAsset(bootstrapPath, ImportAssetOptions.ForceSynchronousImport | ImportAssetOptions.ForceUpdate);
			AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport | ImportAssetOptions.ForceUpdate);

			EditorPrefs.SetBool("RequestBootstrap", true);

			CompilationPipeline.RequestScriptCompilation();
		}
	}
}