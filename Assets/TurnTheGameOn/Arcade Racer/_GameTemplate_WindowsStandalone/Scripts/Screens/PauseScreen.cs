namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using System;

    public class PauseScreen : ScreenBase
    {
        public static PauseScreen Instance = null;
        public string pauseButton;
        public bool isPaused { get; private set; }
        public bool canPause;
        private AudioSource[] allAudioSourcesFoundWhenPaused;
        private bool[] wasPaused;
        public GameObject firstSelected;
        public string garageSceneName;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                OnOpenCallback += OnOpenPauseScreen;
                OnCloseCallback += OnClosePauseScreen;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);
        }

        public void OnOpenPauseScreen()
        {
            GameTemplate_Player.Instance.SetPauseState(true);
            Time.timeScale = 0.0f;
            allAudioSourcesFoundWhenPaused = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
            wasPaused = new bool[allAudioSourcesFoundWhenPaused.Length];
            for (int i = 0; i < allAudioSourcesFoundWhenPaused.Length; i++)
            {
                if (allAudioSourcesFoundWhenPaused[i].enabled && allAudioSourcesFoundWhenPaused[i].isPlaying)
                {
                    allAudioSourcesFoundWhenPaused[i].Pause();
                    wasPaused[i] = true;
                }
            }
        }

        public void OnClosePauseScreen()
        {
            Time.timeScale = 1.0f;
            for (int i = 0; i < allAudioSourcesFoundWhenPaused.Length; i++)
            {
                if (allAudioSourcesFoundWhenPaused[i].enabled && wasPaused[i] == true && allAudioSourcesFoundWhenPaused[i].gameObject.activeInHierarchy)
                {
                    allAudioSourcesFoundWhenPaused[i].Play();
                }
            }
            isPaused = false;
            UISelectionManager.Instance.SetNewSelection(null);
            enabled = true;
            GameTemplate_Player.Instance.SetPauseState(false);
        }

        private void Update()
        {
            if (canPause && Input.GetButtonDown(pauseButton))
            {
                if (isPaused)
                {
                    isPaused = false;
                    Close();
                }
                else
                {
                    isPaused = true;
                    Open();
                    //enabled = false;
                }
            }
        }

        public void ReturnToGarage()
        {
            if (!String.IsNullOrEmpty(garageSceneName))
            {
                canPause = false;
                isPaused = false;
                Close();
                if (SeriesManager.Instance != null)
                {
                    GameObject newGO = new GameObject();
                    SeriesManager.Instance.transform.SetParent(newGO.transform); // NO longer DontDestroyOnLoad();
                }
                LoadingScreen.Instance.EnableLoadingScreen();
                SceneManager.LoadScene(garageSceneName);
            }
        }

        public void SetupPauseButton(string _pauseButtonName)
        {
            pauseButton = _pauseButtonName;
        }


    }
}