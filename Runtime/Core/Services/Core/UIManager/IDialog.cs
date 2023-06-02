using System;

namespace Framework.Core
{
	public interface IDialog
	{
		internal void Init(IUIManager uiManager);
		internal void Dispose();

		bool isOpened { get; }
		bool Initialized { get; }

		event Action OnOpened;
		event Action OnClosed;
		event Action OnBeforeOpened;
		event Action OnBeforeClosed;

		void Open();
		void Open(Action onOpen, Action onClose = null, Action onBeforeClose = null);
		void ForwardTo<T>(bool waitUntilOpened = false) where T : class, IDialog;
		void Close();
		void Close(Action onClose);
	}
}