using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Framework.Utils;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Dropdown<>))]
public class DropdownDrawer : PropertyDrawer
{
    private readonly List<string> cachedVariables = new List<string>();
    private readonly List<string> cachedValues = new List<string>();

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        FieldInfo[] constantVariables = GetConstants(fieldInfo.FieldType.GenericTypeArguments.First()).ToArray();
        string[] constantValues = GetConstantsValues(fieldInfo.FieldType.GenericTypeArguments.First()).ToArray();

        // Getting all const variable names
        if (constantVariables.ToArray().Length != cachedVariables.Count)
        {
            cachedVariables.Clear();
            foreach (var tp in GetConstants(fieldInfo.FieldType.GenericTypeArguments.First()))
            {
                var typeText = tp.ToString();
                cachedVariables.Add(typeText.Replace("System.String ", ""));
            }   
        }

        // Getting all const variable values
        if (constantValues.ToArray().Length != cachedValues.Count)
        {
            cachedValues.Clear();
            foreach (var tp in GetConstantsValues(fieldInfo.FieldType.GenericTypeArguments.First()))
            {
                var typeText = tp;
                cachedValues.Add(typeText);
            }
        }

        var foundValue = cachedValues.Find(value => value.Equals(property.FindPropertyRelative("cachedValue").stringValue));
        var index = foundValue != null ? cachedValues.IndexOf(foundValue) : 0;
        
        // Drawing Dropdown list
        index = EditorGUI.Popup(position,
	        index, cachedVariables.ToArray());
        property.FindPropertyRelative("cachedValue").stringValue =
            cachedValues[index];
        
        
        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
    }
    
    // Utils

    private IEnumerable<FieldInfo> GetConstants(Type type)
    {
	    var fieldInfos = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
        return fieldInfos.Where(fi => fi.IsLiteral && !fi.IsInitOnly);
    }

    private IEnumerable<string> GetConstantsValues(Type type)
    {
        var fieldInfos = GetConstants(type);
        return fieldInfos.Select(fi => fi.GetRawConstantValue() as string);
    }
}