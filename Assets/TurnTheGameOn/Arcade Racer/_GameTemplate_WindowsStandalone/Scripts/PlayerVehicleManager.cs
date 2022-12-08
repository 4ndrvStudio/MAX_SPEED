namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;

    public class PlayerVehicleManager : MonoBehaviour
    {
        public static PlayerVehicleManager Instance = null;
        public PlayerVehicles playerVehicles;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
            SelectVehicleIndex(GameData.GetCurrentPlayerVehicleIndex());
        }

        public void SelectVehicleIndex(int i)
        {
            playerVehicles.SelectVehicleIndex(i);
        }

        public void SelectNextVehicle()
        {
            playerVehicles.SelectNextVehicle();
        }

        public void SelectPreviousVehicle()
        {
            playerVehicles.SelectPreviousVehicle();
        }

        public int CurrentSelection()
        {
            return playerVehicles.currentVehicleSelection;
        }

        public string VehicleName()
        {
            return playerVehicles.playerVehicles[playerVehicles.currentVehicleSelection].name;
        }

        public int VehiclePrice()
        {
            return playerVehicles.playerVehicles[playerVehicles.currentVehicleSelection].price;
        }

        public int VehicleStockTopSpeed()
        {
            return playerVehicles.playerVehicles[playerVehicles.currentVehicleSelection].stockTopSpeed;
        }

        public GameObject VehiclePrefab()
        {
            return playerVehicles.playerVehicles[playerVehicles.currentVehicleSelection].vehiclePrefab;
        }

        public string DefaultBodyColorHex()
        {
            return playerVehicles.playerVehicles[playerVehicles.currentVehicleSelection].defaultBodyColorHex;
        }

        public string DefaultWindowColorHex()
        {
            return playerVehicles.playerVehicles[playerVehicles.currentVehicleSelection].defaultWindowColorHex;
        }

        public string DefaultRimColorHex()
        {
            return playerVehicles.playerVehicles[playerVehicles.currentVehicleSelection].defaultRimColorHex;
        }

        public float DefaultWindowAlpha()
        {
            return playerVehicles.playerVehicles[playerVehicles.currentVehicleSelection].defaultWindowAlpha;
        }

    }
}