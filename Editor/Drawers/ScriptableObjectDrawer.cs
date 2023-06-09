using System;
using Framework.Editor;
using UnityEditor;
using UnityEngine;

namespace Framework
{
    [CustomPropertyDrawer(typeof(ScriptableObject), true)]
    public class ScriptableObjectDrawer : RequiredDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            var propertyRect = position;
            
            if (property.objectReferenceValue == null)
            {
                propertyRect = new Rect(position.x, position.y, position.width - 27, position.height);
                var buttonRect = new Rect(position.x + position.width - 23, position.y, position.width - propertyRect.width - 4, position.height);
                
                if (GUI.Button(buttonRect, EditorHelper.Icon("", "d_Toolbar Plus")))
                {
                    CreateScriptableObject(property);
                }
            }
            
            EditorGUI.PropertyField(propertyRect, property, GUIContent.none);
            OnRequired(propertyRect, property, label);
            
            // Set indent back to what it was
            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }

        private void CreateScriptableObject(SerializedProperty property)
        {
            var type = GetScriptableObjectType();
            if (type != null)
            {
                string path = EditorUtility.SaveFilePanelInProject("Save ScriptableObject", 
                    $"{type.Name}", "asset", 
                    "Please enter a name for the ScriptableObject");

                if (!string.IsNullOrEmpty(path))
                {
                    ScriptableObject scriptableObject = ScriptableObject.CreateInstance(type);
                    AssetDatabase.CreateAsset(scriptableObject, path);
                    AssetDatabase.Refresh();
                    AssetDatabase.SaveAssets();
                    
                    property.objectReferenceValue = AssetDatabase.LoadAssetAtPath(path, type);
                    property.serializedObject.ApplyModifiedProperties();
                    
                    EditorUtility.SetDirty(property.serializedObject.targetObject);
                }   
                
                GUIUtility.ExitGUI();
            }
        }
        
        private Type GetScriptableObjectType()
        {
            Type scriptableObjectType = fieldInfo.FieldType;
            if (typeof(ScriptableObject).IsAssignableFrom(scriptableObjectType))
            {
                return scriptableObjectType;
            }

            return null;
        }
    }
}