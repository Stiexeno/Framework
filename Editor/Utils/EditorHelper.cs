using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Framework.Editor
{
    public static class EditorHelper
    {
        public static void DrawBorderRect(Rect area, Color color,
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
            y1 = area.y + area.height - 1f;
            x2 = area.width;
            y2 = borderWidth;

            lineRect = new Rect(x1, y1, x2, y2);

            EditorGUI.DrawRect(lineRect, color);
        }
        
        public static GUIContent CreateNamedIcon(string text, string iconId, string toolTip = null)
        {
            var icon = new GUIContent();
            if (string.IsNullOrEmpty(iconId) == false)
            {
                icon = new GUIContent(EditorGUIUtility.IconContent($"{iconId}"));
            }

            return new GUIContent($"{text}", icon.image, $"{toolTip}");
        }
        
        public static Texture2D Texture2DColor(Color color)
        {
            var texture = new Texture2D(Screen.width, Screen.height);
            Color[] pixels = Enumerable.Repeat(color, Screen.width * Screen.height).ToArray();
            texture.SetPixels(pixels);
            texture.Apply();
            return texture;
        }
        
        public static void DrawVerticalLine(Rect rect)
        {
            rect.width = 1f;
            
            EditorGUI.DrawRect(rect, new Color(0.14f, 0.14f, 0.14f));
        }
    }
}
