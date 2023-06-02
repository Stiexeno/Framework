using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using SF = UnityEngine.SerializeField;

namespace Framework.UI
{
    public class Toggle : MonoBehaviour, IPointerClickHandler
    {
        [SF] private bool toggled = false;

        [SF] private float            toggleDuration  = 0.2f;
        
        [SF] private Image            slidingFill;

        [SF] private Ease             toggleTweenEase = Ease.InOutQuad;

        [SF] private RectTransform    handle;
        [SF] private RectTransform    onPosition;
        [SF] private RectTransform    offPosition;

        // Private fields

        private bool    togglingTransition = false;
        private bool    lastOnState;

        // Events

        public event Action<bool> OnToggled;
        
        // Toggle

        public void SetState (bool on)
        {
            toggled = on;
            handle.anchoredPosition = on ? onPosition.anchoredPosition : offPosition.anchoredPosition;

            slidingFill.fillAmount = on ? 1f : 0f;
            handle.anchoredPosition = on ? onPosition.anchoredPosition : offPosition.anchoredPosition;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!togglingTransition)
            {
                togglingTransition = true;

                handle.DOAnchorPosX(toggled ? offPosition.anchoredPosition.x : onPosition.anchoredPosition.x, toggleDuration)
                    .SetEase(toggleTweenEase)
                    .OnComplete(() =>
                    {
                        togglingTransition = false;
                        SetState(!toggled);
                        OnToggled?.Invoke(toggled);
                    }).SetUpdate(true);

                if (slidingFill)
                    slidingFill.DOFillAmount(toggled ? 0f : 1f, toggleDuration).SetEase(toggleTweenEase).SetUpdate(true);
            }
        }

        public void Init()
        {
            SetState(toggled);
            lastOnState = toggled;
            slidingFill.fillAmount = toggled ? 0f : 1f;
        }

        private void Update()
        {
            if (lastOnState != toggled)
            {
                SetState(toggled);
                lastOnState = toggled;
            }
        }
    }
}