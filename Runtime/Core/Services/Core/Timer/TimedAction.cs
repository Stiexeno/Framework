using UnityEngine;
using SF = UnityEngine.SerializeField;

namespace Framework.Core
{
	public class TimedAction : ITimedAction
	{
		private Coroutine coroutine;
		private ICoroutineManager coroutineManager;

		public TimedAction(Coroutine coroutine, ICoroutineManager coroutineManager)
		{
			this.coroutineManager = coroutineManager;
			this.coroutine = coroutine;
		}
		
		public void Cancel()
		{
			coroutineManager.Stop(ref coroutine);
		}
	}
}