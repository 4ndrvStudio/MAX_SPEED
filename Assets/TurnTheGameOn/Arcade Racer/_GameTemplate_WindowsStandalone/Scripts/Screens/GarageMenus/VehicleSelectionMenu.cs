namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections.Generic;
    using TMPro;

    public class VehicleSelectionMenu : ScreenBase
    {
        public static VehicleSelectionMenu Instance = null;
        private GameObject currentVehicle;
        public Transform spawnPoint;
        public Transform spawnParent;
        public Button buyButton;
        public Button selectButton;
        public Button customizeButton;
        private List<bool> ownedCars;
        public CanvasGroup mainButtonsCanvasGroup;
        public TextMeshProUGUI priceText;
        public GameObject priceTextObject;
        public TextMeshProUGUI nameText;
        public GameObject nameTextObject;
        public TextMeshProUGUI currentSelectionName;
        public TextMeshProUGUI currentSelectionSpeed;
        public RectTransform selectionInfoBoxRect;
        public float minInfoBoxSize;

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
            Open();
            if (PlayerVehicleManager.Instance == null)
            {
                Debug.LogWarning("This scene requires a PlayerVehicleManager, add one to this scene for testing or load this scene from the main menu.");
            }
            else
            {
                PlayerVehicleManager.Instance.SelectVehicleIndex(GameData.GetCurrentPlayerVehicleIndex());
                PlayerInfoScreen.Instance.SetPlayerCar(PlayerVehicleManager.Instance.VehicleName());
                PlayerInfoScreen.Instance.SetVehicleTopSpeed(PlayerVehicleManager.Instance.VehicleStockTopSpeed().ToString() + " MPH");
                CheckOwnedList();
                SpawnCurrentVehicle();
            }
        }

        public void SelectVehicleIndex(int i)
        {
            PlayerVehicleManager.Instance.SelectVehicleIndex(i);
            Destroy(currentVehicle);
            SpawnCurrentVehicle();
        }

        void SpawnCurrentVehicle()
        {
            currentVehicle = Instantiate (PlayerVehicleManager.Instance.VehiclePrefab(), spawnPoint.position, spawnPoint.rotation);
            nameTextObject.SetActive(false);
            nameText.text = PlayerVehicleManager.Instance.VehicleName();
            nameTextObject.SetActive(true);

            selectionInfoBoxRect.gameObject.SetActive(false);
            currentSelectionName.text = PlayerVehicleManager.Instance.VehicleName();
            currentSelectionSpeed.text = PlayerVehicleManager.Instance.VehicleStockTopSpeed().ToString() + " MPH";
            Vector2 newSize = selectionInfoBoxRect.sizeDelta;
            newSize.x = (33 * PlayerVehicleManager.Instance.VehicleName().Length) + 40;
            if (newSize.x < minInfoBoxSize) newSize.x = minInfoBoxSize;
            selectionInfoBoxRect.sizeDelta = newSize;
            selectionInfoBoxRect.gameObject.SetActive(true);

            priceTextObject.SetActive(!ownedCars[PlayerVehicleManager.Instance.CurrentSelection()]);
            string vehiclePrice = ownedCars[PlayerVehicleManager.Instance.CurrentSelection()] ? "-" : PlayerVehicleManager.Instance.VehiclePrice().ToString("C0");
            priceText.text = vehiclePrice;
            
            buyButton.interactable = ownedCars[PlayerVehicleManager.Instance.CurrentSelection()] == true ? false : true;
            selectButton.interactable = ownedCars[PlayerVehicleManager.Instance.CurrentSelection()] == true ? true : false;
            customizeButton.interactable = ownedCars[PlayerVehicleManager.Instance.CurrentSelection()] == true ? true : false;
            currentVehicle.transform.parent = spawnParent;
        }

        public void CheckOwnedList()
        {
            ownedCars = new List<bool>();
            for (int i = 0; i < PlayerVehicleManager.Instance.playerVehicles.playerVehicles.Length; i++)
            {
                if (PlayerVehicleManager.Instance.playerVehicles.playerVehicles[i].owned == false)
                {
                    ownedCars.Add(GameData.GetCarOwned(i));
                }
                else
                {
                    ownedCars.Add(true);
                }
            }
        }

        public void AcceptPurchaseCar()
        {
            if (ownedCars[PlayerVehicleManager.Instance.CurrentSelection()] == false)
            {
                int playerBank = GameData.GetPlayerBank();
                int vehiclePrice = PlayerVehicleManager.Instance.VehiclePrice();
                if (playerBank >= vehiclePrice)
                {
                    int bankBalance = playerBank - vehiclePrice;
                    GameData.SetPlayerBank(bankBalance);
                    ownedCars[PlayerVehicleManager.Instance.CurrentSelection()] = true;
                    GameData.SetCarOwned(PlayerVehicleManager.Instance.CurrentSelection(), 1);
                    buyButton.interactable = false;
                    selectButton.interactable = true;
                    customizeButton.interactable = true;
                    UISelectionManager.Instance.SetNewSelection(selectButton.gameObject);
                    PlayerInfoScreen.Instance.playerBankText.text = bankBalance.ToString("C0");
                    priceTextObject.SetActive(false);
                }
            }
        }

        public void Button_Select()
        {
            SceneLoader.Instance.LoadMainMenu();
        }

        public void Button_Customize()
        {
            PlayerInfoScreen.Instance.SetPlayerCar(PlayerVehicleManager.Instance.VehicleName());
            PlayerInfoScreen.Instance.SetVehicleTopSpeed(PlayerVehicleManager.Instance.VehicleStockTopSpeed().ToString() + " MPH");
            Close();
            VehicleCustomizationMenu.Instance.Open();
        }

        public void Button_Next()
        {
            PlayerVehicleManager.Instance.SelectNextVehicle();
            Destroy(currentVehicle);
            SpawnCurrentVehicle();
        }

        public void Button_Previous()
        {
            PlayerVehicleManager.Instance.SelectPreviousVehicle();
            Destroy(currentVehicle);
            SpawnCurrentVehicle();
        }

        public void Button_Buy()
        {
            int playerBank = GameData.GetPlayerBank();
            int vehiclePrice = PlayerVehicleManager.Instance.VehiclePrice();
            if (playerBank >= vehiclePrice)
            {
                mainButtonsCanvasGroup.interactable = false;
                ConfirmScreenBuyCar.Instance.ShowUnlockConfirm(PlayerVehicleManager.Instance.VehicleName(), PlayerVehicleManager.Instance.VehiclePrice().ToString("c0"));
            }
        }

    }
}