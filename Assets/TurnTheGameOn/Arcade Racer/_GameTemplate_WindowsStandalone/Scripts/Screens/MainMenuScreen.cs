namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine.SceneManagement;
    using UnityEngine;
    using System;
    using System.Collections;
    using TMPro;

    public class MainMenuScreen : ScreenBase
    {
        public static MainMenuScreen Instance = null;
        private string startupSceneName;
        public string openWorldSceneName;
        public string garageSceneName;
        public TextMeshProUGUI descriptionText;
        public MenuDescription[] menuDescriptions;
        [System.Serializable]
        public class MenuDescription
        {
            public string buttonName;
            [TextArea(2, 8)]
            public string menuDescription;
            public float boxSizeY;
        }
        public RectTransform descriptionBox;

        public void SetMenuDescription(string selectedMenuButtonName)
        {
            for (int i = 0; i < menuDescriptions.Length; i ++)
            {
                if (menuDescriptions[i].buttonName == selectedMenuButtonName)
                {
                    descriptionBox.gameObject.SetActive(false);
                    descriptionText.text = menuDescriptions[i].menuDescription;
                    Vector2 newSize = descriptionBox.sizeDelta;
                    newSize.y = menuDescriptions[i].boxSizeY;
                    descriptionBox.sizeDelta = newSize;
                    descriptionBox.gameObject.SetActive(true);
                }
            }
        }

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
            startupSceneName = SceneManager.GetActiveScene().name;
            if (LoadingScreen.Instance != null) LoadingScreen.Instance.DisableLoadingScreen(0.1f);
            if (PauseScreen.Instance != null) PauseScreen.Instance.canPause = false;
        }

        public void Button_OpenWorld()
        {
            if (!String.IsNullOrEmpty(openWorldSceneName))
            {
                screenObject.SetActive(false);
                StartCoroutine(LoadSceneAsync(openWorldSceneName, OnProgress, OnFailure, OnComplete));
            }
        }

        public void Button_Garage()
        {
            if (!String.IsNullOrEmpty(garageSceneName))
            {
                screenObject.SetActive(false);
                LoadScene(garageSceneName);
            }
        }

        public void Button_OpenUserProfileScreen()
        {
            Close();
            PlayerInfoScreen_MainMenu.Instance.Close();
            UserProfileScreen.Instance.Open();
        }

        public void LoadScene(string _sceneName)
        {
            StartCoroutine(LoadSceneAsync(_sceneName, OnProgress, OnFailure, OnComplete));
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

        public void SetProfileData(string _profileName)
        {
            GameData.SetProfileName(_profileName);
            PlayerInfoScreen_MainMenu.Instance.Open();
        }

    }
}