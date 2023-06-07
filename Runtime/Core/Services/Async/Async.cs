using System;
using System.Collections;
using DG.Tweening;
using Framework.Core;
using UnityEngine;

namespace Framework.Utils
{
	public static class Async
	{
		private static ICoroutineManager coroutineManager;
		
		private static ICoroutineManager CoroutineManager
		{
			get
			{
				if (coroutineManager == null)
				{
					coroutineManager = ProjectContext.DiContainer.Resolve<ICoroutineManager>();
				}

				return coroutineManager;
			}
		}
		
		public static void WaitUntil(Func<bool> condition, Action onComplete)
		{
			CoroutineManager.Begin(WaitUntilCoroutine(condition, onComplete));
		}

		public static Coroutine SkipFrame(Action onComplete)
		{
			return CoroutineManager.Begin(SkipFrameCoroutine(onComplete));
		}

		public static void Delay(float duration, Action onComplete)
		{
			DOVirtual.DelayedCall(duration, () =>
			{
				onComplete?.Invoke();
			});
		}

		public static void Kill(Coroutine routine)
		{
			CoroutineManager.Stop(ref routine);
		}

		private static IEnumerator WaitUntilCoroutine(Func<bool> condition, Action onComplete)
		{
			yield return new WaitUntil(condition);
			onComplete?.Invoke();
		}
        
		private static IEnumerator SkipFrameCoroutine(Action onComplete)
		{
			yield return new WaitForEndOfFrame();
			onComplete?.Invoke();
		}

		private static IEnumerator DelayCoroutine(float duration, Action onComplete)
		{
			yield return new WaitForSeconds(duration);
			onComplete?.Invoke();
		}
	}
}