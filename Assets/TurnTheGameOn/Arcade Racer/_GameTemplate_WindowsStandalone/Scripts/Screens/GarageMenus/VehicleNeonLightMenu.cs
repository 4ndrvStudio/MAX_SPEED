namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;
    using TMPro;

    public class VehicleNeonLightMenu : ScreenBase
    {
        public static VehicleNeonLightMenu Instance = null;
        public GameObject confirmWindow;
        public TextMeshProUGUI messageText;
        public GameObject confirmFirstSelection;
        public CanvasGroup mainButtonsCanvasGroup;
        public int price;
        private string selectedColorHex;
        private GameObject selectedColorButton;
        public TextMeshProUGUI priceText;

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
            priceText.text = "Neon Light Color: " + price.ToString("C0");
        }

        public void Button_Back()
        {
            Close();
            VehicleCustomizationMenu.Instance.Open();
            GameTemplate_PlayerCustomization.Instance.UpdateNeonLightState();
        }

        public void SetColorHex(string hex)
        {
            GameTemplate_PlayerCustomization.Instance.SetNewNeonLightColor(hex);
        }

        public void ConfirmDisableNeonLight()
        {
            messageText.text = "Are you sure you want to remove the neon light?";
        }

        public void ShowUnlockConfirm(string hex)
        {
            if (hex != "OFF")
            {
                selectedColorHex = hex;
                selectedColorButton = UISelectionManager.Instance.selected;
                if (hex == "OFF")
                {
                    messageText.text = "Are you sure you want to remove the neon light?";
                }
                else
                {
                    messageText.text = "Purchase neon light for " + price.ToString("C0") + "?";
                }
                confirmWindow.SetActive(true);
                mainButtonsCanvasGroup.interactable = false;
                UISelectionManager.Instance.SetNewSelection(confirmFirstSelection);
            }
        }

        public void AcceptPurchase()
        {
            if (selectedColorHex == "OFF")
            {
                GameData.SetCarNeonLightState(PlayerVehicleManager.Instance.CurrentSelection(), "OFF");
                Close();
                VehicleCustomizationMenu.Instance.Open();
            }
            else
            {
                int playerBank = GameData.GetPlayerBank();
                if (playerBank >= price)
                {
                    int bankBalance = playerBank - price;
                    GameData.SetPlayerBank(bankBalance);
                    PlayerInfoScreen.Instance.playerBankText.text = bankBalance.ToString("C0");
                    confirmWindow.SetActive(false);
                    mainButtonsCanvasGroup.interactable = true;
                    GameData.SetCarNeonLightColorHex(PlayerVehicleManager.Instance.CurrentSelection(), selectedColorHex);
                    GameData.SetCarNeonLightState(PlayerVehicleManager.Instance.CurrentSelection(), "ON");
                    GameTemplate_PlayerCustomization.Instance.UpdateNeonLightColor();
                    Close();
                    VehicleCustomizationMenu.Instance.Open();
                }
            }
        }

        public void DeclinePurchase()
        {
            confirmWindow.SetActive(false);
            mainButtonsCanvasGroup.interactable = true;
            UISelectionManager.Instance.SetNewSelection(selectedColorButton);
        }
    }
}