using System;

namespace Framework.Core
{
	public interface IFocusManager
	{
		event Action<bool> OnAppPaused;
		event Action<bool> OnAppFocus;
		event Action OnAppQuit;
	}
}