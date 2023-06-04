using System;
using Framework.UI;
using UnityEngine;
using SF = UnityEngine.SerializeField;

namespace Framework.Core
{
	public class Dialog : MonoBehaviour, IDialog
	{
#if UNITY_STANDALONE == false
        [SerializeField] protected bool ignoreSafeArea = false;
#endif
		[SerializeField] protected Button closeButton;

		private IDialogAnimation transition;
		private Widget[] widgets = { };

		public event Action OnOpened;
		public event Action OnClosed;
		public event Action OnBeforeOpened;
		public event Action OnBeforeClosed;

		public bool isOpened { get; private set; }
		public bool Initialized { get; private set; }
		
		private RectTransform rect;
		private Action hiddenCallback;
		private Action beforeHideCallback;

		protected IUIManager UIManager { get; private set; }

		void IDialog.Init(IUIManager uiManager)
		{
			if (Initialized)
				return;

			UIManager = uiManager;

			if (TryGetComponent(out IDialogAnimation t))
			{
				transition = t;
				transition.Init();
			}

			rect = transform as RectTransform;

#if UNITY_STANDALONE == false
            if (ignoreSafeArea == false)
                ApplySafeArea();
#endif
			widgets = GetComponentsInChildren<Widget>(true);

			foreach (var widget in widgets)
			{
				widget.Init(uiManager, this);
			}

			if (closeButton)
				closeButton.OnClick += Close;

			isOpened = gameObject.activeInHierarchy;

			OnInit();

			Initialized = true;
		}

		void IDialog.Dispose()
		{
			OnDispose();

			foreach (var widget in widgets)
			{
				widget.Dispose();
			}
		}
		
		protected virtual void OnInit()
		{
		}

		protected virtual void OnDispose()
		{
		}

		public void Open()
		{
			Open(() => { });
		}

		public void Open(Action done, Action onHidden = null, Action onBeforeHide = null)
		{
			gameObject.SetActive(true);

			OnBeforeOpened?.Invoke();
			OnBeforeOpen();

			hiddenCallback += onHidden;
			beforeHideCallback += onBeforeHide;

			if (transition != null)
			{
				transition.OnOpen(() =>
				{
					OnOpen();
					OnOpened?.Invoke();

					isOpened = true;
					done?.Invoke();
				});
			}
			else
			{
				OnOpen();
				OnOpened?.Invoke();

				isOpened = true;
				done?.Invoke();
			}
		}

		public void ForwardTo<T>(bool waitUntilHidden = true) where T : class, IDialog
		{
			var window = UIManager.GetDialog<T>();

			if (waitUntilHidden)
			{
				Close(() => { window.Open(null, onClose: Open); });
			}
			else
			{
				Close();
				window.Open(null, onBeforeClose: Open);
			}
		}

		public void Close()
		{
			Close(() => { });
		}

		public void Close(Action done)
		{
			OnBeforeClosed?.Invoke();
			OnBeforeClose();

			beforeHideCallback?.Invoke();
			beforeHideCallback = null;

			if (transition != null)
			{
				transition.OnClose(() =>
				{
					gameObject.SetActive(false);
					OnClose();
					OnClosed?.Invoke();

					hiddenCallback?.Invoke();
					hiddenCallback = null;

					isOpened = false;
					done?.Invoke();
				});
			}
			else
			{
				gameObject.SetActive(false);
				OnClose();
				OnClosed?.Invoke();

				hiddenCallback?.Invoke();
				hiddenCallback = null;

				isOpened = false;
				done?.Invoke();
			}
		}

		public T GetWidget<T>() where T : Widget
		{
			foreach (var w in widgets)
			{
				if (w is T widget)
				{
					return widget;
				}
			}

			Debug.LogError($"Widget {nameof(T)} not found");
			return default;
		}

		protected virtual void OnBeforeOpen()
		{
		}

		protected virtual void OnBeforeClose()
		{
		}

		protected virtual void OnOpen()
		{
		}

		protected virtual void OnClose()
		{
		}

#if UNITY_STANDALONE == false
        // NOTE: This might apply safe area wrongly in Game view (it works on DeviceSimulator though)
        private void ApplySafeArea()
        {
            var rootCanvas = GetComponentInParent<Canvas>();

            Rect safeArea = Screen.safeArea;

            Vector2 anchorMin = safeArea.position;
            Vector2 anchorMax = safeArea.position + safeArea.size;

            anchorMin.x /= rootCanvas.pixelRect.width;
            anchorMin.y /= rootCanvas.pixelRect.height;
            anchorMax.x /= rootCanvas.pixelRect.width;
            anchorMax.y /= rootCanvas.pixelRect.height;

            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
        }
#endif
	}
}