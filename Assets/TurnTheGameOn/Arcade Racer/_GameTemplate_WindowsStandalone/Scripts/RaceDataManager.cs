namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class RaceDataManager : MonoBehaviour
    {
        public static RaceDataManager Instance = null;
        public RaceData raceData;

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoadedCallback;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoadedCallback;
        }

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

        void OnSceneLoadedCallback(Scene scene, LoadSceneMode mode)
        {
            if (raceData != null && RaceManager.Instance != null)
            {
                if (scene.name == raceData.sceneName)
                {
                    RaceManager.Instance.LoadRaceData(raceData);
                }
            }
        }

        public void SetRaceData(RaceData _raceData)
        {
            raceData = _raceData;
        }

    }
}