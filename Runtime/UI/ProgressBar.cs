using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using SF = UnityEngine.SerializeField;

namespace Framework.Utils
{
	[RequireComponent(typeof(CanvasGroup))]
	[ExecuteInEditMode]
	public class ProgressBar : MonoBehaviour
	{
		// Serialized fields

		[SF] private Image fill;
		[SF] private float fillDuration;
		[SF] private AnimationCurve ease = AnimationCurve.Linear(0f, 0f, 1f, 1f);
		[SF] private float delay = 0f;
		[SF] private bool lookAtCamera;

		// Private fields
		
		private float value;
		private float progress;

		private bool active;
		
		private CanvasGroup canvasGroup;

		// Properties

		public float Value
		{
			get => value;

			set
			{
				this.value = Mathf.Clamp01(value);
				Evaluate(this.value);
			}
		}
		
		public float DirectValue
		{
			get => value;

			set
			{
				this.value = Mathf.Clamp01(value);
				progress = value;

				if (fill != null)
				{
					fill.fillAmount = progress;	
				}
			}
		}
		
		// FillProgressBar
		
		private void Evaluate(float normalizedValue)
		{
			fill.DOKill();

			DOTween.To(x => progress = x, fill.fillAmount, normalizedValue, fillDuration).OnUpdate(() =>
			{
				fill.fillAmount = progress;
			}).SetEase(ease).SetDelay(delay);
		}

		public void SetEnabled(bool value)
		{
			if (active == value)
				return;
			
			active = value;
			
			if (canvasGroup != null)
			{
				canvasGroup.DOFade(value ? 1f : 0f, 0.2f);
				return;
			}
			
			gameObject.SetActive(value);
		}

		private void Awake()
		{
			canvasGroup = GetComponent<CanvasGroup>();
		}

		private void Update()
		{
			if (active & lookAtCamera)
			{
				transform.forward = Camera.main.transform.forward;
			}
		}
	}	
}