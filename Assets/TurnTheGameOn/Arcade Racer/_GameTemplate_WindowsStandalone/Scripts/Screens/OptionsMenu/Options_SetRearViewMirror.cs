namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;

    public class Options_SetRearViewMirror : MonoBehaviour
    {
        public static Options_SetRearViewMirror Instance = null;
        private bool isActive = false;

        void Start()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Debug.LogWarning("Multiple Options_SetRearViewMirror instances assigned, this is not allowed.");
            }
        }

        private void OnEnable()
        {
            SetMirrorState();
        }

        public void SetMirrorState()
        {
            isActive = GameData.GetRearViewMirrorState() == "ON" ? true : false;
            gameObject.SetActive(isActive);
        }
    }
}