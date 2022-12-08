namespace TurnTheGameOn.ArcadeRacer
{
    using System.Collections.Generic;
    using UnityEngine;

    public class RaceInformation_ChampionshipStandings : MonoBehaviour
    {
        public static RaceInformation_ChampionshipStandings Instance;
        public List<RacerInformation_ChampionshipStandings> racerInformationList;
        public int maxElements = 8;
        public GameObject racerInformationPrefab;
        public GameObject linePrefab;
        private int positionCounter;

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
        public void AddNewElement(string _name, string _points)
        {
            if (racerInformationList.Count < maxElements)
            {
                positionCounter += 1;
                if (racerInformationList.Count > 0)
                {
                    GameObject lineElement = Instantiate(linePrefab);
                    lineElement.transform.SetParent(transform);
                    lineElement.transform.localScale = Vector3.one;
                }
                GameObject racerInformationElement = Instantiate(racerInformationPrefab);
                racerInformationElement.transform.SetParent(transform);
                racerInformationElement.transform.localScale = Vector3.one;
                RacerInformation_ChampionshipStandings racerInformation = racerInformationElement.GetComponent<RacerInformation_ChampionshipStandings>();
                racerInformation.nameText.text = _name;
                racerInformation.positionText.text = positionCounter.DisplayWithSuffix();
                racerInformation.pointsText.text = _points.ToString();
                racerInformationList.Add(racerInformation);
                if (RaceManager.Instance.isChampionshipRace)
                {
                    if (positionCounter == RaceManager.Instance.runtimeRacerInfo.Length)
                    {
                        RaceCompleteScreen.Instance.SetCanContinue();
                    }
                }
            }
        }

    }
}