using System;
using System.Collections;
using UnityEngine;
using SF = UnityEngine.SerializeField;

namespace Framework.Core
{
	public class Timer : ITimer
	{
		private readonly WaitForEndOfFrame skipFrame = new();
		private readonly WaitForFixedUpdate skipFixedUpdate = new();

		private ICoroutineManager coroutineManager;
		private IInstantiator instantiator;

		public Timer(ICoroutineManager coroutineManager, IInstantiator instantiator)
		{
			this.instantiator = instantiator;
			this.coroutineManager = coroutineManager;
		}

		public ITimedAction Delay(float delay, Action done, bool realtime = true)
		{
			return instantiator.Instantiate<TimedAction>(coroutineManager.Begin(DelayCoroutine(delay, done, realtime)));
		}

		public ITimedAction Repeat(float frequency, int times, Action<int> onTick, float initDelay = 0f)
		{
			return instantiator.Instantiate<TimedAction>(coroutineManager.Begin(RepeatCoroutine(frequency, times, initDelay, onTick)));
		}

		public void SkipFrame(Action done)
		{
			coroutineManager.Begin(SkipFrameCoroutine(done));
		}

		public void SkipFixedUpdate(Action done)
		{
			coroutineManager.Begin(SkipFixedUpdateCoroutine(done));
		}

		public ITimedAction WaitUntil(Func<bool> condition, Action onComplete)
		{
			return instantiator.Instantiate<TimedAction>(coroutineManager.Begin(WaitUntilCoroutine(condition, onComplete)));
		}

		private IEnumerator DelayCoroutine(float delay, Action done, bool realtime = true)
		{
			if (realtime)
				yield return new WaitForSecondsRealtime(delay);
			else
				yield return new WaitForSeconds(delay);

			done();
		}

		private IEnumerator RepeatCoroutine(float frequency, int times, float initDelay, Action<int> onTick)
		{
			yield return new WaitForSeconds(initDelay);

			for (int i = 0; i < times; i++)
			{
				onTick?.Invoke(i);
				yield return new WaitForSeconds(frequency);
			}
		}

		private IEnumerator SkipFrameCoroutine(Action done)
		{
			yield return skipFrame;
			done();
		}

		private IEnumerator SkipFixedUpdateCoroutine(Action done)
		{
			yield return skipFixedUpdate;
			done();
		}

		private static IEnumerator WaitUntilCoroutine(Func<bool> condition, Action onComplete)
		{
			yield return new WaitUntil(condition);
			onComplete?.Invoke();
		}
	}
}