using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using SF = UnityEngine.SerializeField;

namespace Framework.Utils
{
	[ExecuteInEditMode]
	public class Progressbar : MonoBehaviour
	{
		// Serialized fields

		[SF] private Image fill;
		[SF] private float fillDuration;
		[SF] private AnimationCurve ease = AnimationCurve.Linear(0f, 0f, 1f, 1f);
		[SF] private float delay = 0f;

		// Private fields
		
		private float value;
		private float progress;

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
	}	
}