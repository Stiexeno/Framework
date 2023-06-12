using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Serialization;
using SF = UnityEngine.SerializeField;

namespace Framework.Tools
{
    public class WaypointTracker : MonoBehaviour
    {
        [SF] private Camera uiCamera;
        [SF] private Camera gameCamera;
        [SF] private Image  mainImage;
        [SF] private Sprite dotImage;

        [SF] private CanvasGroup canvasGroup;
        [SF] private float screenWaypointOffset = 70f;
        [SF] private float lowerScreenOffset = 250f;
        [SF] private float upperScreenOffset = 250f;

        private Transform trackedTransform;
        private bool isVisible;

        public enum WaypointType
        {
            Simple,
            Home,
            Objective
        }

        public void Disable()
        {
            trackedTransform = null;
            ToggleVisibility(false, () =>
            {
                gameObject.SetActive(false);
            });
        }

        public void Setup(Transform trackedTransform, WaypointType waypointType)
        {
            this.trackedTransform = trackedTransform;
            
            switch (waypointType)
            {
                case WaypointType.Objective:
                    mainImage.sprite = dotImage;
                    break;
                case WaypointType.Simple:
                    mainImage.sprite = dotImage;
                    break;
                case WaypointType.Home:
                    mainImage.sprite = dotImage;
                    break;
            }
            
            canvasGroup.DOKill();
            canvasGroup.alpha = 1f;
            isVisible = true;
            gameObject.SetActive(true);
        }

        private void ToggleVisibility(bool value, Action onDone = null)
        {
            if (value == isVisible)
                return;

            if(value)
                gameObject.SetActive(true);

            canvasGroup.DOKill();
            canvasGroup.DOFade(value ? 1f : 0f, 0.5f).OnComplete(() => onDone?.Invoke());
            
            isVisible = value;
        }
        
        private void TrackObject(Transform objTransform)
        {
            var convertedData = ConvertWorldToUISpace(objTransform.position);
            if (convertedData.onScreen)
            {
                ToggleVisibility(false);
            }
            else
            {
                ToggleVisibility(true);
                transform.position = convertedData.clampedToScreenPosition;

                var centerPosition = 
                    uiCamera.ScreenToWorldPoint(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 5));

                var dir = transform.position - centerPosition;
                var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
            }
        }
        
        private (Vector3 convertedPosition, Vector3 clampedToScreenPosition, bool onScreen) ConvertWorldToUISpace(Vector3 position)
        {
            var worldToScreen = gameCamera.WorldToScreenPoint(position);
            if (worldToScreen.z < 0) worldToScreen = -worldToScreen;

            var screenToWorld = uiCamera.ScreenToWorldPoint(worldToScreen);

            var xOnScreen = worldToScreen.x < Screen.width && worldToScreen.x > 0;
            var yOnScreen = worldToScreen.y < Screen.height && worldToScreen.y > 0;

            var clampedX = Mathf.Clamp(worldToScreen.x, screenWaypointOffset, Screen.width - screenWaypointOffset);
            var clampedY = Mathf.Clamp(worldToScreen.y, lowerScreenOffset + screenWaypointOffset, Screen.height - screenWaypointOffset - upperScreenOffset);

            var clampedToScreen = uiCamera.ScreenToWorldPoint(new Vector3(clampedX, clampedY, 5f));
            
            return (screenToWorld, clampedToScreen, xOnScreen && yOnScreen);
        }
        
        private void Update()
        {
            if(trackedTransform)
                TrackObject(trackedTransform);
        }
    }
}

