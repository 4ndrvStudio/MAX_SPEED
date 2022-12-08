namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;

    public class MiniMapDistrict : MonoBehaviour
    {
        public string districtName;
        public string detectionLayer = "Player";
        void OnTriggerEnter(Collider collisionInfo)
        {
            if (MiniMapCanvas.Instance != null)
            {
                if (collisionInfo.gameObject.layer == LayerMask.NameToLayer(detectionLayer))
                {
                    MiniMapCanvas.Instance.SetDistrict(districtName);
                }
            }
        }

    }
}