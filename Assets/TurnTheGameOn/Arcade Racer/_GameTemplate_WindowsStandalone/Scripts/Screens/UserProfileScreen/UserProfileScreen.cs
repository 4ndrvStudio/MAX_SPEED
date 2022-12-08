namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;

    public class UserProfileScreen : ScreenBase
    {
        public static UserProfileScreen Instance = null;

        public GameObject enterProfileNameScreen;
        public GameObject selectUserProfileScreen;

        public TMP_InputField profileNameInputField;

        public GameObject newRacerProfileButton;
        public GameObject submitNewProfileButton;
        public Button cancelCreateProfileButton;

        public UserProfileScreenReferences profile1;
        public UserProfileScreenReferences profile2;
        public UserProfileScreenReferences profile3;
        public UserProfileScreenReferences profile4;

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

        void Start()
        {
            GetSavedProfileInfo();
            if (profile1.name == DefaultGameData.ProfileName)
            {
                firstButtonSelection = profileNameInputField.gameObject;
                screenObject = enterProfileNameScreen;
                cancelCreateProfileButton.interactable = false;
                Open();
            }
            else
            {
                cancelCreateProfileButton.interactable = true;
                MainMenuScreen.Instance.SetProfileData(profile1.name);
                MainMenuScreen.Instance.Open();
            }
        }

        public void SubmitNewProfileName()
        {
            if  (string.IsNullOrEmpty(profileNameInputField.text) == false)
            {
                if (profile1.name == DefaultGameData.ProfileName)
                {
                    profile1.name = profileNameInputField.text;
                    profile1.text.text = profile1.name;
                    GameData.SetProfileName(profile1.name);
                    GameData.SetProfile1Name(profile1.name);
                    profile1.button.interactable = true;
                    firstButtonSelection = profile1.button.gameObject;
                }
                else if (profile2.name == DefaultGameData.ProfileName)
                {
                    profile2.name = profileNameInputField.text;
                    profile2.text.text = profile2.name;
                    GameData.SetProfileName(profile2.name);
                    GameData.SetProfile2Name(profile2.name);
                    profile2.button.interactable = true;
                    firstButtonSelection = profile2.button.gameObject;
                }
                else if (profile3.name == DefaultGameData.ProfileName)
                {
                    profile3.name = profileNameInputField.text;
                    profile3.text.text = profile3.name;
                    GameData.SetProfileName(profile3.name);
                    GameData.SetProfile3Name(profile3.name);
                    profile3.button.interactable = true;
                    firstButtonSelection = profile3.button.gameObject;
                }
                else if (profile4.name == DefaultGameData.ProfileName)
                {
                    profile4.name = profileNameInputField.text;
                    profile4.text.text = profile4.name;
                    GameData.SetProfileName(profile4.name);
                    GameData.SetProfile4Name(profile4.name);
                    profile4.button.interactable = true;
                    firstButtonSelection = profile4.button.gameObject;
                    newRacerProfileButton.GetComponent<Button>().interactable = false;
                }
                cancelCreateProfileButton.interactable = true;
                profileNameInputField.text = "";
                Close();
                screenObject = selectUserProfileScreen;
                Open();
            }
        }

        public void SelectSubmitButton()
        {
            UISelectionManager.Instance.SetNewSelection(submitNewProfileButton);
        }

        public void CreateNewProfile()
        {
            Close();
            screenObject = enterProfileNameScreen;
            firstButtonSelection = profileNameInputField.gameObject;
            Open();
        }

        public void CancelCreateNewProfile()
        {
            Close();
            screenObject = selectUserProfileScreen;
            firstButtonSelection = profile1.button.gameObject;
            Open();
        }

        public void LoadProfile(int _profile)
        {
            if (_profile == 1)
            {
                MainMenuScreen.Instance.SetProfileData(profile1.name);
            }
            else if (_profile == 2)
            {
                MainMenuScreen.Instance.SetProfileData(profile2.name);
            }
            else if (_profile == 3)
            {
                MainMenuScreen.Instance.SetProfileData(profile3.name);
            }
            else if (_profile == 4)
            {
                MainMenuScreen.Instance.SetProfileData(profile4.name);
            }
            cancelCreateProfileButton.interactable = true;
            Close();
            MainMenuScreen.Instance.Open();
        }

        void GetSavedProfileInfo()
        {
            profile1.name = GameData.GetProfile1Name();
            profile2.name = GameData.GetProfile2Name();
            profile3.name = GameData.GetProfile3Name();
            profile4.name = GameData.GetProfile4Name();
            profile1.text.text = profile1.name == "" ? "Empty" : profile1.name;
            profile2.text.text = profile2.name == "" ? "Empty" : profile2.name;
            profile3.text.text = profile3.name == "" ? "Empty" : profile3.name;
            profile4.text.text = profile4.name == "" ? "Empty" : profile4.name;
            profile1.button.interactable = profile1.name == "" ? false : true;
            profile2.button.interactable = profile2.name == "" ? false : true;
            profile3.button.interactable = profile3.name == "" ? false : true;
            profile4.button.interactable = profile4.name == "" ? false : true;
            newRacerProfileButton.GetComponent<Button>().interactable = profile4.name == "" ? true : false;
        }
    }
}