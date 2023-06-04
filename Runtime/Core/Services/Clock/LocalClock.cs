using System;

namespace Framework.Core
{
	public class LocalClock : IClock
	{
		public long LastTimeActive => data.lastTimeActive;
		public long Now => DateTimeOffset.UtcNow.ToUnixTimeSeconds();
		public long NowMilliseconds => DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
		public long TimeInactive => Now - LastTimeActive;
		
		private LocalClockData data;
		
		// LocalClock
		
		public LocalClock(IDataManager dataManager, IFocusManager focusManager)
		{
			data = dataManager.GetOrCreateData<LocalClockData>();
			
			if (data.lastTimeActive == 0)
			{
				data.lastTimeActive = Now;
			}

			focusManager.OnAppFocus += focus =>
			{
				if (focus == false)
				{
					data.lastTimeActive = Now;
					dataManager.SaveData(data);
				}
			};

			focusManager.OnAppPaused += paused =>
			{
				if (paused)
				{
					data.lastTimeActive = Now;
					dataManager.SaveData(data);
				}
			};

			focusManager.OnAppQuit += () =>
			{
				data.lastTimeActive = Now;
				dataManager.SaveData(data);
			};
		}
	}
}