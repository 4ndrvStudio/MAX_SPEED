namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class RaceVictoryMenu : MonoBehaviour
    {
        public static RaceVictoryMenu Instance = null;
        public string mainMenuSceneName;
        public GameObject firstButtonSelection;

        private void Awake()
        {
            if (LoadingScreen.Instance) LoadingScreen.Instance.DisableLoadingScreen(0.5f);
            UISelectionManager.Instance.SetNewSelection(firstButtonSelection);
            Instance = this;
        }

        public void NextButton()
        {
            if (SeriesManager.Instance != null)
            {
                SeriesManager.Instance.LoadNextRaceInSeries();
            }
            else
            {
                SceneManager.LoadScene(mainMenuSceneName);
            }
        }

    }
}