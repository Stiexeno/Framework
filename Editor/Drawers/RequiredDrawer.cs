using System;
using System.Reflection;
using Framework.Editor;
using Framwework.Inspector;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Framwework.Editor
{
	[CustomPropertyDrawer(typeof(Object), true)]
	public class RequiredDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var indent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;

			position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
			EditorGUI.PropertyField(position, property, GUIContent.none, true);

			var value = property.objectReferenceValue;
			var containsFieldAttribute = fieldInfo.GetCustomAttribute<CanBeNull>() != null;
			var containsClassAttribute = property.serializedObject.targetObject.GetType().GetCustomAttribute<CanBeNull>() != null;
			var isExcluded = IsExcludedType(property.serializedObject.targetObject.GetType());

			if (value == false && containsFieldAttribute == false && containsClassAttribute == false && isExcluded == false)
			{
				var containsKey = RequiredData.Data.TryGetKey(fieldInfo.ToString());

				if (RequiredData.Data.showBorder && containsKey == false)
				{
					DrawBorderRect(property, label, position, RequiredData.Data.borderColor, 1f);
				}

				if (RequiredData.Data.showIcon)
				{
					position.x -= 22f;

					if (GUI.Button(position,
						new GUIContent(EditorGUIUtility.IconContent(containsKey == false ? RequiredData.Data.GetIconType() : "Refresh")),
						GUIStyle.none))
					{
						if (containsKey == false)
						{
							RequiredData.Data.AddKey(fieldInfo.ToString());
						}
						else
						{
							RequiredData.Data.RemoveKey(fieldInfo.ToString());
						}
					}
				}
			}

			EditorGUI.indentLevel = indent;
		}

		private bool IsExcludedType(Type parentType)
		{
			return Array.Find(RequiredExcludedTypes.excludedTypes, excludedType => excludedType == parentType) != null;
		}

		private void DrawBorderRect(SerializedProperty property, GUIContent label, Rect area, Color color,
			float borderWidth)
		{
			//------------------------------------------------
			float x1 = area.x;
			float y1 = area.y;
			float x2 = area.width;
			float y2 = borderWidth;

			Rect lineRect = new Rect(x1, y1, x2, y2);

			EditorGUI.DrawRect(lineRect, color);

			//------------------------------------------------
			x1 = area.x + area.width - 1f;
			y1 = area.y;
			x2 = borderWidth;
			y2 = area.height;

			lineRect = new Rect(x1, y1, x2, y2);

			EditorGUI.DrawRect(lineRect, color);

			//------------------------------------------------
			x1 = area.x;
			y1 = area.y;
			x2 = borderWidth;
			y2 = area.height;

			lineRect = new Rect(x1, y1, x2, y2);

			EditorGUI.DrawRect(lineRect, color);

			//------------------------------------------------
			x1 = area.x;
			y1 = area.y + GetPropertyHeight(property, label) - 1f;
			x2 = area.width;
			y2 = borderWidth;

			lineRect = new Rect(x1, y1, x2, y2);

			EditorGUI.DrawRect(lineRect, color);
		}
	}
}