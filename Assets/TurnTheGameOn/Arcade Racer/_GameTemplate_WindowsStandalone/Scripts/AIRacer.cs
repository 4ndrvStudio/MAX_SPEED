namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;
    using UnityEngine.UI;

    public class AIRacer : MonoBehaviour
    {
        public CarAI AIController;
        public Image opponentMiniMapIconPrefab;

        public void EnableControl()
        {
            AIController.enabled = true;
        }

        public void DisableControl()
        {
            AIController.StopDriving();
        }

        private void Start()
        {
            GameTemplate_Player.Instance.miniMapCanvas.RegisterMiniMapIcon(gameObject, opponentMiniMapIconPrefab, true, false);
        }

    }
}