using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Framework.Core
{
	public sealed class UIManager : MonoBehaviour, IUIManager, IProcessable, ITickable
	{
        public Camera Camera { get; private set; }
        public Canvas Canvas { get; private set; }
        public EventSystem EventSystem { get; private set; }

        private readonly Dictionary<Type, IDialog> dialogs = new();
        private readonly List<IProcessable> processedDialogs = new();
        private readonly List<ITickable> tickableDialogs = new();

        [Inject]
        public void Initialize()
        {
            foreach (var dialog in GetComponentsInChildren<IDialog>(true))
            {
                dialogs.Add(dialog.GetType(), dialog);
            }

            foreach (var dialog in dialogs)
            {
                dialog.Value.Init(this);

                if (dialog.Value is IProcessable proc)
                {
                    processedDialogs.Add(proc);
                }

                if (dialog.Value is ITickable tickable)
                {
                    tickableDialogs.Add(tickable);
                }
            }

            Camera = GetComponentInChildren<Camera>();
            Canvas = GetComponentInChildren<Canvas>();
            EventSystem = GetComponentInChildren<EventSystem>();
        }

        private void OnDestroy()
        {
            foreach (var win in dialogs)
            {
                win.Value.Dispose();
            }
        }

        public T GetDialog<T>() where T : class, IDialog
        {
            var wType = typeof(T);

            if (dialogs.TryGetValue(wType, out IDialog dialog))
            {
                return (T)dialog;
            }

            foreach (var w in dialogs)
            {
                if (w.Value is T value)
                {
                    dialogs.Add(wType, w.Value);
                    return value;
                }
            }

            Debug.LogError($"No Dialog {typeof(T).Name} is available!");
            return default;
        }

        public bool TryGetDialog<T>(out T dialog) where T : class, IDialog
        {
            var wType = typeof(T);

            if (dialogs.TryGetValue(wType, out IDialog win))
            {
                dialog = (T)win;
                return true;
            }

            foreach (var w in dialogs)
            {
                if (w.Value is T value)
                {
                    dialogs.Add(wType, w.Value);
                    dialog = value;
                    return true;
                }
            }

            dialog = default;
            return false;
        }

        public bool IsDialogReady<T>() where T : class, IDialog
        {
            var wType = typeof(T);

            if (dialogs.TryGetValue(wType, out IDialog _))
            {
                return true;
            }

            foreach (var w in dialogs)
            {
                if (w.Value is T)
                {
                    dialogs.Add(wType, w.Value);
                    return true;
                }
            }

            return false;
        }

        public void OpenDialog<T>(Action callBack = null, Action onHidden = null, Action onBeforeHide = null) where T : class, IDialog
        {
            var dialog = GetDialog<T>();

            if (dialog.Initialized == false)
                dialog.Init(this);

            dialog.Open(callBack, onHidden, onBeforeHide);
        }

        public void CloseDialog<T>(Action callBack = null) where T : class, IDialog
        {
            GetDialog<T>().Close(callBack);
        }

        void IProcessable.Process(in float delta)
        {
            foreach (var processed in processedDialogs)
            {
                processed.Process(delta);
            }
        }

        void ITickable.Tick()
        {
            foreach (var tickable in tickableDialogs)
            {
                tickable.Tick();
            }
        }
        
        public bool IsPointerOverUI()
        {
            return EventSystem.IsPointerOverGameObject(0);
        }
        
        public Vector2 WorldToCanvas(Vector3 worldPosition)
        {
            var viewportPosition = Camera.WorldToViewportPoint(worldPosition);
            var canvasRect = Canvas.transform.RectTransform();

            return new Vector2((viewportPosition.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f),
                (viewportPosition.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f));
        }
	}   
}

