using Framework.Core;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using SceneManager = UnityEngine.SceneManagement.SceneManager;

namespace Framework.Installer
{
    public static class InstallerActions
    {
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
            "Visuals/Animations"
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
            var uiManager = new GameObject("UIManager");
            uiManager.layer = 5;
            uiManager.AddComponent<UIManager>();
            
            uiManager.AddComponent<EventSystem>();
            uiManager.AddComponent<StandaloneInputModule>();
            
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

            var uiCamera = new GameObject("UICamera");
            uiCamera.transform.SetParent(uiManager.transform);
            
            var cam = uiCamera.AddComponent<Camera>();
            
            cam.cullingMask = (1 << LayerMask.NameToLayer("UI"));
            
            cam.clearFlags = CameraClearFlags.Depth;
            
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = cam;
            
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        }

        public static void SetupApp()
        {
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
        
        public static void SetupManifest()
        {
            //string customModulesDir = Path.Combine(Application.dataPath, "Scripts/Core/");
            //
            //if (Directory.Exists(customModulesDir) == false)
            //{
            //    Directory.CreateDirectory(customModulesDir);
            //}
//
            //var srcManifestFullPath = Path.GetFullPath($"Assets/Installer/{App.MANIFEST_PATH}Boot.txt");
            //var manifestPath = $"Assets/Scripts/Core/{App.MANIFEST_PATH}Boot.cs";
            //var manifestFullPath = Path.GetFullPath(manifestPath);
//
            //File.WriteAllText(manifestFullPath, File.ReadAllText(srcManifestFullPath));
        }
    }
}