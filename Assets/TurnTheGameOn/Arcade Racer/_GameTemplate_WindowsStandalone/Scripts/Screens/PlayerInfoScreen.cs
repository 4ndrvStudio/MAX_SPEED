namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;
    using TMPro;

    public class PlayerInfoScreen : MonoBehaviour
    {
        public static PlayerInfoScreen Instance = null;
        public TextMeshProUGUI profileNameText;
        public TextMeshProUGUI playerBankText;
        public TextMeshProUGUI playerCarText;
        public TextMeshProUGUI vehicleTopSpeedText;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                SetProfileName(GameData.GetProfileName());
                SetPlayerBank(GameData.GetPlayerBank().ToString("C0"));
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }

        public void SetProfileName(string _profileNameText)
        {
            //if (_profileNameText == "") SceneManager.LoadScene("MainMenu");
            profileNameText.text = "Profile: " + _profileNameText;
        }

        public void SetPlayerBank(string _moneyText)
        {
            playerBankText.text = "Bank: " + _moneyText;
        }

        public void SetPlayerCar(string _carText)
        {
            playerCarText.text = "Car: " + _carText;
        }

        public void SetVehicleTopSpeed(string _topSpeed)
        {
            vehicleTopSpeedText.text = "Top Speed: " + _topSpeed;
        }
    }
}