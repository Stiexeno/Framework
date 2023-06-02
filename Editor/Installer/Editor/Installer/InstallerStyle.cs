using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Framework.Installer
{
    public static class InstallerStyle
    {
        public static readonly GUIStyle CATEGORY_BUTTON = new GUIStyle()
        {
            richText = true,
            fontSize = 12,
            fontStyle = FontStyle.Normal,
            wordWrap = true,
            padding = new RectOffset(10, 10, 10, 10),
            margin = new RectOffset(10, 10, 11, 11),
            alignment = TextAnchor.MiddleLeft,
            normal = new GUIStyleState()
            {
                textColor = new Color(0.48f, 0.49f, 0.5f),
                background = Texture2D.blackTexture
            },
            active = new GUIStyleState()
            {
                textColor = Color.white,
                background = Texture2DColor(new Color(0.22f, 0.22f, 0.22f))
            },
            hover = new GUIStyleState()
            {
                textColor = Color.white,
                background = Texture2DColor(new Color(0.22f, 0.22f, 0.22f))
            }
        };
        
        public static readonly GUIStyle CATEGORY_BUTTON_SELECTED = new GUIStyle
        {
            richText = true,
            fontSize = 12,
            fontStyle = FontStyle.Bold,
            wordWrap = true,
            padding = new RectOffset(10, 10, 10, 10),
            margin = new RectOffset(10, 10, 10, 10),
            alignment = TextAnchor.MiddleLeft,
            normal = new GUIStyleState()
            {
                textColor = Color.white,
                background = Texture2DColor(new Color(0.17f, 0.36f, 0.53f))
            },
        };

        public static readonly GUIStyle HEADER = new GUIStyle
        {
            richText = true,
            fontSize = 13,
            fontStyle = FontStyle.Normal,
            wordWrap = true,
            padding = new RectOffset(10, 10, 15, 15),
            margin = new RectOffset(10, 10, 10, 10),
            alignment = TextAnchor.MiddleLeft,
            normal = new GUIStyleState()
            {
                textColor = new Color(0.96f, 0.96f, 0.96f),
                background = Texture2D.blackTexture
            },
        };
        
        public static readonly GUIStyle PACKAGE_HEADER = new GUIStyle()
        {
            richText = true,
            fontSize = 13,
            fontStyle = FontStyle.Bold,
            wordWrap = true,
            padding = new RectOffset(0, 10, 10, 10),
            margin = new RectOffset(10, 10, 10, 10),
            alignment = TextAnchor.UpperLeft,
            normal = new GUIStyleState()
            {
                textColor = new Color(0.94f, 0.94f, 0.94f),
                background = Texture2D.blackTexture
            },
        };
        
        public static readonly GUIStyle PACKAGE_DESCRIPTION = new GUIStyle()
        {
            richText = true,
            fontSize = 12,
            fontStyle = FontStyle.Normal,
            wordWrap = true,
            padding = new RectOffset(0, 10, 0, 0),
            margin = new RectOffset(0, 0, 0, 0),
            alignment = TextAnchor.UpperLeft,
            normal = new GUIStyleState()
            {
                textColor = new Color(0.67f, 0.67f, 0.67f),
                background = Texture2D.blackTexture
            },
        };
        
        public static readonly GUIStyle PACKAGE_FOOTER = new GUIStyle()
        {
            richText = true,
            fontSize = 10,
            fontStyle = FontStyle.Normal,
            wordWrap = true,
            padding = new RectOffset(0, 10, 20, 5),
            margin = new RectOffset(10, 10, 10, 10),
            alignment = TextAnchor.LowerLeft,
            normal = new GUIStyleState()
            {
                textColor = new Color(0.67f, 0.67f, 0.67f),
                background = Texture2D.blackTexture
            },
        };
        
        public static readonly GUIStyle PACKAGE_INSTALLED = new GUIStyle()
        {
            richText = true,
            fontSize = 11,
            fontStyle = FontStyle.Normal,
            wordWrap = true,
            padding = new RectOffset(0, 0, 10, 10),
            margin = new RectOffset(10, 10, 11, 10),
            alignment = TextAnchor.MiddleCenter,
            normal = new GUIStyleState()
            {
                textColor = new Color(0.41f, 0.89f, 0.62f),
                background = Texture2D.blackTexture
            }
        };
        
        public static readonly GUIStyle INSTALLER_BUTTON = new GUIStyle
        {
            richText = true,
            fontSize = 12,
            fontStyle = FontStyle.Normal,
            wordWrap = true,
            padding = new RectOffset(10, 10, 10, 10),
            margin = new RectOffset(10, 10, 11, 11),
            alignment = TextAnchor.MiddleCenter,
            normal = new GUIStyleState()
            {
                textColor = new Color(0.81f, 0.82f, 0.84f),
                background = Texture2D.blackTexture
            },
            hover = new GUIStyleState()
            {
                textColor = Color.white,
                background = Texture2DColor(new Color(0.3f, 0.3f, 0.3f))
            }
        };

        // Utils

        private static Texture2D Texture2DColor(Color color)
        {
            var texture = new Texture2D(Screen.width, Screen.height);
            Color[] pixels = Enumerable.Repeat(color, Screen.width * Screen.height).ToArray();
            texture.SetPixels(pixels);
            texture.Apply();
            return texture;
        }

        public static void DrawHorizontalLine(Rect rect)
        {
            rect.y += rect.height - 1;
            rect.height = 1f;
            
            EditorGUI.DrawRect(rect, new Color(0.14f, 0.14f, 0.14f));
        }
        
        public static void DrawVerticalLine(Rect rect)
        {
            rect.x += rect.width - 1;
            rect.width = 1f;
            
            EditorGUI.DrawRect(rect, new Color(0.14f, 0.14f, 0.14f));
        }
        
        public static void DrawUIBox(Rect rect, Color borderColor, Color backgroundColor, int width = 2)
        {
            Rect outer = new Rect(rect);
            Rect inner = new Rect(rect.x + width, rect.y + width, rect.width - width * 2, rect.height - width * 2);
            EditorGUI.DrawRect(outer, borderColor);
            EditorGUI.DrawRect(inner, backgroundColor);
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
    }
}