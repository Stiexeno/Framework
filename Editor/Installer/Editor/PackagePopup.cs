using System;
using UnityEditor;
using UnityEngine;

namespace Framework
{
    public class PackagePopup : EditorWindow
    {
        private string inputText;
        private string okButton;
        private string cancelButton;
        private bool initializedPosition;

        private Action<string> onResult;

        private bool shouldClose;

        public static void Show(string title, string inputText, string okButton = "OK", Action<string> result = null)
        {
            var window = CreateInstance<PackagePopup>();
            window.titleContent = new GUIContent(title);
            window.inputText = inputText;
            window.okButton = okButton;
            window.onResult = result;
            window.ShowModal();
        }

        private void OnGUI()
        {
            // Check if Esc/Return have been pressed
            var e = Event.current;
            if (e.type == EventType.KeyDown)
            {
                switch (e.keyCode)
                {
                    // Escape pressed
                    case KeyCode.Escape:
                        shouldClose = true;
                        break;

                    // Enter pressed
                    case KeyCode.Return:
                    case KeyCode.KeypadEnter:
                        Close();
                        onResult?.Invoke(inputText);
                        break;
                }
            }

            // Draw our control
            var rect = EditorGUILayout.BeginVertical();

            EditorGUILayout.Space(8);
            GUI.SetNextControlName("inText");
            inputText = EditorGUILayout.TextField("", inputText);
            GUI.FocusControl("inText"); // Focus text field
            EditorGUILayout.Space(5);

            // Draw OK / Cancel buttons
            var r = EditorGUILayout.GetControlRect();
            if (GUI.Button(r, okButton))
            {
                Close();
                onResult?.Invoke(inputText);
            }

            EditorGUILayout.Space(5);
            EditorGUILayout.EndVertical();

            // Force change size of the window
            if (rect.width != 0 && minSize != rect.size)
            {
                minSize = maxSize = rect.size;
            }
        }
    }
}