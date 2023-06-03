using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Framework.Installer
{
    public static class InstallerRegistry
    {
        public static readonly string PACKAGE_REGISTRY = Path.Combine(Application.dataPath, "Installer");
        
        private const string signature = "com.framework.";
        
        private static List<string> installedPackages = new List<string>();
        
        public static bool IsInstalled(string name)
        {
            if (installedPackages.Count == 0)
            {
                var packageRegistry = LoadRawData();
                installedPackages = packageRegistry.installedPackages;
            }
            
            string key = $"{signature}{name.Replace(" ", "").ToLower()}";
            return installedPackages.Contains(key);
        }
        
        public static void SaveInstalledPackage(string name)
        {
            installedPackages.Clear();
            
            string key = $"{signature}{name.Replace(" ", "").ToLower()}";

            var packageRegistry = LoadRawData();

            if (packageRegistry.installedPackages.Contains(key) == false)
            {
                packageRegistry.installedPackages.Add(key);
            }

            string json = JsonUtility.ToJson(packageRegistry);
            var filePath = Path.Combine(PACKAGE_REGISTRY, "PackageRegsitry" + ".json");
            File.WriteAllText(filePath, json);
        }

        public static void RemoveInstalledPackage(string name)
        {
            installedPackages.Clear();
            
            string key = $"{signature}{name.Replace(" ", "").ToLower()}";

            var packageRegistry = LoadRawData();

            if (packageRegistry.installedPackages.Contains(key))
            {
                packageRegistry.installedPackages.Remove(key);
            }

            string json = JsonUtility.ToJson(packageRegistry);
            var filePath = Path.Combine(PACKAGE_REGISTRY, "PackageRegsitry" + ".json");
            File.WriteAllText(filePath, json);
        }

        private static PackageRegistry LoadRawData()
        {
            if (Directory.Exists(PACKAGE_REGISTRY) == false)
            {
                Directory.CreateDirectory(PACKAGE_REGISTRY);
            }

            var filePath = Path.Combine(PACKAGE_REGISTRY, "PackageRegsitry" + ".json");

            string json;

            if (File.Exists(filePath))
            {
                json = File.ReadAllText(filePath);
            }
            else
            {
                return new PackageRegistry();
            }

            PackageRegistry data = JsonUtility.FromJson<PackageRegistry>(json);

            if (data != null)
            {
                return data;
            }

            return new PackageRegistry();
        }

        [Serializable]
        public class PackageRegistry
        {
            public List<string> installedPackages = new List<string>();
        }
    }
}