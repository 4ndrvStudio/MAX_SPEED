namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;
    using System.Collections.Generic;

    public class RaceSelectionMenu : ScreenBase
    {
        public static RaceSelectionMenu Instance = null;
        public RaceData[] raceData;
        public TextMeshProUGUI raceNameText;
        public TextMeshProUGUI descriptionText;
        public TextMeshProUGUI prizeText;
        public TextMeshProUGUI unlockPriceText;
        public TextMeshProUGUI lapsText;
        public TextMeshProUGUI bestLapTimeText;
        public TextMeshProUGUI racersText;
        public GameObject lockedOverlay;
        public Button previousButton;
        public Button nextButton;
        public Button selectButton;
        public Button unlockButton;
        public Button mainMenuButton;
        public Image raceImage;
        public GameObject[] prizeElements;
        public TextMeshProUGUI[] prizeElementText;
        public bool canPayToUnlockRace;
        private int selectedRace;
        public CanvasGroup mainButtonsCanvasGroup;

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
            OnOpenCallback += OnOpenRaceSelectionScreen;
            OnCloseCallback += OnCloseRaceSelectionScreen;
        }

        void OnDisable()
        {
            OnOpenCallback -= OnOpenRaceSelectionScreen;
            OnCloseCallback -= OnCloseRaceSelectionScreen;
        }

        public void OnOpenRaceSelectionScreen()
        {
            if (MainMenuScreen.Instance)
            {
                MainMenuScreen.Instance.Close();
            }
            SetRaceSelection(0);
        }

        public void OnCloseRaceSelectionScreen()
        {
            if (MainMenuScreen.Instance)
            {
                MainMenuScreen.Instance.Open();
            }
        }

        void SetRaceSelection(int i)
        {
            selectedRace = i;
            raceNameText.text = raceData[i].raceName;
            descriptionText.text = raceData[i].description;
            raceImage.sprite = raceData[i].menuImage;
            lapsText.text = raceData[i].laps.ToString();
            racersText.text = raceData[i].racerInfo.Length.ToString();
            CheckPrizes(i);
            CheckBestLapTime(raceData[i].id);
            CheckRaceLock(i);
            CheckCurrentSelections(i);
        }

        void CheckPrizes(int i)
        {
            for (int j = 0; j < prizeElements.Length; j++)
            {
                prizeElements[j].SetActive(false);
            }
            for (int j = 0; j < raceData[i].prize.Length; j++)
            {
                if (j < prizeElements.Length)
                {
                    prizeElements[j].SetActive(true);
                    prizeElementText[j].text = raceData[i].prize[j].ToString("c0");
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

            if (i == raceData.Length - 1)
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

        void CheckBestLapTime(int i)
        {
            if (GameData.GetBestLapTime(i) == 99999)
            {
                bestLapTimeText.text = "--:--.---";
            }
            else
            {
                bestLapTimeText.text = FormatGameTime.ConvertFromSeconds((double)GameData.GetBestLapTime(i));
            }
        }

        void CheckRaceLock(int i)
        {
            bool raceIsUnlocked = GameData.GetRaceUnlocked(i);
            if (raceData[i].locked && raceIsUnlocked == false)
            {
                lockedOverlay.SetActive(true);
                selectButton.interactable = false;
                if (canPayToUnlockRace)
                {
                    unlockPriceText.text = raceData[i].unlockPrice.ToString("c0");
                    unlockButton.interactable = raceData[i].locked;
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

        public void SelectPreviousRace()
        {
            if (selectedRace > 0)
            {
                SetRaceSelection(selectedRace - 1);
            }
        }

        public void SelectNextRace()
        {
            if (selectedRace < raceData.Length - 1)
            {
                SetRaceSelection(selectedRace + 1);
            }
        }

        public void SelectRace()
        {
            RaceDataManager.Instance.SetRaceData(raceData[selectedRace]);
            DontDestroyOnLoad(RaceDataManager.Instance.gameObject);
            MainMenuScreen.Instance.LoadScene(raceData[selectedRace].sceneName);
        }

        public void RequestUnlockRace()
        {
            int playerBank = GameData.GetPlayerBank();
            int unlockPrice = raceData[selectedRace].unlockPrice;
            if (unlockedRaces[selectedRace] == false && playerBank >= unlockPrice)
            {
                mainButtonsCanvasGroup.interactable = false;
                UnlockRaceConfirmScreen.Instance.ShowUnlockConfirm(raceData[selectedRace].raceName, raceData[selectedRace].unlockPrice.ToString("c0"));
                UISelectionManager.Instance.SetNewSelection(UnlockRaceConfirmScreen.Instance.firstSelection);
            }
        }

        public void AcceptUnlockRace()
        {
            if (unlockedRaces[selectedRace] == false)
            {
                int playerBank = GameData.GetPlayerBank();
                int unlockPrice = raceData[selectedRace].unlockPrice;
                if (playerBank >= unlockPrice)
                {
                    int bankBalance = playerBank - unlockPrice;
                    GameData.SetPlayerBank(bankBalance);
                    unlockedRaces[selectedRace] = true;
                    GameData.SetRaceUnlocked(selectedRace, 1);
                    unlockButton.interactable = false;
                    selectButton.interactable = true;
                    UISelectionManager.Instance.SetNewSelection(selectButton.gameObject);
                    lockedOverlay.SetActive(false);
                    PlayerInfoScreen_MainMenu.Instance.RefreshData();
                }
            }
        }

        private List<bool> unlockedRaces;

        public void CheckUnlockedList()
        {
            if (canPayToUnlockRace == false)
            {
                unlockButton.gameObject.SetActive(false);
                lockedOverlay.SetActive(false);
                unlockPriceText.text = "";
            }
            unlockedRaces = new List<bool>();
            for (int i = 0; i < raceData.Length; i++)
            {
                if (raceData[i].locked == true)
                {
                    unlockedRaces.Add(GameData.GetRaceUnlocked(i));
                }
                else
                {
                    unlockedRaces.Add(true);
                }
            }
        }



    }
}