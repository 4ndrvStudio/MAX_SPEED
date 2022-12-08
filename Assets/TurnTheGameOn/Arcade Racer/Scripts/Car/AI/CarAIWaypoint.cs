namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;

    public class CarAIWaypoint : MonoBehaviour
    {
        public CarAIOnReachWaypointInfo onReachWaypointSettings;
        public CarAIWaypoint[] junctionPoint;

        void OnTriggerEnter(Collider col)
        {
            col.transform.root.SendMessage("OnReachedWaypoint", onReachWaypointSettings, SendMessageOptions.DontRequireReceiver);
            if (onReachWaypointSettings.waypointIndexnumber == onReachWaypointSettings.parentRoute.waypointDataList.Count)
            {
                if (onReachWaypointSettings.newRoutePoints.Length == 0)
                {
                    col.transform.root.SendMessage("StopDriving", SendMessageOptions.DontRequireReceiver);
                }
            }
        }

        public void TriggerNextWaypoint(CarAI carAI)
        {
            carAI.OnReachedWaypoint(onReachWaypointSettings);
            if (onReachWaypointSettings.waypointIndexnumber == onReachWaypointSettings.parentRoute.waypointDataList.Count)
            {
                if (onReachWaypointSettings.newRoutePoints.Length == 0)
                {
                    carAI.StopDriving();
                }
            }
        }
    }

}