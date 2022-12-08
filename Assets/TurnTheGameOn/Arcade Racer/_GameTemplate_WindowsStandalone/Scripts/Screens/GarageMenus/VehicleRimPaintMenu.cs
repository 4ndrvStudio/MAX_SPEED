namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;
    using TMPro;

    public class VehicleRimPaintMenu : ScreenBase
    {
        public static VehicleRimPaintMenu Instance = null;
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
            priceText.text = "Rim Paint Color: " + price.ToString("C0");
        }

        public void Button_Back()
        {
            Close();
            VehicleCustomizationMenu.Instance.Open();
            GameTemplate_PlayerCustomization.Instance.UpdateRimColor();
        }

        public void SetColorHex(string hex)
        {
            GameTemplate_PlayerCustomization.Instance.SetNewRimColor(hex);
        }

        public void ShowUnlockConfirm(string hex)
        {
            selectedColorHex = hex;
            selectedColorButton = UISelectionManager.Instance.selected;
            messageText.text = "Purchase rim paint job for " + price.ToString("C0") + "?";
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
                GameData.SetCarRimColorHex(PlayerVehicleManager.Instance.CurrentSelection(), selectedColorHex);
                GameTemplate_PlayerCustomization.Instance.UpdateRimColor();
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