using System;
using System.Collections.Generic;
using Framework.Pool;
using UnityEngine;
using SF = UnityEngine.SerializeField;

namespace Framework.Tools
{
    public class WaypointManager : GameObjectPool, IWaypointManager
    {
        [SF] private CanvasGroup canvasGroup;
        
        private List<TrackedWaypoint> trackedWaypoints = new();

        public void SetEnabled(bool value)
        {
            canvasGroup.alpha = value ? 1 : 0;
        }

        public void AddWaypoint(string tag, Transform trackedTransform, WaypointTracker.WaypointType waypointType)
        {
            if (WaypointExists(tag, trackedTransform))
                return;

            var newWaypointTracker = Take<WaypointTracker>();
            var newWaypoint = new TrackedWaypoint
            {
                waypointTag = tag,
                waypointTransform = trackedTransform,
                assignedTracker = newWaypointTracker
            };
            
            newWaypointTracker.Setup(trackedTransform, waypointType);
            
            trackedWaypoints.Add(newWaypoint);
        }

        public void RemoveWaypoint(string tag)
        {
            for (int i = 0; i < trackedWaypoints.Count; i++)
            {
                if (trackedWaypoints[i].waypointTag == tag)
                {
                    trackedWaypoints[i].assignedTracker.Disable();
                    trackedWaypoints.Remove(trackedWaypoints[i]);
                    return;
                }
            }
        }

        private bool WaypointExists(string tag, Transform trackedTransform)
        {
            for (int i = 0; i < trackedWaypoints.Count; i++)
            {
                if (trackedWaypoints[i].waypointTag == tag ||
                    trackedWaypoints[i].waypointTransform == trackedTransform)
                    return true;
            }

            return false;
        }
    }

    [Serializable]
    public class TrackedWaypoint
    {
        public string waypointTag;
        public Transform waypointTransform;
        public WaypointTracker assignedTracker;
    }
}
