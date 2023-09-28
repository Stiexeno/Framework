using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Framework.Editor
{
    public static class EditorHelper
    {
        /// <summary>
        /// Draws a border rect in the editor by given rect
        /// </summary>
        /// <param name="area">Rect area</param>
        /// <param name="color">Border color</param>
        /// <param name="borderWidth">Border width</param>
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
        
        /// <summary>
        /// Creates a GUIContent with given text and icon
        /// </summary>
        /// <param name="text">Text on the left, can be empty</param>
        /// <param name="iconId">https://github.com/halak/unity-editor-icons/tree/master</param>
        /// <param name="toolTip">Text on hover</param>
        /// <returns></returns>
        /// TODO: Change it to string iconId, string text = "", string toolTip = null
        public static GUIContent Icon(string text, string iconId, string toolTip = null)
        {
            var icon = new GUIContent();
            if (string.IsNullOrEmpty(iconId) == false)
            {
                icon = new GUIContent(EditorGUIUtility.IconContent($"{iconId}"));
            }

            return new GUIContent($"{text}", icon.image, $"{toolTip}");
        }
        
        /// <summary>
        /// Creates a texture2D with given color
        /// </summary>
        /// <param name="color">Texture color</param>
        /// <returns></returns>
        public static Texture2D Texture2DColor(Color color)
        {
            var texture = new Texture2D(Screen.width, Screen.height);
            Color[] pixels = Enumerable.Repeat(color, Screen.width * Screen.height).ToArray();
            texture.SetPixels(pixels);
            texture.Apply();
            return texture;
        }
        
        /// <summary>
        /// Draws a vertical line in the editor by given rect
        /// </summary>
        /// <param name="rect"></param>
        public static void DrawVerticalLine(Rect rect)
        {
            rect.width = 1f;
            EditorGUI.DrawRect(rect, new Color(0.14f, 0.14f, 0.14f));
        }
        
        /// <summary>
        /// Draws a vertical line in the editor
        /// </summary>
        /// <param name="color">Line color</param>
        /// <param name="thickness">Line thickness</param>
        /// <param name="padding">Line padding</param>
        public static void DrawVerticalLine(Color color, int thickness = 1, int padding = 10, int length = 100)
        {
            Rect r = EditorGUILayout.GetControlRect(GUILayout.Width(padding+thickness),GUILayout.Height(length));
            r.width = thickness;
            r.x+=padding/2;
            r.height -= 10;
            EditorGUI.DrawRect(r, color);
        }
        
        /// <summary>
        /// Draws a horizontal line in the editor
        /// </summary>
        /// <param name="color">Line color</param>
        /// <param name="thickness">Line thickness</param>
        /// <param name="padding">Line padding</param>
        public static void DrawHorizontalLine(Color color, int thickness = 1, int padding = 10)
        {
            Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding+thickness));
            r.height = thickness;
            r.y+=padding/2;
            r.x-=2;
            r.width +=6;
            EditorGUI.DrawRect(r, color);
        }
        
        public static List<Object> LoadAllAssetsAtPath (string path, string assetType = "asset") 
        {
            var objects = new List<Object>();
            if (System.IO.Directory.Exists(path)) 
            {
                var assets = System.IO.Directory.GetFiles(path);
                foreach (string assetPath in assets) 
                {
                    if (assetPath.Contains($".{assetType}") && !assetPath.Contains(".meta")) 
                    {
                        objects.Add(AssetDatabase.LoadMainAssetAtPath(assetPath));
                    }
                }
            }
            return objects;
        }
        
        /// <summary>
        /// Draws all properties like base.OnInspectorGUI() but excludes a field by name.
        /// </summary>
        /// <param name="fieldToSkip">The name of the field that should be excluded. Example: "m_Script" will skip the default Script field.</param>
        public static void DrawInspectorExcept(this SerializedObject serializedObject, string fieldToSkip)
        {
            serializedObject.DrawInspectorExcept(new string[1] { fieldToSkip });
        }
 
        /// <summary>
        /// Draws all properties like base.OnInspectorGUI() but excludes the specified fields by name.
        /// </summary>
        /// <param name="fieldsToSkip">
        /// An array of names that should be excluded.
        /// Example: new string[] { "m_Script" , "myInt" } will skip the default Script field and the Integer field myInt.
        /// </param>
        public static void DrawInspectorExcept(this SerializedObject serializedObject, string[] fieldsToSkip)
        {
            serializedObject.Update();
            SerializedProperty prop = serializedObject.GetIterator();
            if (prop.NextVisible(true))
            {
                do
                {
                    if (fieldsToSkip.Any(prop.name.Contains))
                        continue;
 
                    EditorGUILayout.PropertyField(serializedObject.FindProperty(prop.name), true);
                }
                while (prop.NextVisible(false));
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
