using System;

namespace Framework.Core
{
	public interface IDialogAnimation
	{
		void Init();
		void OnOpen(Action done);
		void OnClose(Action done);
	}   
}

