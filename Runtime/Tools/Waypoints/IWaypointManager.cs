using UnityEngine;

namespace Framework.Tools
{
	public interface IWaypointManager
	{
		public void AddWaypoint(string tag, Transform trackedTransform, WaypointTracker.WaypointType waypointType);
		public void RemoveWaypoint(string tag);
		public void SetEnabled(bool value);
	}
}