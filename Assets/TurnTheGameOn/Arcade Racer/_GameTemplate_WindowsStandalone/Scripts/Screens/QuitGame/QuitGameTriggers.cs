namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;

    public class QuitGameTriggers : MonoBehaviour
    {
        public void Button_Quit()
        {
            if (MainMenuScreen.Instance) MainMenuScreen.Instance.canvasGroup.interactable = false;
            if (PauseScreen.Instance) PauseScreen.Instance.canvasGroup.interactable = false;
            gameObject.SetActive(false);
            gameObject.SetActive(true);
            QuitConfirmScreen.Instance.Open();
        }
    }
}