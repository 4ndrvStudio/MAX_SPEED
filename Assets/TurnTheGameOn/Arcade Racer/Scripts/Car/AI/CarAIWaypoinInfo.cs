namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;
    [System.Serializable]
    public struct CarAIWaypoinInfo
    {
        public string _name;
        public Transform _transform;
        public CarAIWaypoint _waypoint;
    }
}