namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using TMPro;

    public class PlayerInfoScreen_MainMenu : MonoBehaviour
    {
        public static PlayerInfoScreen_MainMenu Instance = null;
        public TextMeshProUGUI profileNameText;
        public TextMeshProUGUI playerBankText;
        public TextMeshProUGUI playerCarText;
        public GameObject infoObject;

        void Awake()
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

        public void SetProfileName(string _profileNameText)
        {
            if (_profileNameText == "") SceneManager.LoadScene("OpenWorldTemplate_MainMenu");
            profileNameText.text = "User Profile: " + _profileNameText;
        }

        public void SetPlayerBank(string _moneyText)
        {
            playerBankText.text = "Bank: " + _moneyText;
        }

        public void SetPlayerCar(string _carText)
        {
            playerCarText.text = "Car: " + _carText;
        }

        public void RefreshData()
        {
            SetProfileName(GameData.GetProfileName());
            SetPlayerBank(GameData.GetPlayerBank().ToString("C0"));
            SetPlayerCar(PlayerVehicleManager.Instance.playerVehicles.playerVehicles[GameData.GetCurrentPlayerVehicleIndex()].name);
        }

        public void Open()
        {
            RefreshData();
            infoObject.SetActive(true);
        }

        public void Close()
        {
            infoObject.SetActive(false);
        }

    }
}