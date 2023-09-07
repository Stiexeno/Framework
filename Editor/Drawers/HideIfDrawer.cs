using Framework.Inspector;
using UnityEditor;
using UnityEngine;

namespace Framework.Editor
{
    [CustomPropertyDrawer(typeof(HideIf), true)]
    public class HideIfDrawer : PropertyDrawer
    {
        private bool showField = true;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var hideIf = ((HideIf) attribute);
            var compare = property.serializedObject.FindProperty(hideIf.PropertyName);
            var value = compare.GetValue();

            showField = !value.Equals(hideIf.Value);

            if (showField)
            {
                EditorGUI.PropertyField(position, property, new GUIContent(label.text),true);
            }
                
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return showField ? EditorGUI.GetPropertyHeight(property,label) : -2f;
        }
    }
}