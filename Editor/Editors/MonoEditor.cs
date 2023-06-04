using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Framework.Inspector;
using UnityEditor;
using UnityEngine;

namespace Framework.Editor
{
    [CustomEditor(typeof (MonoBehaviour), true)]
    public class MonoEditor : UnityEditor.Editor
    {
        private MonoBehaviour targetBehaviour;
        private MemberInfo[] behaviourMethods;

        private List<SerializedProperty> foundSerialisedProperties;

        // Editor
        
        private void OnEnable()
        {
            targetBehaviour = target as MonoBehaviour;
            if (targetBehaviour)
            {
                behaviourMethods = GetMonoMethods();
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            CustomInspector();
        }

        // MonoEditor
        
        private void CustomInspector()
        {
            if (!targetBehaviour)
            {
                EditorGUILayout.HelpBox("MonoBehaviour is not properly loaded, try selecting the GameObject again.",MessageType.Error);
                return;
            }

            ButtonInspector();
            
        }

        // Buttons

        private void ButtonInspector()
        {
            IEnumerable<MemberInfo> methods = GetMethodsWithAttribute(typeof(ButtonAttribute));
            
            EditorGUILayout.Space(10);
        
            // Draw buttons
            foreach (var memberInfo in methods)
            {
                DrawButton(memberInfo as MethodInfo);
            }
        }

        private void DrawButton(MethodInfo thisMethod)
        {
            var attribute         = thisMethod.GetCustomAttribute<ButtonAttribute>();
            var buttonName = string.IsNullOrEmpty(attribute.label) ? $"{thisMethod.Name}" : attribute.label;
            var ifPlaying         = thisMethod.GetCustomAttribute<IfPlaying>();

            var showButton = true;
            
            if (ifPlaying != null)
            {
                showButton = (Application.isPlaying && ifPlaying.value) || (!Application.isPlaying && !ifPlaying.value);
            }
            
            if (showButton)
            {
                Color oldGUIColor = GUI.color;
                GUI.color = Color.white;
                if (GUILayout.Button(buttonName,GUILayout.Height(attribute.height)))
                {
                    thisMethod.Invoke(targetBehaviour, null);
                }
                GUI.color = oldGUIColor;
            }
        }
        
        // Helpers
        
        private MemberInfo[] GetMonoMethods()
        {
            return targetBehaviour.GetType()
                .GetMembers(BindingFlags.Instance | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public |
                            BindingFlags.NonPublic);
        }

        private IEnumerable<MemberInfo> GetMethodsWithAttribute(Type thisAttribute)
        {
            behaviourMethods ??= GetMonoMethods();

            return behaviourMethods.Where(o => Attribute.IsDefined(o, thisAttribute));
        }
    }
}