namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;
    using UnityEngine.UI;

    [System.Serializable]
    public class MiniMapIcon
    {
        public bool isPlayer;
        public bool updateRotation;
        public Image icon;
        public GameObject iconObject;
        public Transform iconTransform;
    }
}