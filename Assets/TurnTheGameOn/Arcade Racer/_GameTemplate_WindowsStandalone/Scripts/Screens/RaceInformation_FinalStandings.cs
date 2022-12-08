namespace TurnTheGameOn.ArcadeRacer
{
    using System.Collections.Generic;
    using UnityEngine;

    public class RaceInformation_FinalStandings : MonoBehaviour
    {
        public static RaceInformation_FinalStandings Instance;
        public List<RacerInformation_FinalStandings> racerInformationList;
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
        public void AddNewElement(string _name, string _raceTime)
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
                RacerInformation_FinalStandings racerInformation = racerInformationElement.GetComponent<RacerInformation_FinalStandings>();
                racerInformation.nameText.text = _name;

                if (_name == GameData.GetProfileName()) RaceCompleteScreen.Instance.raceRankText.text = positionCounter.DisplayWithSuffix();

                racerInformation.positionText.text = positionCounter.DisplayWithSuffix();
                racerInformation.raceTimeText.text = _raceTime;


                if (RaceManager.Instance.isChampionshipRace)
                {
                    int finishPoints = 0;
                    for (int i = 0; i < SeriesManager.Instance.seriesData.finishPoints.Length; i++)
                    {
                        if (i == positionCounter - 1)
                        {
                            finishPoints = SeriesManager.Instance.seriesData.finishPoints[i];
                        }
                    }

                    string finishPointsString = finishPoints == 0 ? "" : "+" + finishPoints.ToString();
                    racerInformation.pointsText.text = finishPointsString;
                    racerInformationList.Add(racerInformation);
                    SeriesManager.Instance.AddRacerPoints(_name, finishPoints);
                    if (positionCounter == RaceManager.Instance.runtimeRacerInfo.Length)
                    {
                        RaceCompleteScreen.Instance.SetCanContinue();
                    }
                }
                else
                {
                    racerInformation.pointsText.text = "";
                }
                
            }
        }

    }
}