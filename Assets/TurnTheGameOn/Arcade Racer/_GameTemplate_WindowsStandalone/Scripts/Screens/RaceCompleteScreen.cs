namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;
    using TMPro;
    using System;
    using UnityEngine.SceneManagement;

    public class RaceCompleteScreen : MonoBehaviour
    {
        public CanvasGroup canvasGroup;
        public static RaceCompleteScreen Instance;
        public TextMeshProUGUI trackNameText;
        public TextMeshProUGUI prizeText;
        public GameObject nextButton;
        public GameObject raceCompleteScreen;
        public CanvasGroup seriesStandingsCanvasGroup;
        public GameObject seriesStandingsNextButton;
        public TextMeshProUGUI seriesPrizeText;
        public TextMeshProUGUI seriesRankText;
        public TextMeshProUGUI seriesNameText;
        public TextMeshProUGUI racesCompletedText;
        public TextMeshProUGUI raceRankText;
        public string menuSceneName;

        void OnEnable()
        {
            if (Instance == null)
            {
                Instance = this;
                canvasGroup.alpha = 0;
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
                seriesStandingsCanvasGroup.alpha = 0;
                seriesStandingsCanvasGroup.interactable = false;
                seriesStandingsCanvasGroup.blocksRaycasts = false;
                
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }

        public void OnRaceComplete()
        {
            trackNameText.text = RaceManager.Instance.raceData.raceName;
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
        }

        public void SetCanContinue()
        {
            if (RaceManager.Instance.isChampionshipRace) prizeText.text = "";
            canvasGroup.interactable = true;
            if (UISelectionManager.Instance) UISelectionManager.Instance.SetNewSelection(nextButton);
        }

        public void RaceComplete_ContinueButton()
        {
            if (RaceManager.Instance.isChampionshipRace)
            {
                ShowSeriesStandingsButton();
            }
            else
            {
                if (RaceManager.Instance.raceData.usePlayerVictoryScene)
                {
                    if (GameTemplate_Player.Instance.wasDefeated)
                    {
                        LoadDefeatScene();
                    }
                    else
                    {
                        LoadVictoryScene();
                    }
                    
                }
                else
                {
                    LoadMenuScene();
                }
            }
        }

        public void LoadMenuScene()
        {
            if (!String.IsNullOrEmpty(menuSceneName))
            {
                if (LoadingScreen.Instance) LoadingScreen.Instance.EnableLoadingScreen();
                SceneManager.LoadScene(menuSceneName);
            }
        }

        public void LoadVictoryScene()
        {
            if (LoadingScreen.Instance) LoadingScreen.Instance.EnableLoadingScreen();
            SceneManager.LoadScene(RaceManager.Instance.raceData.victorySceneName);
        }

        public void LoadDefeatScene()
        {
            if (LoadingScreen.Instance) LoadingScreen.Instance.EnableLoadingScreen();
            SceneManager.LoadScene(RaceManager.Instance.raceData.defeatSceneName);
        }

        public void ShowSeriesStandingsButton()
        {
            ShowSeriesStandings();
        }

        public void ShowSeriesStandings()
        {
            raceCompleteScreen.SetActive(false);
            seriesStandingsCanvasGroup.alpha = 1;
            seriesStandingsCanvasGroup.blocksRaycasts = true;
            seriesStandingsCanvasGroup.interactable = true;
            seriesNameText.text = SeriesManager.Instance.seriesData.seriesName;
            seriesPrizeText.text = "";
            int rank = 0;
            string playerName = GameData.GetProfileName();
            for (int i = 0; i < SeriesManager.Instance.racerList.Count; i++)
            {
                if (playerName == SeriesManager.Instance.racerList[i].name)
                {
                    rank = i + 1;
                    seriesRankText.text = rank.DisplayWithSuffix();
                }
            }
            racesCompletedText.text = "Races Completed: " + (SeriesManager.Instance.raceIndexNumber + 1).ToString() + "/" + SeriesManager.Instance.seriesData.raceInfo.Length;
            if (SeriesManager.Instance.raceIndexNumber + 1 == SeriesManager.Instance.seriesData.raceInfo.Length)
            {
                if (rank - 1 < SeriesManager.Instance.seriesData.prize.Length)
                {
                    seriesPrizeText.text = "Earnings: " + SeriesManager.Instance.seriesData.prize[rank - 1].ToString("c0");
                    int playerBank = GameData.GetPlayerBank();
                    int newBankBalance = playerBank + SeriesManager.Instance.seriesData.prize[rank - 1];
                    GameData.SetPlayerBank(newBankBalance);
                }
                else
                {
                    seriesPrizeText.text = "Earnings: " + (0).ToString("c0");
                }
                int id = SeriesManager.Instance.seriesData.id;
                if (rank < GameData.GetBestSeriesFinish(id))
                {
                    GameData.SetBestSeriesFinish(id, rank);
                }
            }
            for (int i = 0; i < SeriesManager.Instance.racerList.Count; i++)
            {
                RaceInformation_ChampionshipStandings.Instance.AddNewElement(
                               SeriesManager.Instance.racerList[i].name,
                               SeriesManager.Instance.racerList[i].points.ToString()
                               );
            }

            if (UISelectionManager.Instance) UISelectionManager.Instance.SetNewSelection(seriesStandingsNextButton);
        }

        public void SeriesStandings_ContinueButton()
        {
            SeriesManager.Instance.CompletedRaceInSeries();
        }
    }
}