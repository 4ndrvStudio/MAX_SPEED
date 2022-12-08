namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;
    using TMPro;

    public class UnlockRaceConfirmScreen : MonoBehaviour
    {
        public GameObject unlockConfirmWindow;
        public TextMeshProUGUI messageText;
        public static UnlockRaceConfirmScreen Instance = null;
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

        public void ShowUnlockConfirm(string _raceName, string _price)
        {
            messageText.text = "Are you sure you want to unlock " + _raceName + " for " + _price + "?";
            unlockConfirmWindow.SetActive(true);
        }

        public void AcceptPurchase()
        {
            unlockConfirmWindow.SetActive(false);
            RaceSelectionMenu.Instance.mainButtonsCanvasGroup.interactable = true;
            RaceSelectionMenu.Instance.AcceptUnlockRace();
        }

        public void DeclinePurchase()
        {
            unlockConfirmWindow.SetActive(false);
            RaceSelectionMenu.Instance.mainButtonsCanvasGroup.interactable = true;
            UISelectionManager.Instance.SetNewSelection(RaceSelectionMenu.Instance.unlockButton.gameObject);
        }

    }
}