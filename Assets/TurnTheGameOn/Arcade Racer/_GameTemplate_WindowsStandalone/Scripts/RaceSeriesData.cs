namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "RaceSeriesData", menuName = "TurnTheGameOn/Arcade Racer/RaceSeriesData")]
    public class RaceSeriesData : ScriptableObject
    {
        public int id;
        public string seriesName;
        public string description;
        public Sprite menuImage;
        public RaceData[] raceInfo;
        public RacerInfo[] racerInfo;
        public int[] finishPoints;
        public int[] prize;
        public int unlockPrice;
        public bool locked;
    }

}