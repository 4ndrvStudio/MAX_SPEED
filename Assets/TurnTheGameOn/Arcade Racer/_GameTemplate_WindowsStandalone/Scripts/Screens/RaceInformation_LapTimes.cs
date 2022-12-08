namespace TurnTheGameOn.ArcadeRacer
{
    using System.Collections.Generic;
    using UnityEngine;
    using TMPro;

    public class RaceInformation_LapTimes : MonoBehaviour
    {
        public static RaceInformation_LapTimes Instance;
        public List<LapTime> lapTimeList;
        public int maxElements = 8;
        public GameObject lapTimePrefab;
        public GameObject linePrefab;
        public TextMeshProUGUI bestLapTimeText;
        private float previousBestTime;
        private int raceId;

        void OnEnable()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }

        private void OnDisable()
        {
            Instance = null;
        }

        public void Initialize()
        {
            raceId = RaceManager.Instance.raceData.id;
            previousBestTime = GameData.GetBestLapTime(raceId);
            bestLapTimeText.text = FormatGameTime.ConvertFromSeconds((double)previousBestTime);
        }

        [ContextMenu("AddNewElement")]
        public void AddNewElement(string _lap, float _lapTime)
        {
            if (lapTimeList.Count < maxElements)
            {
                if (_lapTime < previousBestTime)
                {
                    previousBestTime = _lapTime;
                    GameData.SetBestLapTime(raceId, _lapTime);
                    bestLapTimeText.text = FormatGameTime.ConvertFromSeconds((double)previousBestTime);
                }
                if (lapTimeList.Count > 0)
                {
                    GameObject lineElement = Instantiate(linePrefab);
                    lineElement.transform.SetParent(transform);
                    lineElement.transform.localScale = Vector3.one;
                }
                GameObject lapTimeListElement = Instantiate(lapTimePrefab);
                lapTimeListElement.transform.SetParent(transform);
                lapTimeListElement.transform.localScale = Vector3.one;
                LapTime lapTime = lapTimeListElement.GetComponent<LapTime>();
                lapTime.lapText.text = _lap;
                lapTime.lapTimeText.text = FormatGameTime.ConvertFromSeconds((double)_lapTime);
                lapTimeList.Add(lapTime);
            }
        }
    }
}