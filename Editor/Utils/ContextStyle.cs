using UnityEngine;

namespace Framework.Editor
{
    internal static class ContextStyle
    {
        internal static GUIStyle TextStyle(int fontSize = 13, Color color = default, FontStyle fontStyle = FontStyle.Bold, TextAnchor anchor = TextAnchor.MiddleCenter)
        {
            color = color == default ? new Color(0.94f, 0.94f, 0.94f) : color;
                
            return new GUIStyle()
            {
                richText = true,
                fontSize = fontSize,
                fontStyle = fontStyle,
                wordWrap = true,
                padding = new RectOffset(10, 10, 10, 10),
                margin = new RectOffset(10, 10, 10, 10),
                alignment = anchor,
                normal = new GUIStyleState()
                {
                    textColor = color,
                    background = Texture2D.blackTexture
                },
            };
        }
    }
}
