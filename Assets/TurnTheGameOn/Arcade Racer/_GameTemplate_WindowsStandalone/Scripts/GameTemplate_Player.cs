namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.EventSystems;
    using UnityEngine.SceneManagement;

    public class GameTemplate_Player : MonoBehaviour
    {
        public static GameTemplate_Player Instance = null;
        public enum ControllerType
        {
            Default,
            EVP
        }
        public ControllerType controllerType = ControllerType.Default;
        public MiniMapCanvas miniMapCanvas { get; private set; }
        public bool wasDefeated { get; private set; }
        public bool wrongWay;
        public CarPlayerInputSettings[] playerInputProfiles;
        public CarPlayerInputSettings playerInputSettings;
        public CarDriveSystem driveSystem;
        public AvatarDriverSettings avatarSettings;
        public GameObject miniMapCanvasPrefab;
        public GameObject miniMapCameraPrefab;
        public Image playerMiniMapIconPrefab;
        public GameObject wrongWayWarningCanvasPrefab;
        public Transform waypointArrow;
        [Range(0.0001f, 20)] public float arrowSmooth = 1;
        [Space()]
        public string garageSceneName;
        public Behaviour[] disableIfInGarage;
        public GameObject[] disableObjIfInGarage;
        [Space()]
        public Behaviour[] disableOnPause;
        public Transform arrowTarget { get; private set; }
        private MiniMapCamera miniMapCamera;
        public Vector3 playerVectorForward;
        public Vector3 playerVectorToToOther;
        public int playerIndex { get; private set; }
        public bool useWaypointArrow { get; private set; }
        public bool useWrongWayDetection { get; private set; }
        public CarUIManager carUI;
        public CarNitro carNitro;
        public CarUIManager dashboardCanvas;
        public CarCameraSwitch vehicleCameraSystem;
        public SpeedType speedType = SpeedType.MPH;
        public Behaviour[] frozenControls;
        public bool enableInput = true;


        #region INPUT
        public bool useStandardCanvas { get; private set; }
        public EventTrigger.Entry nitroON;
        public EventTrigger.Entry nitroOFF;
        public EventTrigger.Entry shiftUp;
        public EventTrigger.Entry shiftDown;
        private float accelerationInput, footBrakeInput, horizontalInput, emergencyBrakeInput;
        private bool canShift = true;
        bool canCycleCamera;
        public bool isInitialized { get; private set; }

        public virtual void Start()
        {
            if (!isInitialized) Initialize();
            speedType = driveSystem.vehicleSettings.speedType;
        }

        public void Initialize()
        {
            isInitialized = true;
            if (controllerType == ControllerType.Default)
            {
                carNitro = GetComponent<CarNitro>();
                driveSystem = GetComponent<CarDriveSystem>();
                useStandardCanvas = playerInputSettings.uIType == UIType.Standalone;
                if (playerInputSettings.uIType == UIType.Mobile)
                {
                    if (playerInputSettings.mobileCanvas != null) //Spwan UI Mobile Input
                    {
                        driveSystem.vehicleUI = Instantiate(playerInputSettings.mobileCanvas);
                        driveSystem.vehicleUI.RegisterDriveSystem(driveSystem);
                        MobileControlRig mobileRig = driveSystem.vehicleUI.GetComponentInChildren<MobileControlRig>();
                        mobileRig.vehicleController = (CarDriveSystem)driveSystem as CarDriveSystem;

                        EventTrigger mobileButton = GameObject.Find("Nitro Button").GetComponent<EventTrigger>(); //Setup Nitro UI Button
                        EventTrigger.Entry entry = nitroON;
                        mobileButton.triggers.Add(entry);
                        entry = nitroOFF;
                        mobileButton.triggers.Add(entry);

                        if (driveSystem.vehicleSettings.manual)
                        {
                            mobileButton = GameObject.Find("Shift Up Button").GetComponent<EventTrigger>(); //Setup Shift Up UI Button
                            entry = shiftUp;
                            mobileButton.triggers.Add(entry);

                            mobileButton = GameObject.Find("Shift Down Button").GetComponent<EventTrigger>(); //Setup Shift Down UI Button
                            entry = shiftDown;
                            mobileButton.triggers.Add(entry);
                        }
                    }
                }
                else
                {
                    SpawnDefaultCanvas();
                }
            }
        }

        public virtual void Update()
        {
            if (useWaypointArrow)
            {
                if (arrowTarget != null)
                {
                    arrowTarget.localPosition = Vector3.Lerp(arrowTarget.localPosition, RaceManager.Instance.racerInformation[playerIndex].currentWaypoint.localPosition, arrowSmooth * Time.deltaTime);
                    arrowTarget.localRotation = Quaternion.Lerp(arrowTarget.localRotation, RaceManager.Instance.racerInformation[playerIndex].currentWaypoint.localRotation, arrowSmooth * Time.deltaTime);
                    waypointArrow.LookAt(arrowTarget);
                }
            }
            if (useWrongWayDetection)
            {
                playerVectorForward = RaceManager.Instance.racerInformation[playerIndex].racerObject.transform.TransformDirection(Vector3.forward);
                playerVectorToToOther = RaceManager.Instance.racerInformation[playerIndex].currentWaypoint.localPosition - RaceManager.Instance.racerInformation[playerIndex].racerObject.transform.localPosition;
                wrongWay = (Vector3.Dot(playerVectorForward, playerVectorToToOther) < 0) ? true : false;
            }
            if (enableInput)
            {
                if (vehicleCameraSystem.isLookingBack)
                {
                    if (Input.GetAxisRaw(playerInputSettings.inputAxes.lookBack) == 0) vehicleCameraSystem.OnLookBackKeyUp();
                }
                else
                {
                    if (Input.GetAxisRaw(playerInputSettings.inputAxes.lookBack) == 1) vehicleCameraSystem.LookBackCamera();
                }
                if (carNitro)
                {
                    if (Input.GetAxisRaw(playerInputSettings.inputAxes.nitro) == 1) carNitro.NitroOn();
                    if (Input.GetAxisRaw(playerInputSettings.inputAxes.nitro) == 0) carNitro.NitroOff();
                }
                if (driveSystem.vehicleSettings.manual && canShift)
                {
                    if (Input.GetAxisRaw(playerInputSettings.inputAxes.shiftUp) == 1)
                    {
                        canShift = false;
                        driveSystem.ShiftUp();
                    }
                    if (Input.GetAxisRaw(playerInputSettings.inputAxes.shiftDown) == 1)
                    {
                        canShift = false;
                        driveSystem.ShiftDown();
                    }
                }
                else
                {
                    if (Input.GetAxisRaw(playerInputSettings.inputAxes.shiftUp) == 0 && Input.GetAxisRaw(playerInputSettings.inputAxes.shiftDown) == 0) canShift = true;
                }
            }
        }

        void LateUpdate()
        {
            if (canCycleCamera)
            {
                if (Input.GetAxisRaw(playerInputSettings.inputAxes.cycleCamera) == 1)
                {
                    canCycleCamera = false;
                    vehicleCameraSystem.CycleCamera();
                }
            }
            else if (Input.GetAxisRaw(playerInputSettings.inputAxes.cycleCamera) == 0)
            {
                canCycleCamera = true;
            }
        }

        void FixedUpdate()
        {
            if (enableInput)
            {
                if (controllerType == ControllerType.Default)
                {
                    if (playerInputSettings.uIType == UIType.Mobile)
                    {
                        horizontalInput = MobileInputManager.GetAxis(playerInputSettings.inputAxes.steering);
                        accelerationInput = MobileInputManager.GetAxis(playerInputSettings.inputAxes.throttle);
                        footBrakeInput = playerInputSettings.inputAxes.invertFootBrake ? -1 * MobileInputManager.GetAxis(playerInputSettings.inputAxes.brake) : MobileInputManager.GetAxis(playerInputSettings.inputAxes.brake);
                        emergencyBrakeInput = MobileInputManager.GetAxis(playerInputSettings.inputAxes.handBrake);
                    }
                    else
                    {
                        horizontalInput = Input.GetAxis(playerInputSettings.inputAxes.steering);
                        accelerationInput = Input.GetAxis(playerInputSettings.inputAxes.throttle);
                        footBrakeInput = playerInputSettings.inputAxes.invertFootBrake ? -1 * Input.GetAxis(playerInputSettings.inputAxes.brake) : Input.GetAxis(playerInputSettings.inputAxes.brake);
                        emergencyBrakeInput = Input.GetAxis(playerInputSettings.inputAxes.handBrake);
                    }

                    driveSystem.Move(horizontalInput, accelerationInput, footBrakeInput, emergencyBrakeInput);
                }
            }
        }



        #endregion



        #region Primary Get
        public virtual float SteerInput ()
        {
            return Input.GetAxis(playerInputSettings.inputAxes.steering);
        }

        public virtual float SpeedFactor()
        {
            return driveSystem.currentSpeed / driveSystem.topSpeed;
        }

        public virtual float HorizontalCameraRotation()
        {
            return Input.GetAxis(playerInputSettings.inputAxes.horizontalCameraRotation) * -90;
        }

        public virtual float CurrentSpeed()
        {
            return driveSystem.currentSpeed;
        }

        public virtual float GearFactor()
        {
            return driveSystem.gearFactor;
        }

        public virtual float RPM_Minimum()
        {
            return driveSystem.vehicleSettings.minRPM;
        }

        public virtual float RPM_Limit()
        {
            return driveSystem.vehicleSettings.RPMLimit;
        }

        public virtual float RPM()
        {
            return 0;
        }

        public virtual bool ManualTransmission()
        {
            return driveSystem.vehicleSettings.manual;
        }

        public virtual int CurrentGear()
        {
            return driveSystem.currentGear;
        }

        public virtual float AccelInput()
        {
            return driveSystem.AccelInput;
        }

        public virtual float BrakeInput()
        {
            return driveSystem.BrakeInput;
        }

        public virtual bool IsReversing()
        {
            return driveSystem.isReversing;
        }

        public virtual float NitroAmount()
        {
            return carNitro.nitroAmount;
        }

        public virtual float NitroInput()
        {
            return Input.GetAxisRaw(playerInputSettings.inputAxes.nitro);
        }

        public virtual bool UseStandardCanvas()
        {
            return playerInputSettings.uIType == UIType.Standalone &&
                playerInputSettings.defaultCanvas != null
                ? true : false;
        }
        #endregion

        #region Primary Set
        public virtual void Set_CarUI(CarUIManager ui)
        {
            driveSystem.vehicleUI = ui;
        }
        #endregion

        public void SpawnDefaultCanvas()
        {
            if (UseStandardCanvas())
            {
                carUI = Instantiate(playerInputSettings.defaultCanvas);
            }
            else
            {
                carUI = dashboardCanvas;
                carUI.primaryUIController = true;
            }
        }

        public void EnableControl()
        {
            for (int i = 0; i < frozenControls.Length; i++)
            {
                frozenControls[i].enabled = true;
            }
            enableInput = true;
        }

        public void DisableControl()
        {
            for (int i = 0; i < frozenControls.Length; i++)
            {
                frozenControls[i].enabled = false;
            }
        }

        private void OnEnable()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
            if (SceneManager.GetActiveScene().name == garageSceneName)
            {
                GetComponent<Rigidbody>().isKinematic = true;
                DisableIfInGarage();
            }
            else
            {
                if (RaceManager.Instance) playerIndex = RaceManager.Instance.playerIndex;
                SetupInput();
                SetupWaypointArrow();
                if (controllerType == ControllerType.Default)
                {
                    Initialize();
                }
                else
                {
                    SpawnDefaultCanvas();
                }
                SetTransmissionState();
            }
        }

        //private void OnDisable()
        //{
        //    Instance = null;
        //}

        private void Awake()
        {
            carNitro = GetComponent<CarNitro>();
        }

        void DisableIfInGarage()
        {
            for (int i = 0; i < disableIfInGarage.Length; i++)
            {
                disableIfInGarage[i].enabled = false;
            }
            for (int i = 0; i < disableObjIfInGarage.Length; i++)
            {
                disableObjIfInGarage[i].SetActive(false);
            }
        }

        public void SetupWaypointArrow()
        {
            if (RaceManager.Instance)
            {
                useWaypointArrow = RaceManager.Instance.raceData.showWaypointArrow;
                waypointArrow.gameObject.SetActive(useWaypointArrow);
                if (useWaypointArrow)
                {
                    GameObject newObject = new GameObject();
                    newObject.name = "Arrow Target";
                    arrowTarget = newObject.transform;
                }
            }
        }

        public void SetupWrongWayDetection()
        {
            useWrongWayDetection = true;
            Instantiate(wrongWayWarningCanvasPrefab);
        }

        public void SetupMiniMap(bool _showLocationInfo, bool _lockPositionAndRotation, Vector3 _position, Vector3 _rotation, float _cameraSize)
        {
            if (miniMapCamera == null)
            {
                miniMapCanvas = Instantiate(miniMapCanvasPrefab, Vector3.zero, Quaternion.identity).GetComponent<MiniMapCanvas>();
                miniMapCamera = Instantiate(miniMapCameraPrefab, Vector3.zero, Quaternion.identity).GetComponent<MiniMapCamera>();
                miniMapCanvas.playerTransform = transform;
                miniMapCamera.target = transform;
                miniMapCanvas.miniMapCamera = miniMapCamera.GetComponent<MiniMapCamera>();
                miniMapCanvas.RegisterMiniMapIcon(gameObject, playerMiniMapIconPrefab, true, true);
                if (_lockPositionAndRotation)
                {
                    miniMapCanvas.miniMapCamera.SetFixedPositionAndRotation(_position, _rotation, _cameraSize);
                }
                else
                {
                    miniMapCanvas.miniMapCamera.miniMapCamera.orthographicSize = _cameraSize;
                }
                miniMapCanvas.locationInfo.SetActive(_showLocationInfo);
            }
        }

        public void SetupInput()
        {
            string inputProfile = GameData.GetControllerType();
            for (int i = 0; i < playerInputProfiles.Length; i++)
            {
                if (inputProfile == playerInputProfiles[i].inputAxes.name)
                {
                    playerInputSettings.inputAxes = playerInputProfiles[i].inputAxes;
                    avatarSettings.steeringAxis = playerInputProfiles[i].inputAxes.steering;
                    avatarSettings.throttleAxis = playerInputProfiles[i].inputAxes.throttle;
                }
            }
        }

        public void SetPauseState(bool isPaused)
        {
            for (int i = 0; i < disableOnPause.Length; i++)
            {
                disableOnPause[i].enabled = !isPaused;
            }
            enableInput = !isPaused;
        }

        public void SetDefeated()
        {
            wasDefeated = true;
        }

        #region Options - Transmission
        public void SetTransmissionState()
        {
            if (controllerType == ControllerType.Default)
            {
                CarDriveSystem driveSystem = GetComponent<CarDriveSystem>();
                bool automaticTransmission = GameData.GetAutomaticTransmissionState() == "ON" ? true : false;
                driveSystem.vehicleSettings.manual = !automaticTransmission;
            }
        }
        #endregion

    }
}