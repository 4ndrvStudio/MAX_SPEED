namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;
    using TMPro;

    public class ConfirmScreenBuyCar : MonoBehaviour
    {
        public GameObject unlockConfirmWindow;
        public TextMeshProUGUI messageText;
        public static ConfirmScreenBuyCar Instance = null;
        public GameObject firstSelection;

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

        public void ShowUnlockConfirm(string _name, string _price)
        {
            messageText.text = "Purchase " + _name + " for " + _price + "?";
            unlockConfirmWindow.SetActive(true);
            UISelectionManager.Instance.SetNewSelection(firstSelection);
        }

        public void AcceptPurchase()
        {
            unlockConfirmWindow.SetActive(false);
            VehicleSelectionMenu.Instance.mainButtonsCanvasGroup.interactable = true;
            VehicleSelectionMenu.Instance.AcceptPurchaseCar();
        }

        public void DeclinePurchase()
        {
            unlockConfirmWindow.SetActive(false);
            VehicleSelectionMenu.Instance.mainButtonsCanvasGroup.interactable = true;
            UISelectionManager.Instance.SetNewSelection(VehicleSelectionMenu.Instance.buyButton.gameObject);
        }

    }
}