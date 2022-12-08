namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;
    using System.Collections.Generic;

    public class RaceInformation : MonoBehaviour
    {
        public static RaceInformation Instance;
        public List<RacerInformation> racerInformationList;
        public GameObject racerInformationPrefab;
        public GameObject linePrefab;
        public int maxElements = 8;
        public TMPro.TextMeshProUGUI playerStandingText;
        public TMPro.TextMeshProUGUI playerPositionText;
        public TMPro.TextMeshProUGUI playerLapText;
        public TMPro.TextMeshProUGUI lapTimeText;
        public TMPro.TextMeshProUGUI raceTimeText;

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

        [ContextMenu("AddNewElement")]
        public void AddNewElement()
        {
            if (racerInformationList.Count < maxElements)
            {
                //Debug.Log("1");
                if (racerInformationList.Count > 0)
                {
                    GameObject lineElement = Instantiate(linePrefab);
                    lineElement.transform.SetParent(transform);
                    lineElement.transform.localScale = Vector3.one;
                }
                GameObject racerInformationElement = Instantiate(racerInformationPrefab);
                racerInformationElement.transform.SetParent(transform);
                racerInformationElement.transform.localScale = Vector3.one;
                RacerInformation racerInformation = racerInformationElement.GetComponent<RacerInformation>();
                racerInformation.positionText.text = (racerInformationList.Count + 1).ToString();
                racerInformationList.Add(racerInformation);
            }
        }


    }
}