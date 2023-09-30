using UnityEditor;
using UnityEngine;
using SF = UnityEngine.SerializeField;

public static class GraphStyle
{
    public static readonly GUIStyle Header0Middle = new GUIStyle
    {
        richText = true,
        fontSize = 13,
        fontStyle = FontStyle.Bold,
        wordWrap = true,
        padding = new RectOffset(10, 10, 10, 0),
        alignment = TextAnchor.MiddleCenter,
        normal = new GUIStyleState()
        {
            textColor = new Color(0.67f, 0.67f, 0.67f),
            background = Texture2D.blackTexture
        },
    };
    
    public static readonly GUIStyle Header1Middle = new GUIStyle
    {
        richText = true,
        fontSize = 11,
        fontStyle = FontStyle.Bold,
        wordWrap = true,
        padding = new RectOffset(10, 10, 10, 0),
        alignment = TextAnchor.MiddleCenter,
        normal = new GUIStyleState()
        {
            textColor = new Color(0.67f, 0.67f, 0.67f),
            background = Texture2D.blackTexture
        },
    };
    
    public static readonly GUIStyle Header1Left = new GUIStyle
    {
        richText = true,
        fontSize = 12,
        fontStyle = FontStyle.Normal,
        wordWrap = true,
        padding = new RectOffset(10, 10, 10, 0),
        alignment = TextAnchor.MiddleCenter,
        normal = new GUIStyleState()
        {
            textColor = new Color(0.86f, 0.86f, 0.86f),
            background = Texture2D.blackTexture
        },
    };
    
    public static readonly GUIStyle Header0Center = new GUIStyle
    {
        richText = true,
        fontSize = 15,
        fontStyle = FontStyle.Bold,
        wordWrap = true,
        padding = new RectOffset(10, 10, 10, 10),
        alignment = TextAnchor.MiddleCenter,
        normal = new GUIStyleState()
        {
            textColor = new Color(0.67f, 0.67f, 0.67f),
            background = Texture2D.blackTexture
        },
    };
    
    public static readonly GUIStyle SearchHeader = new GUIStyle
    {
        richText = true,
        fontSize = 11,
        fontStyle = FontStyle.Bold,
        wordWrap = true,
        padding = new RectOffset(3, 0, 0, 0),
        alignment = TextAnchor.MiddleLeft,
        normal = new GUIStyleState()
        {
            textColor = new Color(1f, 0.79f, 0.05f),
            background = Texture2D.blackTexture
        },
    };
    
    public static readonly GUIStyle SearchItem = new GUIStyle
    {
        richText = true,
        fontSize = 11,
        fontStyle = FontStyle.Normal,
        wordWrap = true,
        padding = new RectOffset(15, 0, 0, 0),
        alignment = TextAnchor.MiddleLeft,
        normal = new GUIStyleState()
        {
            textColor = new Color(0.77f, 0.77f, 0.77f),
            background = Texture2D.blackTexture
        },
        hover = new GUIStyleState()
        {
            textColor = new Color(1f, 0.79f, 0.05f),
            background = Texture2D.blackTexture
        }
    };
    
    public static readonly GUIStyle ToolbarTab = new GUIStyle
    {
        richText = true,
        fontSize = 11,
        fontStyle = FontStyle.Normal,
        wordWrap = true,
        padding = new RectOffset(0, 0, 0, 0),
        alignment = TextAnchor.MiddleCenter,
        normal = new GUIStyleState()
        {
            textColor = new Color(0.89f, 0.89f, 0.89f),
            background = Texture2D.blackTexture
        },
    };
    
    public static readonly GUIStyle ToolbarTabActive = new GUIStyle
    {
        richText = true,
        fontSize = 11,
        fontStyle = FontStyle.Normal,
        wordWrap = true,
        padding = new RectOffset(0, 0, 0, 0),
        alignment = TextAnchor.MiddleCenter,
        normal = new GUIStyleState()
        {
            textColor = new Color(1f, 0.79f, 0.05f),
            background = Texture2D.blackTexture
        },
    };
    
    public static readonly GUIStyle IconCenter = new GUIStyle
    {
        richText = true,
        fontStyle = FontStyle.Normal,
        alignment = TextAnchor.MiddleCenter
    };

    public static void DrawHorizontalLine(Rect rect)
    {
        rect.height = 2;
        EditorGUI.DrawRect(rect, new Color(0.15f, 0.15f, 0.16f));
        
        rect.y += 2;
        rect.height = 1;
        EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f));
    }
}
