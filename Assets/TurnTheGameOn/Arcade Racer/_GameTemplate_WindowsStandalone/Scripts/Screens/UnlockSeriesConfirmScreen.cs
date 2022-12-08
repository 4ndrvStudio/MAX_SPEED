namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;
    using TMPro;

    public class UnlockSeriesConfirmScreen : MonoBehaviour
    {
        public GameObject unlockConfirmWindow;
        public TextMeshProUGUI messageText;
        public GameObject firstSelection;
        
        public SeriesType seriesType;

        public void ShowUnlockConfirm(string _seriesName, string _price)
        {
            messageText.text = "Are you sure you want to unlock " + _seriesName + " for " + _price + "?";
            unlockConfirmWindow.SetActive(true);
        }

        public void AcceptPurchase()
        {
            unlockConfirmWindow.SetActive(false);
            if (seriesType == SeriesType.Championship)
            {
                ChampionshipSeriesMenu.Instance.mainButtonsCanvasGroup.interactable = true;
                ChampionshipSeriesMenu.Instance.AcceptUnlockSeries();
            }
            else if (seriesType == SeriesType.Arcade)
            {
                ArcadeSeriesMenu.Instance.mainButtonsCanvasGroup.interactable = true;
                ArcadeSeriesMenu.Instance.AcceptUnlockSeries();
            }
        }

        public void DeclinePurchase()
        {
            unlockConfirmWindow.SetActive(false);
            if (seriesType == SeriesType.Championship)
            {
                ChampionshipSeriesMenu.Instance.mainButtonsCanvasGroup.interactable = true;
                UISelectionManager.Instance.SetNewSelection(ChampionshipSeriesMenu.Instance.unlockButton.gameObject);
            }
            else if (seriesType == SeriesType.Arcade)
            {
                ArcadeSeriesMenu.Instance.mainButtonsCanvasGroup.interactable = true;
                UISelectionManager.Instance.SetNewSelection(ArcadeSeriesMenu.Instance.unlockButton.gameObject);
            }
        }

    }
}