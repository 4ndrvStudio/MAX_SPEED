namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;
    using UnityEngine.Events;

    [System.Serializable]
    public struct CarAIOnReachWaypointInfo
    {
        public CarAIWaypointRoute parentRoute;
        public int waypointIndexnumber;
        public bool useSpeedLimit;
        public float speedLimit;
        public bool useBrakeTrigger;
        public bool releaseBrakeWhenStopped;
        [Range(0.01f, 1f)] public float brakeAmount;
        public bool stopDriving;
        public float distanceFromStart;
        public CarAIWaypoint[] newRoutePoints;
        public UnityEvent OnReachWaypointEvent;
        public int[] desiredRouteIndexes;
    }
}