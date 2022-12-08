namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class RaceDefeatMenu : MonoBehaviour
    {
        public static RaceDefeatMenu Instance = null;
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
            SceneManager.LoadScene(mainMenuSceneName);
        }

    }
}