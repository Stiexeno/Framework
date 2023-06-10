using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DG.DemiEditor;
using Framework.Character;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;
using SF = UnityEngine.SerializeField;

namespace Framework.Editor
{
	[CustomEditor(typeof(Character.Character), true)]
	public class CharacterComponentEditor : UnityEditor.Editor
	{
		// Private fields

		private static Lazy<EditorSkin> _skin = new Lazy<EditorSkin>(() => new EditorSkin());
		private static EditorSkin skin => _skin.Value;

		private Character.Character character;

		private static readonly HashSet<Type> excludedComponents = new HashSet<Type>();

		// Properties

		// EntityComponentEditor

		private Type[] GetComponentPrototypeTypes()
		{
			return AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(x => x.GetTypes())
				.Where(x => !x.IsAbstract)
				.Where(x => x.GetInterface(nameof(ICharacterComponent)) != null)
				.Where(x => x.GetCustomAttribute<ObsoleteAttribute>() == null)
				.ToArray();
		}

		// MonoBehaviour

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			using (new EditorGUILayout.VerticalScope(GUI.skin.box))
			{
				var components = character.GetComponents<ICharacterComponent>()
					.Where(x => x != null);
				{
					var labelRect = EditorGUILayout.GetControlRect(true);
					EditorGUI.LabelField(labelRect, "Entity Components", EditorStyles.boldLabel);

					var buttonRect = labelRect.AddX(labelRect.width).AddX(-skin.buttonWidth).SetWidth(skin.buttonWidth);

					if (GUI.Button(buttonRect, "+", EditorStyles.miniButton))
					{
						var existingComponentPrototypes = components
							.Select(x => x.GetType())
							.ToList();

						var availableComponents = GetComponentPrototypeTypes()
							.Where(x => !existingComponentPrototypes.Contains(x))
							.ToList();

						EditorUtility.DisplayCustomMenu(buttonRect, availableComponents.Select(x => new GUIContent(x.Name)).ToArray(), -1,
							(userData, opts, selected) =>
							{
								var component = Undo.AddComponent(character.gameObject, availableComponents[selected]);
								component.hideFlags = HideFlags.HideInInspector;

								EditorUtility.SetDirty(character);
								AssetDatabase.SaveAssets();

								Repaint();
							}, null);
					}
				}

				using (new EditorGUI.IndentLevelScope())
				{
					foreach (var c in components)
					{
						var go = (Object)c;
						var so = new SerializedObject(go);
						var sp = so.GetIterator();
						var rect = GUILayoutUtility.GetRect(GUIContent.none, skin.inspectorTitlebar);
						sp.isExpanded = EditorGUI.InspectorTitlebar(rect, sp.isExpanded, go, true);
						Rect textRect = new Rect(rect.x + 35, rect.y, rect.width - 100, rect.height);

						if (Event.current.type == EventType.Repaint)
						{
							if (go.hideFlags != HideFlags.HideInInspector)
							{
								go.hideFlags = HideFlags.HideInInspector;

								EditorUtility.SetDirty(character);
								AssetDatabase.SaveAssets();
							}

							using (new DeGUI.ColorScope(skin.inspectorTitlebarBackground))
							{
								var texRect = textRect;
								texRect.y += 2;
								texRect.height -= 2;
								GUI.DrawTextureWithTexCoords(texRect, Texture2D.whiteTexture, new Rect(0.5f, 0.5f, 0.0f, 0.0f), false);
							}

							skin.inspectorTitlebar.Draw(textRect, $"{c.GetType().Name}", false, false, false, false);
						}

						if (sp.isExpanded)
						{
							var editor = CreateEditor(go);

							editor.OnInspectorGUI();
						}
					}
				}
			}
		}

		private void OnEnable()
		{
			character = target as Character.Character;
		}
	}
}