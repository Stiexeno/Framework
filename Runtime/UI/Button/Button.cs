using System;
using System.Collections.Generic;
using Framework.Inspector;
using Framework.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using SF = UnityEngine.SerializeField;

namespace Framework.UI
{
	[RequireComponent(typeof(Image))]
	public class Button : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler,
		IPointerEnterHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
	{
		// Private

		private Image image;
		private TMP_Text tmpText;
		private ScrollRect scrollRectParent;

		private bool isAwoken;
		
		private const float TIME_TO_START_REPEATING = 0.2f;
		private const float REPEATING_INTERVAL = 0.15f;

		private float nextTimeToRepeat;
		
		private List<ButtonAnimation> buttonAnimations = new List<ButtonAnimation>();

		// Properties
		
		protected bool Interactable { get; private set; } = true;
		public bool HeldDown { get; protected set; }
		public bool MouseOverObject { get; protected set; }

		public Image Image
		{
			get
			{
				if (image == null)
				{
					image = GetComponent<Image>();
				}

				return image;
			}
		}

		public TMP_Text Text
		{
			get
			{
				if (tmpText == null)
				{
					tmpText = GetComponentInChildren<TMP_Text>();
				}

				return tmpText;
			}
		}

		public List<ButtonAnimation> ButtonAnimations => buttonAnimations;

		// Events

		[SF] private bool repeating;

		public event Action OnImpressed;
		public event Action OnRelease;
		public event Action OnClick;

		// Button

		private void Update()
		{
			Process();

			if (Interactable == false)
				return;
			
			if (repeating && HeldDown && Time.time > nextTimeToRepeat)
			{
				nextTimeToRepeat = Time.time + REPEATING_INTERVAL;
				ClickPerformed();
			}
		}

		protected virtual void Process()
		{
		}

		public void ForceClick()
		{
			ClickPerformed();
		}

		protected virtual void ClickPerformed()
		{
			OnClick?.Invoke();
		}

		public virtual void OnPointerDown(PointerEventData eventData)
		{
			Init();
			
			if (Interactable == false)
				return;

			HeldDown = true;
			nextTimeToRepeat = Time.time + TIME_TO_START_REPEATING;

			if (ButtonAnimations.IsEmpty() == false)
			{
				foreach (var buttonAnimation in ButtonAnimations)
				{
					buttonAnimation.PressAnimation(OnImpressed);
				}
			}
			else
			{
				OnImpressed?.Invoke();
			}
		}

		public void SetInteractable(bool interactable)
		{
			Init();
			Interactable = interactable;

			if (ButtonAnimations.IsEmpty() == false)
			{
				foreach (var buttonAnimation in ButtonAnimations)
				{
					buttonAnimation.SetInteractable(interactable);
				}
			}
		}

		public virtual void OnPointerUp(PointerEventData eventData)
		{
			Init();
			
			if (ButtonAnimations.IsEmpty() == false)
			{
				foreach (var buttonAnimation in ButtonAnimations)
				{
					buttonAnimation.ReleaseAnimation(OnRelease);
				}
			}
			else
			{
				OnRelease?.Invoke();
			}
			
			if (Interactable)
			{
				if (MouseOverObject && HeldDown)
				{
					ClickPerformed();
				}	
			}

			HeldDown = false;
		}

		public virtual void OnPointerExit(PointerEventData eventData)
		{
			if (ButtonAnimations.IsEmpty() == false && HeldDown)
			{
				foreach (var buttonAnimation in ButtonAnimations)
				{
					buttonAnimation.ReleaseAnimation(OnRelease);
				}
			}

			MouseOverObject = false;
		}

		public virtual void OnPointerEnter(PointerEventData eventData)
		{
			MouseOverObject = true;
		}

		public virtual void OnDrag(PointerEventData eventData)
		{
			if (scrollRectParent != null)
			{
				scrollRectParent.OnDrag(eventData);
			}
		}

		public void OnEndDrag(PointerEventData eventData)
		{
			if (scrollRectParent != null)
			{
				scrollRectParent.OnEndDrag(eventData);
			}
		}

		public void OnBeginDrag(PointerEventData eventData)
		{
			if (scrollRectParent != null)
			{
				scrollRectParent.OnBeginDrag(eventData);
				OnPointerExit(eventData);
			}
		}

		private void Init()
		{
			if (isAwoken == false)
			{
				Awake();
			}
		}

		protected virtual void OnInit()
		{
		}

#if UNITY_EDITOR
		[Button("Add Default Animation", height: 20f)]
		public void AddDefaultAnimation()
		{
			gameObject.AddComponent<DefaultButtonAnimation>();
			UnityEditor.EditorUtility.SetDirty(this);
		}
#endif

		// Mono

		protected virtual void Awake()
		{
			ButtonAnimations.AddRange(GetComponents<ButtonAnimation>());
			if (ButtonAnimations.IsEmpty() == false)
			{
				foreach (var buttonAnimation in ButtonAnimations)
				{
					buttonAnimation.Init();
				}
			}

			scrollRectParent = GetComponentInParent<ScrollRect>();

			OnInit();

			isAwoken = true;
		}
	}
}