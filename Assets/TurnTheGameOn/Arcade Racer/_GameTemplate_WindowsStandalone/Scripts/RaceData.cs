namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "RaceData", menuName = "TurnTheGameOn/Arcade Racer/RaceData")]
    public class RaceData : ScriptableObject
    {
        public int id;
        public string sceneName;
        public string raceName;
        public string description;
        public Sprite menuImage;
        public int laps = 1;
        public float preRaceTime = 7.0f;
        public RacerInfo[] racerInfo;
        public int[] prize;
        public MiniMapSettings miniMapSettings;
        public bool useDefeatTimer;
        public float defeatTimer = 90;
        public int unlockPrice;
        public bool locked;
        public bool showWaypointArrow = true;
        public bool showWrongWayWarning = true;
        public bool usePlayerVictoryScene;
        public string victorySceneName;
        public string defeatSceneName;
        public bool endSeriesOnDefeat;
    }

    [System.Serializable]
    public class RacerInfo
    {
        public string name;
        public bool player;
        public GameObject vehiclePrefab;
    }

    [System.Serializable]
    public class MiniMapSettings
    {
        public bool enabled;
        public bool lockPositionAndRotation;
        public Vector3 lockPosition;
        public Vector3 lockRotation;
        public float cameraSize;
    }
}