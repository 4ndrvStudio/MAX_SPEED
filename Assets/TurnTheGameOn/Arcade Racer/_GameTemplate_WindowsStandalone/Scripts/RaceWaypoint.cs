namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;

    public class RaceWaypoint : MonoBehaviour
    {
        public int waypointNumber;

        void OnTriggerEnter(Collider col)
        {
            for (int i = 0; i < RaceManager.Instance.racerInformation.Count; i++)
            {
                if (col.transform.root.name == RaceManager.Instance.racerInformation[i].racerName)
                {
                    RaceManager.Instance.ChangeTarget(i, waypointNumber);
                }
            }
        }
    }
}