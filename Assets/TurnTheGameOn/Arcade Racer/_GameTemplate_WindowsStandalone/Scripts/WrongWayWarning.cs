namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;

    public class WrongWayWarning : MonoBehaviour
    {
        public static WrongWayWarning Instance;
        public GameObject wrongWayWarningObject;
        private float wrongWayTimer;
        public float wrongWayDelay;

        private void Awake()
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

        private void Update()
        {
            if (GameTemplate_Player.Instance.wrongWay)
            {
                wrongWayTimer += 1 * Time.deltaTime;
                if (wrongWayTimer >= wrongWayDelay)
                {
                    wrongWayWarningObject.SetActive(true);
                }
            }
            else
            {
                wrongWayTimer = 0;
                wrongWayWarningObject.SetActive(false);
            }
        }
    }
}