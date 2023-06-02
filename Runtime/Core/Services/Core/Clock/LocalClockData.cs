using System;

namespace Framework.Core
{
	[Serializable]
	public class LocalClockData : IData
	{
		public long lastTimeActive;
	}	
}