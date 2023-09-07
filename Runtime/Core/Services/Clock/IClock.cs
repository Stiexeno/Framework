using SF = UnityEngine.SerializeField;

namespace Framework.Core
{
	public interface IClock
	{
		public long LastTimeActive { get; }
		public long Now { get; }
		public long NowMilliseconds { get; }
		public long TimeInactive { get; }
	}
   
}