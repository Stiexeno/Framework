using Framework.Inspector;
using UnityEditor;
using UnityEngine;
using SF = UnityEngine.SerializeField;

namespace Framework.Editor
{
	[CustomPropertyDrawer(typeof(HelpBox), true)]
	public class HelpBoxDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var helpBox = ((HelpBox) attribute);
			
			GUILayout.Space(5f);
            var messageType = MessageType.None;
            
            switch (helpBox.messageType)
            {
                case "Info": messageType = MessageType.Info;
                    break;
                case "Error": messageType = MessageType.Error;
                    break;
                case "Warning": messageType = MessageType.Warning;
                    break;
            }
            
			EditorGUILayout.HelpBox(helpBox.description, messageType);
			EditorGUI.PropertyField(position, property, new GUIContent(label.text),true);
		}
	}

}