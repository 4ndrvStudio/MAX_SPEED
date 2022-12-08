namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;

    [System.Serializable]
    public class RaceManager_QuickRace_RacerInformation
    {
        public string racerName;
        public GameObject racerObject;
        public int position = 1;
        public int lap = 1;
        public bool finishedRace;
        public int finalStanding;
        public float finishTime;
        public Transform currentWaypoint;
        public Transform previousWaypoint;
        public int nextWP;
        public float checkpointDistance;
        public float positionScore;
        public float distanceScore;
        public float totalScore;
        public Rigidbody rbody;
        public bool isPlayer;
        public GameTemplate_Player gameTemplate_Player;
        public AIRacer gameTemplate_AI;
        public float[] lapTimes;
    }
}