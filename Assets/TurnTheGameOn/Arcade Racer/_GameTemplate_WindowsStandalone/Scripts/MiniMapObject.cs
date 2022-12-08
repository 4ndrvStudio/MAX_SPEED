namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections;

    public class MiniMapObject : MonoBehaviour
    {
        public Image miniMapIconPrefab;
        private MiniMapCanvas miniMapCanvas;

        void Start()
        {
            StartCoroutine(RegisterMiniMapIcon());
        }

        private void OnDestroy()
        {
            if (miniMapCanvas && gameObject) miniMapCanvas.RemoveMiniMapIcon(gameObject);
        }

        IEnumerator RegisterMiniMapIcon()
        {
            while (miniMapCanvas == null)
            {
                miniMapCanvas = FindObjectOfType<MiniMapCanvas>();
                if (miniMapCanvas == null)
                {
                    yield return new WaitForSeconds(0.1f);
                }
            }
            miniMapCanvas.RegisterMiniMapIcon(gameObject, miniMapIconPrefab, false, false);
        }

    }
}