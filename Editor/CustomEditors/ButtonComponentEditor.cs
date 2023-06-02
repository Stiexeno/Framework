using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DG.DemiEditor;
using Framework.UI;
using UnityEditor;
using UnityEngine;
using SF = UnityEngine.SerializeField;

namespace Framework.Editor
{
	[CustomEditor(typeof(Button), true)]
	public class ButtonComponentEditor : UnityEditor.Editor
	{
		// Private fields

		private static Lazy<EditorSkin> _skin = new Lazy<EditorSkin>(() => new EditorSkin());
		private static EditorSkin skin => _skin.Value;

		private Button button;

		private static readonly Type[] componentPrototypeTypes = AppDomain.CurrentDomain.GetAssemblies()
			.SelectMany(x => x.GetTypes())
			.Where(x => !x.IsAbstract)
			.Where(x => x.BaseType == typeof(ButtonAnimation))
			.Where(x => x.GetCustomAttribute<ObsoleteAttribute>() == null)
			.ToArray();

		private static readonly HashSet<Type> excludedComponents = new HashSet<Type>();

		// Properties

		// EntityComponentEditor

		// MonoBehaviour

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			using (new EditorGUILayout.VerticalScope(GUI.skin.box))
			{
				var components = button.GetComponents<ButtonAnimation>()
					.Where(x => x != null);
				{
					var labelRect = EditorGUILayout.GetControlRect(true);
					EditorGUI.LabelField(labelRect, "Button animations", EditorStyles.boldLabel);

					var buttonRect = labelRect.AddX(labelRect.width).AddX(-skin.buttonWidth).SetWidth(skin.buttonWidth);

					if (GUI.Button(buttonRect, "+", EditorStyles.miniButton))
					{
						var existingComponentPrototypes = components
							.Select(x => x.GetType())
							.ToList();

						var availableComponents = componentPrototypeTypes
							.Where(x => !existingComponentPrototypes.Contains(x))
							.ToList();
						
						EditorUtility.DisplayCustomMenu(buttonRect, availableComponents.Select(x => new GUIContent(x.Name)).ToArray(), -1,
							(userData, opts, selected) =>
							{
								var component = Undo.AddComponent(button.gameObject, availableComponents[selected]);
								component.hideFlags = HideFlags.HideInInspector;
								
								EditorUtility.SetDirty(button);
								AssetDatabase.SaveAssets();
								
								Repaint();
							}, null);
					}
				}

				using (new EditorGUI.IndentLevelScope())
				{
					foreach (var c in components)
					{
						var so = new SerializedObject(c);
						var sp = so.GetIterator();
						var rect = GUILayoutUtility.GetRect(GUIContent.none, skin.inspectorTitlebar);
						sp.isExpanded = EditorGUI.InspectorTitlebar(rect, sp.isExpanded, c, true);
						Rect textRect = new Rect(rect.x + 35, rect.y, rect.width - 100, rect.height);
						
						if (Event.current.type == EventType.Repaint) {

							using (new DeGUI.ColorScope(skin.inspectorTitlebarBackground)) {
								var texRect = textRect;
								texRect.y += 2;
								texRect.height -= 2;
								GUI.DrawTextureWithTexCoords(texRect, Texture2D.whiteTexture, new Rect(0.5f, 0.5f, 0.0f, 0.0f), false);
							}

							skin.inspectorTitlebar.Draw(textRect, $"{c.GetType().Name}", false, false, false, false);
						}

						if (sp.isExpanded)
						{
							var editor = CreateEditor(c);
						
							editor.OnInspectorGUI();	
						}
					}
				}
			}
		}

		private void OnEnable()
		{
			button = target as Button;
		}
	}
}