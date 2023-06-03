using Framework.Utils;
using UnityEditor;
using UnityEngine;

namespace Framework.Editor
{
	[CustomPropertyDrawer(typeof(AnimationArgs), true)]
	public class AnimationArgsDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);
            
			position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            
			var indent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;
            
			var curveRect = new Rect(position.x, position.y, position.width - 70, position.height);
			var timeRect = new Rect(position.x + position.width - 70, position.y, position.width - curveRect.width - 23, position.height);

			var curveProperty = property.FindPropertyRelative("curve");
			var timeProperty = property.FindPropertyRelative("duration");
            
			EditorGUI.PropertyField(curveRect, curveProperty, GUIContent.none);
			EditorGUI.PropertyField(timeRect, timeProperty, GUIContent.none);

			var copyRect = new Rect(position.x + position.width - 23, position.y, position.width - curveRect.width - timeRect.width, position.height);

			if (GUI.Button(copyRect, EditorHelper.Icon("", "d_Refresh")))
			{
				curveProperty.animationCurveValue =  AnimationCurve.Linear(0, 0, 1, 1);
				timeProperty.floatValue = 1f;
                
				EditorUtility.SetDirty(property.serializedObject.targetObject);
			}
			
			// Set indent back to what it was
			EditorGUI.indentLevel = indent;

			EditorGUI.EndProperty();
		}
	}
}