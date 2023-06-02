using Framework.Inspector;
using UnityEditor;
using UnityEngine;

namespace Framework.Editor
{
    [CustomPropertyDrawer(typeof(IfPlaying), true)]
    public class IfPlayingDrawer : PropertyDrawer
    {
        private bool showField = true;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var ifPlaying = ((IfPlaying) this.attribute).value;
            showField = (Application.isPlaying && ifPlaying) || (!Application.isPlaying && !ifPlaying);

            if (showField)
            {
                EditorGUI.PropertyField(position, property, new GUIContent(label.text),true);
            }
                
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return showField ? EditorGUI.GetPropertyHeight(property) : 0f;
        }
    }
}