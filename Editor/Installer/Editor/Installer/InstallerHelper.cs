using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;

namespace Framework.Installer
{
    public static class InstallerHelper
    {
        public static IEnumerable<FieldInfo> GetConstants(Type type)
        {
            var fieldInfos = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            return fieldInfos.Where(fi => fi.IsLiteral && !fi.IsInitOnly);
        }

        public static IEnumerable<string> GetConstantsValues(Type type)
        {
            var fieldInfos = GetConstants(type);
            return fieldInfos.Select(fi => fi.GetRawConstantValue() as string);
        }

        public static void PingObject(string path)
        {
            if (path[path.Length - 1] == '/')
                path = path.Substring(0, path.Length - 1);
            
            UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath(path, typeof(UnityEngine.Object));
            
            Selection.activeObject = obj;
            
            EditorGUIUtility.PingObject(obj);
        }
    }
}