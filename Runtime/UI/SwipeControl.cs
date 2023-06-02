using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using SF = UnityEngine.SerializeField;

namespace Framework.Utils
{
    public struct SwipeAction
    {
        public SwipeDirection direction;
        public Vector2 rawDirection;
        public Vector2 startPosition;
        public Vector2 endPosition;
        public float startTime;
        public float endTime;
        public float duration;
        public bool longPress;
        public float distance;
        public float longestDistance;

        public override string ToString()
        {
            return $"[SwipeAction: {direction}, From {rawDirection}, To {startPosition}, Delta {endPosition}, Time {duration:0.00}s]";
        }
    }

    public enum SwipeDirection
    {
        None, // Invalid swipe
        Up,
        UpRight,
        Right,
        DownRight,
        Down,
        DownLeft,
        Left,
        UpLeft
    }

    public class SwipeControl : MonoBehaviour
    {
        [Range(0f, 200f)] [SF]
        private float minSwipeLength = 100f;

        private Vector2 currentSwipe;
        private SwipeAction currentSwipeAction = new SwipeAction();

        private bool reset = true;
        private bool swipped = false;

        // Events

        public event Action<SwipeAction> OnSwipe;

        // SwipeControl

        private void DetectSwipe()
        {
            if (Input.GetMouseButton(0))
            {
                PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
                eventDataCurrentPosition.position = Input.mousePosition;

                List<RaycastResult> results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

                if (results.Any(m => m.gameObject == gameObject))
                {
                    if (reset)
                    {
                        ResetCurrentSwipeAction(Input.mousePosition);
                        reset = false;
                    }

                    if (swipped)
                        return;

                    UpdateCurrentSwipeAction(Input.mousePosition);

                    // Make sure it was a legit swipe, not a tap, or long press
                    if (currentSwipeAction.distance < minSwipeLength || currentSwipeAction.longPress) // Didnt swipe enough or this is a long press
                    {
                        currentSwipeAction.direction = SwipeDirection.None; // Invalidate current swipe action
                        return;
                    }

                    if (OnSwipe != null)
                    {
                        OnSwipe?.Invoke(currentSwipeAction); // Fire event
                        swipped = true;
                    }
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
	            swipped = false;
	            Reset();
            }
        }

        public void Reset()
        {
	        reset = true;
        }

        private void ResetCurrentSwipeAction(Vector2 position)
        {
            currentSwipeAction.duration = 0f;
            currentSwipeAction.distance = 0f;
            currentSwipeAction.longestDistance = 0f;
            currentSwipeAction.longPress = false;
            currentSwipeAction.startPosition = position;
            currentSwipeAction.startTime = Time.time;
            currentSwipeAction.endPosition = currentSwipeAction.startPosition;
            currentSwipeAction.endTime = currentSwipeAction.startTime;
        }

        private void UpdateCurrentSwipeAction(Vector2 position)
        {
            currentSwipeAction.endPosition = position;
            currentSwipeAction.endTime = Time.time;
            currentSwipeAction.duration = currentSwipeAction.endTime - currentSwipeAction.startTime;

            currentSwipe = currentSwipeAction.endPosition - currentSwipeAction.startPosition;

            currentSwipeAction.rawDirection = currentSwipe;
            currentSwipeAction.direction = GetSwipeDirection(currentSwipe);
            currentSwipeAction.distance = Vector2.Distance(currentSwipeAction.startPosition, currentSwipeAction.endPosition);

            if (currentSwipeAction.distance > currentSwipeAction.longestDistance) // If new distance is longer than previously longest
            {
                currentSwipeAction.longestDistance = currentSwipeAction.distance; // Update longest distance
            }
        }

        private SwipeDirection GetSwipeDirection(Vector2 direction)
        {
            var angle = Vector2.Angle(Vector2.up, direction.normalized); // Degrees
            var swipeDirection = SwipeDirection.None;

            if (direction.x > 0) // Right
            {
                if (angle < 22.5f) // 0.0 - 22.5
                {
                    swipeDirection = SwipeDirection.Up;
                }
                else if (angle < 67.5f) // 22.5 - 67.5
                {
                    swipeDirection = SwipeDirection.UpRight;
                }
                else if (angle < 112.5f) // 67.5 - 112.5
                {
                    swipeDirection = SwipeDirection.Right;
                }
                else if (angle < 157.5f) // 112.5 - 157.5
                {
                    swipeDirection = SwipeDirection.DownRight;
                }
                else if (angle < 180.0f) // 157.5 - 180.0
                {
                    swipeDirection = SwipeDirection.Down;
                }
            }
            else // Left
            {
                if (angle < 22.5f) // 0.0 - 22.5
                {
                    swipeDirection = SwipeDirection.Up;
                }
                else if (angle < 67.5f) // 22.5 - 67.5
                {
                    swipeDirection = SwipeDirection.UpLeft;
                }
                else if (angle < 112.5f) // 67.5 - 112.5
                {
                    swipeDirection = SwipeDirection.Left;
                }
                else if (angle < 157.5f) // 112.5 - 157.5
                {
                    swipeDirection = SwipeDirection.DownLeft;
                }
                else if (angle < 180.0f) // 157.5 - 180.0
                {
                    swipeDirection = SwipeDirection.Down;
                }
            }

            return swipeDirection;
        }

        private void Update()
        {
            DetectSwipe();
        }
    }
}