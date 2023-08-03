using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using Framework.Inspector;

namespace Framework.Editor
{
	[CustomEditor(typeof(ScriptableObject), true)]
	public class SObjectEditor : UnityEditor.Editor
	{
		private ScriptableObject targetObject;
		private MemberInfo[] behaviourMethods;

		// Editor

		private void OnEnable()
		{
			targetObject = target as ScriptableObject;
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			CustomInspector();
		}

		// MonoEditor

		private void CustomInspector()
		{
			if (!targetObject)
			{
				EditorGUILayout.HelpBox("ScriptableObject is not properly loaded, try selecting the GameObject again.", MessageType.Error);
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
			var attribute = thisMethod.GetCustomAttribute<ButtonAttribute>();
			var buttonName = string.IsNullOrEmpty(attribute.label) ? $"{thisMethod.Name}" : attribute.label;
			var ifPlaying = thisMethod.GetCustomAttribute<IfPlaying>();

			var showButton = true;

			if (ifPlaying != null)
			{
				showButton = (Application.isPlaying && ifPlaying.value) || (!Application.isPlaying && !ifPlaying.value);
			}

			if (showButton)
			{
				Color oldGUIColor = GUI.color;
				GUI.color = Color.white;
				if (GUILayout.Button(buttonName, GUILayout.Height(attribute.height)))
				{
					thisMethod.Invoke(targetObject, null);
				}

				GUI.color = oldGUIColor;
			}
		}

		// Helpers

		private MemberInfo[] GetSOMethods()
		{
			return targetObject.GetType()
				.GetMembers(BindingFlags.Instance | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public |
				            BindingFlags.NonPublic);
		}

		private IEnumerable<MemberInfo> GetMethodsWithAttribute(Type thisAttribute)
		{
			behaviourMethods ??= GetSOMethods();
			return behaviourMethods.Where(o => Attribute.IsDefined(o, thisAttribute));
		}
	}
}