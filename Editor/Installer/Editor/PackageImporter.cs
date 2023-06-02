using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using SF = UnityEngine.SerializeField;

namespace Framework.Installer
{
    [InitializeOnLoad]
    public class PackageImporter
    {
        private const string PACKAGES_PATH = "Assets/Installer/Packages/";
        private const string UNITY_PACKAGES_PATH = "Assets/Installer/UnityPackages/";

        private const string PACKAGE_KEY = "Package_Importer";

        static PackageImporter()
        {
            if (EditorPrefs.HasKey(PACKAGE_KEY))
            {
                AssetDatabase.onImportPackageItemsCompleted += OnItemsImported;
            }
        }

        public static void ImportPackage(PackageConfig config)
        {
            var packagePrefs = new PackagePrefs();
            packagePrefs.pacakgeConfigPath = AssetDatabase.GetAssetPath(config);
            EditorPrefs.SetString(PACKAGE_KEY, JsonUtility.ToJson(packagePrefs));

            AssetDatabase.ImportPackage(AssetDatabase.GetAssetPath(config.package), false);
        }

        public static void ExportPackage(string packageName, bool folderOnly)
        {
            string packageFilePath;

            if (folderOnly)
            {
                packageFilePath = EditorUtility.OpenFolderPanel("Select folder to export as Unity Package", "Assets", "");
            }
            else
            {
                packageFilePath = EditorUtility.OpenFilePanel("Select file to export as Unity Package", "Assets", "");
            }

            if (!string.IsNullOrEmpty(packageFilePath))
            {
                string[] filePaths = new string[1];

                if (folderOnly)
                {
                    filePaths = Directory.GetFiles(packageFilePath, "*", SearchOption.AllDirectories);
                }
                else
                {
                    filePaths[0] = packageFilePath;
                }

                for (var index = 0; index < filePaths.Length; index++)
                {
                    filePaths[index] = filePaths[index].Replace($"{Application.dataPath.Replace("Assets", "")}", "");
                }

                AssetDatabase.ExportPackage(filePaths, $"{UNITY_PACKAGES_PATH}{packageName}.unityPackage",
                    ExportPackageOptions.IncludeDependencies);

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                CreatePackageConfig(packageName);
            }
        }

        public static void DeletePackage(PackageConfig config)
        {
            foreach (var file in config.FileContents)
            {
                AssetDatabase.DeleteAsset(file);
            }

            InstallerRegistry.RemoveInstalledPackage(config.packageName);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static void CreatePackageConfig(string packageName)
        {
            var package = AssetDatabase.LoadAssetAtPath<DefaultAsset>($"{UNITY_PACKAGES_PATH}{packageName}.unityPackage");

            PackageConfig packageConfig = ScriptableObject.CreateInstance<PackageConfig>();
            packageConfig.packageName = packageName;
            packageConfig.package = package;

            AssetDatabase.CreateAsset(packageConfig, $"{PACKAGES_PATH}/Package_{packageName}.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            InstallerHelper.PingObject($"{PACKAGES_PATH}/Package_{packageName}.asset");
        }

        private static PackageConfig GetPackageConfig(string path)
        {
            var package = AssetDatabase.LoadAssetAtPath<PackageConfig>(path);
            return package;
        }

        private static void OnItemsImported(string[] obj)
        {
            var packagePrefs = JsonUtility.FromJson<PackagePrefs>(EditorPrefs.GetString(PACKAGE_KEY));
            var packageConfig = GetPackageConfig(packagePrefs.pacakgeConfigPath);

            packageConfig.FileContents = obj;

            InstallerRegistry.SaveInstalledPackage(packageConfig.packageName);

            AssetDatabase.onImportPackageItemsCompleted -= OnItemsImported;
            EditorPrefs.DeleteKey(PACKAGE_KEY);
        }
    }

    [Serializable]
    public class PackagePrefs
    {
        public string pacakgeConfigPath;
    }
}