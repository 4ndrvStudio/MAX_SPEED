namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;

    [System.Serializable]
    public class PlayerVehicle
    {
        public string name;
        public int price;
        public int stockTopSpeed;
        public bool owned;
        public GameObject vehiclePrefab;
        public string defaultBodyColorHex = "ffffff";
        public string defaultWindowColorHex = "ffffff";
        public string defaultRimColorHex = "ffffff";
        [Range(0,1)] public float defaultWindowAlpha = 0.4f;
    }
}