using System;
using System.IO;
using Framework.Utils;
using UnityEditor;
using UnityEngine;
using SF = UnityEngine.SerializeField;

namespace Framework.Installer
{
    [CreateAssetMenu(fileName = "Framework", menuName = "Framework/Installer/PackageConfig")]
    public class PackageConfig : ScriptableObject
    {
        public enum IconKey {Script, Animation, VFX}
        
        public DefaultAsset package;

        public string packageName;
        public string description;
        public Dropdown<InstallerCategories> category;
        public IconKey icon;

        [field:SF, HideInInspector] public string[] FileContents { get; set; }
        
        public DateTime Version => File.GetLastWriteTime(AssetDatabase.GetAssetPath(this));

        public string GetIconKey()
        {
            return icon switch
            {
                IconKey.Script => "cs Script Icon",
                IconKey.Animation => "d_AnimationClip Icon",
                IconKey.VFX => "d_ParticleSystem Icon",
                _ => ""
            };
        }

#if UNITY_EDITOR
        private void AddDefineSymbol(BuildTargetGroup group, string symbol)
        {
            var symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(group);
            
            var fullDefine = symbol;
            
            if (symbols.Contains(';') || (symbols.Length > 0 && symbols.Contains(';') == false))
            {
                fullDefine = ";" + fullDefine;
            }
            
            if (symbols.Contains(fullDefine))
                return;
            
            symbols += fullDefine;
            
            PlayerSettings.SetScriptingDefineSymbolsForGroup(group, symbols);
        }
        
        private void RemoveDefineSymbol(BuildTargetGroup group, string symbol)
        {
            var symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(group);

            var fullDefine = symbol + ";";

            if (symbols.Contains(fullDefine))
            {
                symbols = symbols.Replace(fullDefine, "");
            }
            else if (symbols.Contains(symbol))
            {
                symbols = symbols.Replace(symbol, "");
            }
  
            PlayerSettings.SetScriptingDefineSymbolsForGroup(group, symbols);
        }
        #endif
    }
}