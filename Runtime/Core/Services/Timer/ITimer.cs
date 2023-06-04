using System;

namespace Framework.Core
{
	public interface ITimer
	{
		ITimedAction Delay(float delay, Action done, bool realtime = true);
		ITimedAction Repeat(float frequency, int times, Action<int> onTick, float initDelay = 0f);
		void SkipFrame(Action done);
		void SkipFixedUpdate(Action done);
		ITimedAction WaitUntil(Func<bool> condition, Action done);
	}
}