using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using SF = UnityEngine.SerializeField;

namespace Framework.UI
{
    public class HoldableButton : Button
    {
        [SF] private Image fillImage;

        private float holdProgress;
        private bool  isHolding;

        // Button
        
        public override void OnPointerDown(PointerEventData eventData)
        {
            if (Interactable)
            {
                isHolding = true;
            }
            
            base.OnPointerDown(eventData);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            if (Interactable)
            {
                isHolding = false;

                if (holdProgress < 1f)
                    return;
            }
            
            base.OnPointerUp(eventData);
        }

        protected override void Process()
        {
            holdProgress += isHolding && MouseOverObject && HeldDown ? Time.unscaledDeltaTime : -Time.unscaledDeltaTime;

            holdProgress = Mathf.Clamp01(holdProgress);

            fillImage.fillAmount = holdProgress;

            if (holdProgress >= 1f)
            {
                OnPointerUp(new PointerEventData(EventSystem.current));
                holdProgress = 0f;
            }
        }
    }
}