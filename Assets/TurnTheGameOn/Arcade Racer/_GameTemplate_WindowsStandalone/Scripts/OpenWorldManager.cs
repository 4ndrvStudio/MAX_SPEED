namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using System.Collections;

    public class OpenWorldManager : MonoBehaviour
    {
        public static OpenWorldManager Instance = null;
        public string sceneName;
        public Transform spawnPoint;
        public GameObject defaultCamera;
        public float minMapSize;

        void OnEnable()
        {
            if (Instance == null)
            {
                Instance = this;
                SceneManager.sceneLoaded += OnSceneLoaded;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            Instance = null;
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == sceneName)
            {
                StartCoroutine("SpawnPlayer");
            }
        }

        public IEnumerator SpawnPlayer()
        {
            yield return new WaitForSeconds(1);
            if (PlayerVehicleManager.Instance == null)
            {
                Debug.LogWarning("This scene requires a PlayerVehicleManager, add one to this scene for testing or load this scene from the main menu.");
            }
            else
            {
                GameObject playerObject = Instantiate(PlayerVehicleManager.Instance.playerVehicles.playerVehicles[PlayerVehicleManager.Instance.playerVehicles.currentVehicleSelection].vehiclePrefab, spawnPoint.position, spawnPoint.rotation);
                defaultCamera.SetActive(false);
                if (LoadingScreen.Instance != null) LoadingScreen.Instance.DisableLoadingScreen(0.1f);
                if (PauseScreen.Instance != null) PauseScreen.Instance.canPause = true;
                GameTemplate_Player playerInput = playerObject.GetComponent<GameTemplate_Player>();
                playerInput.EnableControl();
                playerInput.SetupMiniMap(true, false, Vector3.zero, Vector3.zero, minMapSize);
            }
        }
    }
}