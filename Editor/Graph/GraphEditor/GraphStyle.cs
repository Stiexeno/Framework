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
            textColor = new Color(0.77f, 0.77f, 0.77f),
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
            textColor = new Color(0.77f, 0.77f, 0.77f),
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
            textColor = new Color(0.77f, 0.77f, 0.77f),
            background = Texture2D.blackTexture
        },
    };

    public static void DrawHorizontalLine(Rect rect)
    {
        rect.height = 2;
        EditorGUI.DrawRect(rect, new Color(0.15f, 0.15f, 0.16f));
        
        rect.y += 2;
        rect.height = 1;
        EditorGUI.DrawRect(rect, new Color(0.69f, 0.69f, 0.69f));
    }
}
