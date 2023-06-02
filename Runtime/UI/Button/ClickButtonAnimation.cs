using System;
using DG.Tweening;
using UnityEngine;
using SF = UnityEngine.SerializeField;

namespace Framework.UI
{
	[RequireComponent(typeof(RectTransform))]
	public class ClickButtonAnimation : ButtonAnimation
	{
		// Serialized fields

		[SF] protected float scaleDownDuration = 0.06f;
		[SF] protected float scaleUpDuration = 0.06f;

		[SF] protected float scaleFactor = 0.9f;

		// Private fields

		private RectTransform rectTransform;
		private Vector3 originalScale;

		// Properties

		// ClickAnimation

		public override void PressAnimation(Action callback)
		{
			rectTransform.DOKill();
			rectTransform.DOScale(originalScale * scaleFactor, scaleDownDuration).SetUpdate(true).OnComplete(() => callback?.Invoke());
		}

		public override void ReleaseAnimation(Action callback)
		{
			rectTransform.DOKill();
			rectTransform.DOScale(originalScale, scaleDownDuration).SetUpdate(true).OnComplete(() => callback?.Invoke());
		}

		public override void Init()
		{
			rectTransform = GetComponent<RectTransform>();
			originalScale = transform.localScale;
		}
	}	
}