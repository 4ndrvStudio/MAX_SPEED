namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "CarAIWaypointRouteSettings", menuName = "TurnTheGameOn/Arcade Racer/CarAIWaypointRouteSettings")]
    public class CarAIWaypointRouteSettings : ScriptableObject
    {
        public Color pathColor = new Color(0, 1, 0, 0.298f);
        public Color selectedPathColor = new Color(0, 1, 0, 1);
        public Color junctionColor = new Color(1, 1, 0, 0.298f);
        public Color selectedJunctionColor = new Color(1, 1, 0, 1);
        public bool alwaysDrawGizmos = true;
        public float arrowHeadWidth = 10.0f;
        public float arrowHeadLength = 2.0f;
        public Vector3 arrowHeadScale = new Vector3(1, 0, 1);
        public float updateGizmoCoolDown = 0.1f;
        public bool canUpdateGizmos;
    }
}