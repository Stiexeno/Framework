using System;
using UnityEngine;
using DG.Tweening;
using SF = UnityEngine.SerializeField;

namespace Framework.UI
{
    [RequireComponent(typeof(RectTransform),typeof(CanvasGroup)), DisallowMultipleComponent]
    public class DefaultButtonAnimation : ButtonAnimation
    {
        [SF] protected float scaleDownDuration = 0.06f;
        [SF] protected float scaleUpDuration = 0.06f;
        
        [SF] protected float scaleFactor = 0.9f;

        private Vector3 originalScale;
        private RectTransform rectTransform;
        private CanvasGroup canvasGroup;

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

        public override void SetInteractable(bool interactable)
        {
            canvasGroup.interactable = interactable;
            canvasGroup.alpha = interactable ? 1f : 0.6f;
        }

        public override void Init()
        {
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
            
            originalScale = transform.localScale;
        }
    }
}
