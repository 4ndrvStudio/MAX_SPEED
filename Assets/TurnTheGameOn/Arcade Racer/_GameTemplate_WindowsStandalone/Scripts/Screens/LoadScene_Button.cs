namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using System;

    public class LoadScene_Button : MonoBehaviour
    {
        public string sceneName;
        public void LoadScene()
        {
            if (!String.IsNullOrEmpty(sceneName))
            {
                if (SeriesManager.Instance != null)
                {
                    GameObject newGO = new GameObject();
                    SeriesManager.Instance.transform.SetParent(newGO.transform); // Remove DontDestroyOnLoad();
                }
                if (LoadingScreen.Instance) LoadingScreen.Instance.EnableLoadingScreen();
                SceneManager.LoadScene(sceneName);
            }
        }
    }
}