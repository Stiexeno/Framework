using UnityEditor;
using UnityEngine;
using SF = UnityEngine.SerializeField;

namespace Framework
{
    public interface IGUIView : IGUIElement
    {
        public void OnGUI(EditorWindow window, Rect rect);
    }
}