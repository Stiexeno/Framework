using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using SF = UnityEngine.SerializeField;

namespace Framework
{
	[RequireComponent(typeof(HorizontalLayoutGroup))]
	public class FlexibleText : UIBehaviour
	{
		// Private fields

		[SF, HideInInspector] private TMP_Text textRef;
		private Image image;

		// Properties

		/// <summary>
		/// Lowercase to match TMP_Text rules
		/// </summary>
		public string text
		{
			get => textRef.text;
			set => textRef.text = value;
		}

		public Image Image
		{
			get
			{
				if (image == null)
				{
					image = GetComponentInChildren<Image>();
				}

				return image;
			}
		}

		// FlexibleText

		private void Initialize()
		{
			if (textRef == null)
			{
				var childs = GetComponentsInChildren<RectTransform>(true);

				for (var i = 1; i < childs.Length; i++)
				{
					var child = childs[i];
					if (child.TryGetComponent(out TMP_Text tmp_text))
					{
						textRef = tmp_text;

						var textLayoutElement = textRef.gameObject.GetComponent<LayoutElement>();

						if (textLayoutElement == null)
						{
							textLayoutElement = textRef.gameObject.AddComponent<LayoutElement>();
							textLayoutElement.flexibleWidth = 0;
						}

						continue;
					}

					var iconLayoutElement = child.gameObject.GetComponent<LayoutElement>();

					if (iconLayoutElement == null)
					{
						iconLayoutElement = child.gameObject.AddComponent<LayoutElement>();
						iconLayoutElement.preferredHeight = child.sizeDelta.y;
						iconLayoutElement.preferredWidth = child.sizeDelta.x;
					}
				}

				if (textRef == null)
					throw new NullReferenceException("No text component found!");

				var horizontalLayoutGroup = GetComponent<HorizontalLayoutGroup>();

				horizontalLayoutGroup.childAlignment = TextAnchor.MiddleCenter;
				horizontalLayoutGroup.childForceExpandHeight = false;
				horizontalLayoutGroup.childForceExpandWidth = false;

				horizontalLayoutGroup.childScaleWidth = true;
				horizontalLayoutGroup.childControlWidth = true;
			}
		}

#if UNITY_EDITOR

		protected override void Reset()
		{
			base.Reset();
			Initialize();
		}

		/* ? Use this if you want to remove ContentSizeFitter from textRef
		*[ContextMenu("Setup")]
		*public void Setup()
		*{
		*	DestroyImmediate(textRef.GetComponent<ContentSizeFitter>(), true);
		*	textRef = null;
		*	
		*	Initialize();
		*	EditorUtility.SetDirty(gameObject);
		*}
		*/
#endif
	}
}