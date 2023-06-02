using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using SF = UnityEngine.SerializeField;

namespace Framework.UI
{
    [RequireComponent(typeof(RectTransform),typeof(CanvasGroup))]
    public class SpriteSwitchButtonAnimation : ButtonAnimation
    {
        [SF] private Sprite idleSprite;
        [SF] private Sprite clickedSprite;
        [SF] private Vector2 moveOffset;
        [SF] private float moveTime;

        private Vector2[] originalRectPositions;

        private Image buttonImage;
        private Vector3 originalScale;

        public override void PressAnimation(Action callback)
        {
	        var index = 0;
	        foreach (Transform child in transform)
	        {
		        var childRect = child.transform as RectTransform;
		        childRect.DOKill();
		        
		        if (originalRectPositions[index] == default)
		        {
			        originalRectPositions[index] = childRect.anchoredPosition;   
		        }
		        
		        childRect.DOAnchorPos(originalRectPositions[index] + moveOffset, moveTime).SetEase(Ease.Linear);
		        index++;
	        }
  
            buttonImage.sprite = clickedSprite;
            callback?.Invoke();
        }
        
        public override void ReleaseAnimation(Action callback)
        {
	        var index = 0;
	        foreach (Transform child in transform)
	        {
		        var childRect = child.transform as RectTransform;
		        childRect.DOKill();
		        childRect.DOAnchorPos(originalRectPositions[index], moveTime).SetEase(Ease.Linear);
		        index++;
	        }

	        buttonImage.sprite = idleSprite;
            callback?.Invoke();
        }

        public override void Init()
        {
            buttonImage = GetComponent<Image>();

            originalRectPositions = new Vector2[transform.childCount];
        }
    }
}