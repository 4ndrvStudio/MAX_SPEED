namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;

    public class MiniMapStreet : MonoBehaviour
    {
        public string streetName;
        public string detectionLayer = "Player";
        void OnTriggerEnter(Collider collisionInfo)
        {
            if (MiniMapCanvas.Instance != null)
            {
                if (collisionInfo.gameObject.layer == LayerMask.NameToLayer(detectionLayer))
                {
                    MiniMapCanvas.Instance.SetStreet(streetName);
                }
            }
        }

    }
}