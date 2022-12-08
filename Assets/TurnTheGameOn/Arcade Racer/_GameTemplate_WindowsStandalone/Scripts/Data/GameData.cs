namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;

    public static class GameData
    {

        #region AudioState
        public static string GetAudioState()
        {
            return PlayerPrefs.GetString("AudioState_" + GetProfileName(), DefaultGameData.AudioState);
        }
        public static void SetAudioState(string newValue)
        {
            PlayerPrefs.SetString("AudioState_" + GetProfileName(), newValue);
        }
        #endregion

        #region QualitySettingsLevel
        public static int GetQualitySettingsLevel()
        {
            return PlayerPrefs.GetInt("QualitySettingsLevel_" + GetProfileName(), DefaultGameData.QualitySettingsLevel);
        }
        public static void SetQualitySettingsLevel(int newValue)
        {
            PlayerPrefs.SetInt("QualitySettingsLevel_" + GetProfileName(), newValue);
        }
        #endregion

        #region ControllerType
        public static string GetControllerType()
        {
            return PlayerPrefs.GetString("ControllerType_" + GetProfileName(), DefaultGameData.ControllerType);
        }
        public static void SetControllerType(string newValue)
        {
            PlayerPrefs.SetString("ControllerType_" + GetProfileName(), newValue);
        }
        #endregion

        #region AutomaticTransmissionState
        public static string GetAutomaticTransmissionState()
        {
            return PlayerPrefs.GetString("AutomaticTransmissionState_" + GetProfileName(), DefaultGameData.AutomaticTransmissionState);
        }
        public static void SetAutomaticTransmissionState(string newValue)
        {
            PlayerPrefs.SetString("AutomaticTransmissionState_" + GetProfileName(), newValue);
        }
        #endregion

        #region RearViewMirrorState
        public static string GetRearViewMirrorState()
        {
            return PlayerPrefs.GetString("RearViewMirrorState_" + GetProfileName(), DefaultGameData.RearViewMirrorState);
        }
        public static void SetRearViewMirrorState(string newValue)
        {
            PlayerPrefs.SetString("RearViewMirrorState_" + GetProfileName(), newValue);
        }
        #endregion

        #region FixedMiniMapRotationState
        public static string GetFixedMiniMapRotationState()
        {
            return PlayerPrefs.GetString("FixedMiniMapRotationState_" + GetProfileName(), DefaultGameData.FixedMiniMapRotationState);
        }
        public static void SetFixedMiniMapRotationState(string newValue)
        {
            PlayerPrefs.SetString("FixedMiniMapRotationState_" + GetProfileName(), newValue);
        }
        #endregion

        #region MiniMapState
        public static string GetMiniMapState()
        {
            return PlayerPrefs.GetString("MiniMapState_" + GetProfileName(), DefaultGameData.MiniMapState);
        }
        public static void SetMiniMapState(string newValue)
        {
            PlayerPrefs.SetString("MiniMapState_" + GetProfileName(), newValue);
        }
        #endregion

        #region PlayerVehicle
        public static int GetCurrentPlayerVehicleIndex()
        {
            return PlayerPrefs.GetInt("CurrentPlayerVehicleIndex_" + GetProfileName(), DefaultGameData.CurrentPlayerVehicleIndex);
        }
        public static void SetCurrentPlayerVehicleIndex(int newValue)
        {
            PlayerPrefs.SetInt("CurrentPlayerVehicleIndex_" + GetProfileName(), newValue);
        }
        #endregion

        #region ProfileName
        public static string GetProfileName()
        {
            return PlayerPrefs.GetString("ProfileName", DefaultGameData.ProfileName);
        }
        public static void SetProfileName(string newValue)
        {
            PlayerPrefs.SetString("ProfileName", newValue);
        }
        #endregion

        #region Profile1Name
        public static string GetProfile1Name()
        {
            return PlayerPrefs.GetString("Profile1Name", DefaultGameData.ProfileName);
        }
        public static void SetProfile1Name(string newValue)
        {
            PlayerPrefs.SetString("Profile1Name", newValue);
        }
        #endregion

        #region Profile2Name
        public static string GetProfile2Name()
        {
            return PlayerPrefs.GetString("Profile2Name", DefaultGameData.ProfileName);
        }
        public static void SetProfile2Name(string newValue)
        {
            PlayerPrefs.SetString("Profile2Name", newValue);
        }
        #endregion

        #region Profile3Name
        public static string GetProfile3Name()
        {
            return PlayerPrefs.GetString("Profile3Name", DefaultGameData.ProfileName);
        }
        public static void SetProfile3Name(string newValue)
        {
            PlayerPrefs.SetString("Profile3Name", newValue);
        }
        #endregion

        #region Profile4Name
        public static string GetProfile4Name()
        {
            return PlayerPrefs.GetString("Profile4Name", DefaultGameData.ProfileName);
        }
        public static void SetProfile4Name(string newValue)
        {
            PlayerPrefs.SetString("Profile4Name", newValue);
        }
        #endregion

        #region PlayerBank
        public static int GetPlayerBank()
        {
            return PlayerPrefs.GetInt("PlayerBank_" + GetProfileName(), DefaultGameData.PlayerBank);
        }
        public static void SetPlayerBank(int newValue)
        {
            PlayerPrefs.SetInt("PlayerBank_" + GetProfileName(), newValue);
        }
        #endregion

        #region CarOwned
        public static bool GetCarOwned(int _index)
        {
            if (PlayerPrefs.GetInt("CarOwned_" + _index + "_" + GetProfileName(), 0) == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public static void SetCarOwned(int _index, int newValue)
        {
            PlayerPrefs.SetInt("CarOwned_" + _index + "_" + GetProfileName(), newValue);
        }
        #endregion

        #region RaceUnlocked
        public static bool GetRaceUnlocked(int _index)
        {
            if (PlayerPrefs.GetInt("RaceUnlocked_" + _index + "_" + GetProfileName(), 0) == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public static void SetRaceUnlocked(int _index, int newValue)
        {
            PlayerPrefs.SetInt("RaceUnlocked_" + _index + "_" + GetProfileName(), newValue);
        }
        #endregion

        #region BestLapTime
        public static float GetBestLapTime(int _id)
        {
            return PlayerPrefs.GetFloat("BestLapTime_" + _id  + GetProfileName(), 99999.0f);
        }
        public static void SetBestLapTime(int _id, float newValue)
        {
            PlayerPrefs.SetFloat("BestLapTime_" + _id + GetProfileName(), newValue);
        }
        #endregion

        #region BestSeriesFinish
        public static float GetBestSeriesFinish(int _id)
        {
            return PlayerPrefs.GetInt("BestSeriesFinish_" + _id + GetProfileName(), 99999);
        }
        public static void SetBestSeriesFinish(int _id, int newValue)
        {
            PlayerPrefs.SetInt("BestSeriesFinish_" + _id + GetProfileName(), newValue);
        }
        #endregion

        #region CarBodyColorHex
        public static string GetCarBodyColorHex(int carIndex)
        {
            return PlayerPrefs.GetString("CarBodyColorHex_" + GetProfileName() + "_" + carIndex.ToString(), "");
        }
        public static void SetCarBodyColorHex(int carIndex, string newValue)
        {
            PlayerPrefs.SetString("CarBodyColorHex_" + GetProfileName() + "_" + carIndex.ToString(), newValue);
        }
        #endregion

        #region CarRimColorHex
        public static string GetCarRimColorHex(int carIndex)
        {
            return PlayerPrefs.GetString("CarRimColorHex_" + GetProfileName() + "_" + carIndex.ToString(), "");
        }
        public static void SetCarRimColorHex(int carIndex, string newValue)
        {
            PlayerPrefs.SetString("CarRimColorHex_" + GetProfileName() + "_" + carIndex.ToString(), newValue);
        }
        #endregion

        #region CarWindowColorHex
        public static string GetCarWindowColorHex(int carIndex)
        {
            return PlayerPrefs.GetString("CarWindowColorHex_" + GetProfileName() + "_" + carIndex.ToString(), "");
        }
        public static void SetCarWindowColorHex(int carIndex, string newValue)
        {
            PlayerPrefs.SetString("CarWindowColorHex_" + GetProfileName() + "_" + carIndex.ToString(), newValue);
        }
        #endregion

        #region CarNeonLightColorHex
        public static string GetCarNeonLightColorHex(int carIndex)
        {
            return PlayerPrefs.GetString("CarNeonLightColorHex_" + GetProfileName() + "_" + carIndex.ToString(), "");
        }
        public static void SetCarNeonLightColorHex(int carIndex, string newValue)
        {
            PlayerPrefs.SetString("CarNeonLightColorHex_" + GetProfileName() + "_" + carIndex.ToString(), newValue);
        }
        #endregion

        #region CarNeonLightState
        public static string GetCarNeonLightState(int carIndex)
        {
            return PlayerPrefs.GetString("CarNeonLightState_" + GetProfileName() + "_" + carIndex.ToString(), DefaultGameData.neonLightState);
        }
        public static void SetCarNeonLightState(int carIndex, string newValue)
        {
            PlayerPrefs.SetString("CarNeonLightState_" + GetProfileName() + "_" + carIndex.ToString(), newValue);
        }
        #endregion
    }
}