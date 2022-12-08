namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;

    [System.Serializable]
    public class RaceManager_Circuit_SpawnPoint
    {
        public string name;
        public Transform transform;
        public CarAIWaypointRoute startingAIRoute;
        public bool customResetPoint;
        public CarAIWaypointRoute stuckResetRoute;
        public int stuckResetPoint;
    }
}