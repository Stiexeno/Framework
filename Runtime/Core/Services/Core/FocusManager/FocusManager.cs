using System;
using UnityEngine;

namespace Framework.Core
{
	public class FocusManager : MonoBehaviour, IFocusManager
	{
		public event Action<bool> OnAppPaused;
		public event Action<bool> OnAppFocus;
		public event Action OnAppQuit;
		
		private void OnApplicationPause(bool paused)
		{
			OnAppPaused?.Invoke(paused);
		}

		private void OnApplicationFocus(bool focus)
		{
			OnAppFocus?.Invoke(focus);
		}

		private void OnApplicationQuit()
		{
			OnAppQuit?.Invoke();
		}
	}   
}
