using UnityEngine;

namespace Framework.Core
{
	public class Widget : MonoBehaviour
	{
		protected Dialog parentDialog;
		protected IUIManager UIManager { get; private set; }

		internal void Init(IUIManager uiManager, Dialog parentDialog)
		{
			this.parentDialog = parentDialog;
			UIManager = uiManager;
			
			parentDialog.OnOpened += OnDialogOpen;
			parentDialog.OnClosed += OnDialogClose;
			parentDialog.OnBeforeOpened += OnDialogBeforeOpen;
			parentDialog.OnBeforeClosed += OnDialogBeforeClose;

			OnInit();
		}
		
		internal void Dispose()
		{
			OnDispose();
		}

		protected virtual void OnInit()
		{
		}

		protected virtual void OnDispose()
		{
		}

		protected virtual void OnDialogBeforeOpen()
		{
		}

		protected virtual void OnDialogBeforeClose()
		{
		}

		protected virtual void OnDialogOpen()
		{
		}

		protected virtual void OnDialogClose()
		{
		}
	}   
}

