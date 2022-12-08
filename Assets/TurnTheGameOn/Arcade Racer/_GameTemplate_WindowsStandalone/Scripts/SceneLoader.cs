namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using System.Collections;
    using System;

    public class SceneLoader : MonoBehaviour
    {
        public static SceneLoader Instance = null;
        private string startupSceneName;
        public string mainMenuSceneName;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        private void Start()
        {
            Time.timeScale = 1.0f;
            startupSceneName = SceneManager.GetActiveScene().name;
        }

        public static IEnumerator LoadSceneAsync(string sceneName, Action<float> OnProgress, Action OnFailure, Action OnComplete)
        {
            LoadingScreen.Instance.EnableLoadingScreen();
            float startTime;
            startTime = Time.timeSinceLevelLoad;
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

            while (operation.progress < 1f)
            {
                //OnProgress (operation.progress);
                yield return null;
            }
            //SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
            //OnComplete();

            float totalTime = Time.timeSinceLevelLoad - startTime;
            Debug.Log("total time to load " + sceneName + ": " + totalTime);
        }

        public void UnloadScene(string s)
        {
            if (SceneManager.GetSceneByName(s).isLoaded)
            {
                Debug.Log("unloading scene " + s);
                //AsyncOperation sceneUnloading = SceneManager.UnloadSceneAsync (s);
                SceneManager.UnloadSceneAsync(s);
            }
        }

        void OnProgress(float f)
        {
            Debug.Log("Loading scene progress: " + f);
        }

        void OnFailure()
        {
            Debug.LogError("scene not loaded - check to make sure it's in the build settings");
        }

        void OnComplete()
        {
            UnloadScene(startupSceneName);
            Debug.Log("scene loading is complete");
        }

        public void LoadMainMenu()
        {
            if (!String.IsNullOrEmpty(mainMenuSceneName))
            {
                GameData.SetCurrentPlayerVehicleIndex(PlayerVehicleManager.Instance.CurrentSelection());
                SceneManager.LoadScene(mainMenuSceneName);
            }
        }
    }
}