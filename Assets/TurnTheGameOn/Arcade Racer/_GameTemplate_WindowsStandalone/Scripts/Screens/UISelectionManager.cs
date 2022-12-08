namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    [RequireComponent(typeof(EventSystem))]
    [RequireComponent(typeof(StandaloneInputModule))]
    public class UISelectionManager : MonoBehaviour
    {
        public static UISelectionManager Instance = null;
        public bool enableControllerNavigation;
        private EventSystem eventSystem;
        private StandaloneInputModule standaloneInputModule;
        public GameObject selected { get; private set; }
        public CarPlayerInputSettings[] playerInputSettings;
        private GameObject previousSelection;

        void Start()
        {
            if (Instance == null)
            {
                Instance = this;
                eventSystem = GetComponent<EventSystem>();
                eventSystem.enabled = true;
                standaloneInputModule = GetComponent<StandaloneInputModule>();
                SetInputModuleForControllerType(GameData.GetControllerType());
                DontDestroyOnLoad(gameObject);
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }

        void Update()
        {
            if (eventSystem.currentSelectedGameObject != selected)
            {
                if (eventSystem.currentSelectedGameObject != null)
                {
                    selected = eventSystem.currentSelectedGameObject;
                    eventSystem.SetSelectedGameObject(null);
                    eventSystem.SetSelectedGameObject(selected);
                }

                if (eventSystem.currentSelectedGameObject == null)
                {
                    eventSystem.SetSelectedGameObject(previousSelection);
                }
                if (selected.GetComponent<Button>())
                {
                    selected.GetComponent<Button>().Select();
                    selected.GetComponent<Button>().SendMessage("OnSelectedEvent", SendMessageOptions.DontRequireReceiver);
                }
                else if (selected.GetComponent<Toggle>())
                {
                    selected.GetComponent<Toggle>().Select();
                }
                else if (selected.GetComponent<TMPro.TMP_InputField>())
                {
                    selected.GetComponent<TMPro.TMP_InputField>().Select();
                    selected.GetComponent<TMPro.TMP_InputField>().ActivateInputField();
                }
                CheckSetMainMenuDescription(); // needs to be a subscribe event
                previousSelection = selected;
            }
            else { }
        }

        public void SetNewSelection(GameObject _newSelection)
        {
            //eventSystem.SetSelectedGameObject(null);
            if (!eventSystem.alreadySelecting) eventSystem.SetSelectedGameObject(_newSelection);
            selected = _newSelection;
            if (selected)
            {
                if (selected.GetComponent<Button>())
                {
                    selected.GetComponent<Button>().Select();
                    CheckSetMainMenuDescription();
                }
                else if (selected.GetComponent<Toggle>())
                {
                    selected.GetComponent<Toggle>().Select();
                }
                else if (selected.GetComponent<TMPro.TMP_InputField>())
                {
                    selected.GetComponent<TMPro.TMP_InputField>().Select();
                    selected.GetComponent<TMPro.TMP_InputField>().ActivateInputField();
                }
            }
        }

        public void SetInputModuleForControllerType(string _inputType)
        {
            for (int i = 0; i < playerInputSettings.Length; i++)
            {
                if (playerInputSettings[i].inputModuleSettings.inputType == _inputType)
                {
                    standaloneInputModule.horizontalAxis = playerInputSettings[i].inputModuleSettings.horizontalAxis;
                    standaloneInputModule.verticalAxis = playerInputSettings[i].inputModuleSettings.verticalAxis;
                    standaloneInputModule.submitButton = playerInputSettings[i].inputModuleSettings.submitButton;
                    standaloneInputModule.cancelButton = playerInputSettings[i].inputModuleSettings.cancelButton;
                    PauseScreen.Instance.SetupPauseButton(playerInputSettings[i].pause);
                    break;
                }
            }
            TryToUpdateVehicleControllerIfAvailable();
        }

        void TryToUpdateVehicleControllerIfAvailable()
        {
            GameTemplate_Player currentPlayerController = FindObjectOfType<GameTemplate_Player>();
            if (currentPlayerController) currentPlayerController.SetupInput();
        }

        void CheckSetMainMenuDescription()
        {
            if (MainMenuScreen.Instance)
            {
                if (MainMenuScreen.Instance.screenObject.activeInHierarchy)
                {
                    MainMenuScreen.Instance.SetMenuDescription(selected.name);
                }
            }
        }

    }
}