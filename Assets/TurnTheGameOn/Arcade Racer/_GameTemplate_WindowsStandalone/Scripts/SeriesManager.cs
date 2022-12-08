namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;
    using System.Collections.Generic;
    using System.Collections;
    using UnityEngine.SceneManagement;
    using System;

    public class SeriesManager : MonoBehaviour
    {
        public static SeriesManager Instance;
        public RaceSeriesData seriesData;
        public List<SeriesRacer> racerList;
        public int raceIndexNumber;
        private SeriesType seriesType;
        private bool seriesComplete;

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

        public void AddRacerToList(string name)
        {
            SeriesRacer newRacer = new SeriesRacer();
            newRacer.name = name;
            racerList.Add(newRacer);
        }

        public void AddRacerPoints(string racerName, int pointsToAdd)
        {
            for (int i = 0; i < racerList.Count; i++)
            {
                if (racerName == racerList[i].name)
                {
                    racerList[i].points += pointsToAdd;
                    break;
                }
            }
            racerList.Sort();
            racerList.Reverse();
        }

        public void StartSeries(RaceSeriesData data, SeriesType seriesType)
        {
            DontDestroyOnLoad(gameObject);
            seriesData = data;
            raceIndexNumber = 0;
            for (int i = 0; i < seriesData.racerInfo.Length; i++)
            {
                if (seriesData.racerInfo[i].player)
                {
                    AddRacerToList(GameData.GetProfileName());
                }
                else
                {
                    AddRacerToList(seriesData.racerInfo[i].name);
                }
            }
            RaceDataManager.Instance.SetRaceData(seriesData.raceInfo[0]);
            DontDestroyOnLoad(RaceDataManager.Instance.gameObject);
            MainMenuScreen.Instance.LoadScene(seriesData.raceInfo[0].sceneName);
        }

        public void CompletedRaceInSeries()
        {
            if (raceIndexNumber < seriesData.raceInfo.Length - 1)
            {
                if (LoadingScreen.Instance != null) LoadingScreen.Instance.EnableLoadingScreen();
                raceIndexNumber += 1;
                RaceDataManager.Instance.SetRaceData(seriesData.raceInfo[raceIndexNumber]);
            }
            else
            {
                seriesComplete = true;

            }
            if (seriesData.raceInfo[raceIndexNumber].usePlayerVictoryScene) // go to victory scene
            {
                LoadVictoryScene();
            }
            else
            {
                LoadNextRaceInSeries();
            }
        }

        public void LoadMainMenu()
        {
            GameObject o = new GameObject();
            this.transform.SetParent(o.transform); // Remove DontDestroyOnLoad();
            RaceDataManager.Instance.transform.SetParent(o.transform); // Remove DontDestroyOnLoad();
            RaceCompleteScreen.Instance.LoadMenuScene(); // Load menu scene
        }

        public void LoadNextRaceInSeries()
        {  
            if (seriesComplete)
            {
                LoadMainMenu();
            }
            else
            {
                LoadScene(seriesData.raceInfo[raceIndexNumber].sceneName);
            }
        }

        public void LoadVictoryScene()
        {
            LoadScene(seriesData.raceInfo[raceIndexNumber].victorySceneName);
        }

        public void LoadScene(string _sceneName)
        {
            StartCoroutine(LoadSceneAsync(_sceneName, OnProgress, OnFailure, OnComplete));
        }

        public void LoadDefeatScene()
        {
            SceneManager.LoadScene(seriesData.raceInfo[raceIndexNumber].defeatSceneName);
        }

        public static IEnumerator LoadSceneAsync(string sceneName, Action<float> OnProgress, Action OnFailure, Action OnComplete)
        {
            LoadingScreen.Instance.EnableLoadingScreen();
            //float startTime; //startTime = Time.timeSinceLevelLoad;
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            while (operation.progress < 1f)
            {
                yield return null; //OnProgress (operation.progress);
            }
            //SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));  //OnComplete(); //float totalTime = Time.timeSinceLevelLoad - startTime; //Debug.Log("total time to load " + sceneName + ": " + totalTime);
        }

        public void UnloadScene(string s)
        {
            if (SceneManager.GetSceneByName(s).isLoaded)
            {
                //Debug.Log("unloading scene " + s); //AsyncOperation sceneUnloading = SceneManager.UnloadSceneAsync (s);
                SceneManager.UnloadSceneAsync(s);
            }
        }

        void OnProgress(float f)
        {
            //Debug.Log("Loading scene progress: " + f);
        }

        void OnFailure()
        {
            //Debug.LogError("scene not loaded - check to make sure it's in the build settings");
        }

        void OnComplete()
        {
            //UnloadScene(startupSceneName);
            //Debug.Log("scene loading is complete");
        }

    }

    [System.Serializable]
    public class SeriesRacer : IComparable<SeriesRacer>
    {
        public int points;
        public string name;

        public int CompareTo(SeriesRacer item)
        {       // A null value means that this object is greater.
            if (item == null)
            {
                return 1;
            }
            else
            {
                return this.points.CompareTo(item.points);
            }
        }
    }


}