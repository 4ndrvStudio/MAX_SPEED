namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;
    using TMPro;

    public class VehicleWindowTintMenu : ScreenBase
    {
        public static VehicleWindowTintMenu Instance = null;
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
            priceText.text = "Window Tint Color: " + price.ToString("C0");
        }

        public void Button_Back()
        {
            Close();
            VehicleCustomizationMenu.Instance.Open();
            GameTemplate_PlayerCustomization.Instance.UpdateWindowColor();
        }

        public void SetColorHex(string hex)
        {
            GameTemplate_PlayerCustomization.Instance.SetNewRimColor(hex);
        }

        public void ShowUnlockConfirm(string hex)
        {
            selectedColorHex = hex;
            selectedColorButton = UISelectionManager.Instance.selected;
            messageText.text = "Purchase window tints for " + price.ToString("C0") + "?";
            confirmWindow.SetActive(true);
            mainButtonsCanvasGroup.interactable = false;
            UISelectionManager.Instance.SetNewSelection(confirmFirstSelection);
        }

        public void AcceptPurchase()
        {
            int playerBank = GameData.GetPlayerBank();
            if (playerBank >= price)
            {
                int bankBalance = playerBank - price;
                GameData.SetPlayerBank(bankBalance);
                PlayerInfoScreen.Instance.playerBankText.text = bankBalance.ToString("C0");
                confirmWindow.SetActive(false);
                mainButtonsCanvasGroup.interactable = true;
                GameData.SetCarWindowColorHex(PlayerVehicleManager.Instance.CurrentSelection(), selectedColorHex);
                GameTemplate_PlayerCustomization.Instance.UpdateWindowColor();
                Close();
                VehicleCustomizationMenu.Instance.Open();
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