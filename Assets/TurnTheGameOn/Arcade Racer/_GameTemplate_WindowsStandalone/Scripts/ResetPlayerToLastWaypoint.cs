namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;

    public class ResetPlayerToLastWaypoint : MonoBehaviour
    {
        public KeyCode resetPlayerKey = KeyCode.R;

        void Update()
        {
            if (Input.GetKeyDown(resetPlayerKey))
            {
                ResetPlayer();
            }
        }

        public void ResetPlayer()
        {
            Transform currentPoint = RaceManager.Instance.racerInformation[RaceManager.Instance.playerIndex].currentWaypoint;
            Transform previousPoint = RaceManager.Instance.racerInformation[RaceManager.Instance.playerIndex].previousWaypoint;

            RaceManager.Instance.racerInformation[RaceManager.Instance.playerIndex].racerObject.transform.position = previousPoint.transform.position;
            RaceManager.Instance.racerInformation[RaceManager.Instance.playerIndex].racerObject.transform.LookAt(currentPoint);
        }
    }
}