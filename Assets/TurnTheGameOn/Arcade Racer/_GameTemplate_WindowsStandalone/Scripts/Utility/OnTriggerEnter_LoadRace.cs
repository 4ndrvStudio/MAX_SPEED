namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine.SceneManagement;
    using UnityEngine;
    using System;
    using System.Collections;

    public class OnTriggerEnter_LoadRace : MonoBehaviour
    {
        public string triggerTag = "Player";
        public RaceData raceData;
        private string currentScene;

        public void OnTriggerEnter(Collider other)
        {
            if (other.tag == triggerTag)
            {
                RaceDataManager.Instance.raceData = raceData;
                DontDestroyOnLoad(RaceDataManager.Instance.gameObject);
                LoadScene(raceData.sceneName);
            }
        }

        public void LoadScene(string _sceneName)
        {
            currentScene = SceneManager.GetActiveScene().name;
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
            UnloadScene(currentScene);
            Debug.Log("scene loading is complete");
        }


    }
}