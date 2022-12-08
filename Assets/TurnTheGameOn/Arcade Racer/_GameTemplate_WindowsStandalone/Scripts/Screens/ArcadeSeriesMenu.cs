namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;
    using System.Collections.Generic;

    public class ArcadeSeriesMenu : ScreenBase
    {
        public static ArcadeSeriesMenu Instance = null;
        public RaceSeriesData[] seriesData;
        public TextMeshProUGUI seriesNameText;
        public TextMeshProUGUI descriptionText;
        public TextMeshProUGUI prizeText;
        public TextMeshProUGUI unlockPriceText;
        public TextMeshProUGUI bestFinishText;
        public TextMeshProUGUI racesText;
        public GameObject lockedOverlay;
        public Button previousButton;
        public Button nextButton;
        public Button selectButton;
        public Button unlockButton;
        public Button mainMenuButton;
        public Image seriesImage;
        public GameObject[] prizeElements;
        public TextMeshProUGUI[] prizeElementText;
        public bool canPayToUnlockSeries;
        private int selectedSeries;
        public CanvasGroup mainButtonsCanvasGroup;
        public UnlockSeriesConfirmScreen unlockWindow;
        public GameObject descriptionBox;
        public GameObject imageBox;
        public GameObject infoBox;

        void Awake()
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

        private void Start()
        {
            CheckUnlockedList();
        }

        void OnEnable()
        {
            OnOpenCallback += OnOpenSeriesSelectionScreen;
            OnCloseCallback += OnCloseSeriesSelectionScreen;
        }

        void OnDisable()
        {
            OnOpenCallback -= OnOpenSeriesSelectionScreen;
            OnCloseCallback -= OnCloseSeriesSelectionScreen;
        }

        public void OnOpenSeriesSelectionScreen()
        {
            if (MainMenuScreen.Instance)
            {
                MainMenuScreen.Instance.Close();
            }
            SetSeriesSelection(0);
        }

        public void OnCloseSeriesSelectionScreen()
        {
            if (MainMenuScreen.Instance)
            {
                MainMenuScreen.Instance.Open();
            }
        }

        void SetSeriesSelection(int i)
        {
            descriptionBox.SetActive(false);
            imageBox.SetActive(false);
            infoBox.SetActive(false);
            selectedSeries = i;
            seriesNameText.text = seriesData[i].seriesName;
            racesText.text = seriesData.Length.ToString();
            descriptionText.text = seriesData[i].description;
            seriesImage.sprite = seriesData[i].menuImage;
            CheckPrizes(i);
            CheckBestSeriesFinish(seriesData[i].id);
            CheckRaceLock(i);
            CheckCurrentSelections(i);
            descriptionBox.SetActive(true);
            imageBox.SetActive(true);
            infoBox.SetActive(true);
        }

        void CheckPrizes(int i)
        {
            for (int j = 0; j < prizeElements.Length; j++)
            {
                prizeElements[j].SetActive(false);
            }
            for (int j = 0; j < seriesData[i].prize.Length; j++)
            {
                if (j < prizeElements.Length)
                {
                    prizeElements[j].SetActive(true);
                    prizeElementText[j].text = seriesData[i].prize[j].ToString("c0");
                }   
            }
        }

        void CheckCurrentSelections(int i)
        {
            if (i == 0)
            {
                if (UISelectionManager.Instance.selected == previousButton.gameObject)
                {
                    UISelectionManager.Instance.SetNewSelection(nextButton.gameObject);
                }
                previousButton.interactable = false;
            }
            else
            {
                previousButton.interactable = true;
            }

            if (i == seriesData.Length - 1)
            {
                if (UISelectionManager.Instance.selected == nextButton.gameObject)
                {
                    UISelectionManager.Instance.SetNewSelection(previousButton.gameObject);
                }
                nextButton.interactable = false;
            }
            else
            {
                nextButton.interactable = true;
            }
        }

        void CheckBestSeriesFinish(int i)
        {
            if (GameData.GetBestSeriesFinish(i) == 99999)
            {
                bestFinishText.text = "-";
            }
            else
            {
                bestFinishText.text = GameData.GetBestSeriesFinish(i).ToString();
            }
        }

        void CheckRaceLock(int i)
        {
            bool seriesIsUnlocked = GameData.GetRaceUnlocked(i);
            if (seriesData[i].locked && seriesIsUnlocked == false)
            {
                lockedOverlay.SetActive(true);
                selectButton.interactable = false;
                if (canPayToUnlockSeries)
                {
                    unlockPriceText.text = seriesData[i].unlockPrice.ToString("c0");
                    unlockButton.interactable = seriesData[i].locked;
                    unlockButton.gameObject.SetActive(true);
                }
            }
            else
            {
                lockedOverlay.SetActive(false);
                selectButton.interactable = true;
                unlockButton.gameObject.SetActive(false);
            }
        }

        public void SelectPreviousSeries()
        {
            if (selectedSeries > 0)
            {
                SetSeriesSelection(selectedSeries - 1);
            }
        }

        public void SelectNextSeries()
        {
            if (selectedSeries < seriesData.Length - 1)
            {
                SetSeriesSelection(selectedSeries + 1);
            }
        }

        public void SelectSeries()
        {
            SeriesManager.Instance.StartSeries(seriesData[selectedSeries], SeriesType.Arcade);
        }

        public void RequestUnlockSeries()
        {
            int playerBank = GameData.GetPlayerBank();
            int unlockPrice = seriesData[selectedSeries].unlockPrice;
            if (unlockedSeries[selectedSeries] == false && playerBank >= unlockPrice)
            {
                mainButtonsCanvasGroup.interactable = false;
                unlockWindow.ShowUnlockConfirm(seriesData[selectedSeries].seriesName, seriesData[selectedSeries].unlockPrice.ToString("c0"));
                UISelectionManager.Instance.SetNewSelection(unlockWindow.firstSelection);
            }
        }

        public void AcceptUnlockSeries()
        {
            if (unlockedSeries[selectedSeries] == false)
            {
                int playerBank = GameData.GetPlayerBank();
                int unlockPrice = seriesData[selectedSeries].unlockPrice;
                if (playerBank >= unlockPrice)
                {
                    int bankBalance = playerBank - unlockPrice;
                    GameData.SetPlayerBank(bankBalance);
                    unlockedSeries[selectedSeries] = true;
                    GameData.SetRaceUnlocked(selectedSeries, 1);
                    unlockButton.interactable = false;
                    selectButton.interactable = true;
                    UISelectionManager.Instance.SetNewSelection(selectButton.gameObject);
                    lockedOverlay.SetActive(false);
                    PlayerInfoScreen_MainMenu.Instance.RefreshData();
                }
            }
        }

        private List<bool> unlockedSeries;

        public void CheckUnlockedList()
        {
            if (canPayToUnlockSeries == false)
            {
                unlockButton.gameObject.SetActive(false);
                lockedOverlay.SetActive(false);
                unlockPriceText.text = "";
            }
            unlockedSeries = new List<bool>();
            for (int i = 0; i < seriesData.Length; i++)
            {
                if (seriesData[i].locked == true)
                {
                    unlockedSeries.Add(GameData.GetRaceUnlocked(i));
                }
                else
                {
                    unlockedSeries.Add(true);
                }
            }
        }



    }
}