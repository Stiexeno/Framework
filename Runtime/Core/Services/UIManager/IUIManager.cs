using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Framework.Core
{
	public interface IUIManager
	{
		Camera Camera { get; }
		Canvas Canvas { get; }
		EventSystem EventSystem { get; }
		T GetDialog<T>() where T : class, IDialog;
		bool TryGetDialog<T>(out T dialog) where T : class, IDialog;
		bool IsDialogReady<T>() where T : class, IDialog;

		void OpenDialog<T>(Action callBack = null, Action onHidden = null, Action onBeforeHide = null) where T : class, IDialog;
		
		void CloseDialog<T>(Action callBack = null) where T : class, IDialog;
		bool IsPointerOverUI();
		Vector2 WorldToCanvas(Vector3 worldPosition);
	}
}