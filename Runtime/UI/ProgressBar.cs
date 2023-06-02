using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using SF = UnityEngine.SerializeField;

namespace Framework.Utils
{
	[ExecuteInEditMode]
	public sealed class ProgressBar : MonoBehaviour
	{
		// Serialized fields
		
		[SF] private float fillDuration;
		[SF] private AnimationCurve ease;
		[SF] private float delay = 0.3f;
		[SF] private bool reverse;

		private bool initialValueChange;
		private Image image;

		[HideInInspector] [SF] private RectTransform fill;
		
		// Private
		
		private float value;
		
		public float FillDuration { get => fillDuration; set => fillDuration = value;}
		
		public Image Image
		{
			get
			{
				if (image == null)
				{
					image = fill.GetComponent<Image>();
				}

				return image;
			}
		}

		// Events
		
		public event Action<float, bool> OnProgressChange;
		public event Action<float> OnAnchorChange;

		// Properties
		
		public float Value
		{
			get => value;

			set
			{
				if (Math.Abs(this.value - value) > 0.001f || !initialValueChange)
				{
					initialValueChange = true;
					this.value = Mathf.Clamp01(value);

					if (Application.isPlaying == false)
					{
						if (reverse)
						{
							fill.anchorMin = new Vector2(1 - this.value, fill.anchorMin.y);	
							OnAnchorChange?.Invoke(1 - this.value);
						}
						else
						{
							fill.anchorMax = new Vector2(this.value, fill.anchorMax.y);		
							OnAnchorChange?.Invoke(this.value);
						}
					}
					else
						Evaluate(this.value);

					OnProgressChange?.Invoke(this.value, false);
				}
			}
		}

		public float DirectValue
		{
			set
			{
				if (Math.Abs(this.value - value) > 0.001f || !initialValueChange)
				{
					initialValueChange = true;
					
					this.value = Mathf.Clamp01(value);
					fill.DOKill();
					fill.anchorMax = new Vector2(this.value, fill.anchorMax.y);
					
					OnProgressChange?.Invoke(this.value, true);
				}
			}
		}

		// ProgressBar

		private void Init()
		{
			
		}
		
		private void Evaluate(float normalizedValue)
		{
			fill.DOKill();

			if (reverse)
			{
				fill.DOAnchorMin(new Vector2(1 - normalizedValue, fill.anchorMin.y), FillDuration)
					.SetEase(ease).SetDelay(delay).OnUpdate(() =>
					{
						OnAnchorChange?.Invoke(1 - normalizedValue);
					});
			}
			else
			{
				fill.DOAnchorMax(new Vector2(normalizedValue, fill.anchorMax.y), FillDuration)
					.SetEase(ease).SetDelay(delay).OnUpdate(() =>
					{
						OnAnchorChange?.Invoke(normalizedValue);
					});
			}
		}

		private void Awake()
		{
			Init();
		}

#if UNITY_EDITOR
		public void Setup()
		{
			fill = (RectTransform)transform.GetChild(1).GetComponentInChildren<Image>().transform;
			ease = AnimationCurve.Linear(0, 0, 1, 1);

			OnProgressChange?.Invoke(Value, false);
		}
#endif
	}
}