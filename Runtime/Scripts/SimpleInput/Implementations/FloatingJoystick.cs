using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using SF = UnityEngine.SerializeField;

namespace Framework.SimpleInput
{
    [RequireComponent(typeof(Image))]
    public class FloatingJoystick : InputHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        [SF] private float movementRange = 50;
        [SF] private bool fixedPosition = false;
        [SF] private RectTransform stick;

        // Private

        private Vector2 startPosition;
        private Vector2 pointerDownPosition;

        private RectTransform inputRect;
        private RectTransform stickHolder;

        // Floating Joystick

        public void OnPointerDown(PointerEventData eventData)
        {
            if (InputSystem.Enabled == false)
                return;
            
            stickHolder.gameObject.SetActive(true);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(inputRect, eventData.position, eventData.pressEventCamera, out pointerDownPosition);
            stickHolder.anchoredPosition = pointerDownPosition;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (InputSystem.Enabled == false)
                return;
            
            RectTransformUtility.ScreenPointToLocalPointInRectangle(inputRect, eventData.position, eventData.pressEventCamera, out var position);

            var delta = position - pointerDownPosition;
            var clampedDelta = Vector2.ClampMagnitude(delta, movementRange);

            stick.anchoredPosition = startPosition + (fixedPosition ? clampedDelta : delta);

            Direction = new Vector2(clampedDelta.x / movementRange, clampedDelta.y / movementRange);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (InputSystem.Enabled == false)
                return;
            
            stickHolder.gameObject.SetActive(false);
            stick.anchoredPosition = startPosition;
            Direction = Vector2.zero;
        }
        
        // MonoBehaviour
        
        private void Awake()
        {
            inputRect = transform.GetComponent<RectTransform>();

            stickHolder = (RectTransform)stick.parent;
            startPosition = stick.anchoredPosition;
            stickHolder.gameObject.SetActive(false);
        }
    }
}
