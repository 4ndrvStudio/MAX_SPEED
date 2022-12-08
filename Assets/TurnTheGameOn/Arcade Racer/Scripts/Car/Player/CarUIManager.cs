namespace TurnTheGameOn.ArcadeRacer
{
	using UnityEngine;
	using UnityEngine.UI;

	public class CarUIManager : MonoBehaviour
    {
        public CarUIManager dashboardUI { get; private set; }
        public bool primaryUIController;
		public CarUIAnalogNeedle speedometerNeedle;
		public CarUIAnalogNeedle tachometerNeedle;
		public Slider nitroSlider;
		public Text distanceTypeText;
		public Text speedText;
		public Text gearText;
		public GameObject rearViewMirrorObject;

        private float currentSpeed;
        private float gearFactor;
        private float rpmMinimum;
        private float rpmLimit;
        private bool manualTransmission;
        private int currentGear;
        private float accelInput;
        private float brakeInput;
        private bool isReversing;
        private float nitroAmount;

        public bool templateCar = false;
        public CarDriveSystem vehicleController;
        public CarNitro vehicleControllerNitro;

        public void RegisterDriveSystem(CarDriveSystem _carDriveSystem)
        {
            vehicleController = _carDriveSystem;
            if (vehicleController) vehicleControllerNitro = vehicleController.GetComponent<CarNitro>();
        }

		void Start ()
		{
			if (primaryUIController) 
			{
                if (templateCar)
                {
                    if (GameTemplate_Player.Instance.controllerType == GameTemplate_Player.ControllerType.Default)
                    {
                        GameTemplate_Player.Instance.Set_CarUI(this);
                        dashboardUI = GameTemplate_Player.Instance.GetComponentInChildren<CarUIManager>();
                        switch (GameTemplate_Player.Instance.speedType)
                        {
                            case SpeedType.KPH:
                                distanceTypeText.text = "kph";
                                if (dashboardUI) dashboardUI.distanceTypeText.text = "kph";
                                break;
                            case SpeedType.MPH:
                                distanceTypeText.text = "mph";
                                if (dashboardUI) dashboardUI.distanceTypeText.text = "mph";
                                break;
                        }
                    }
                    else
                    {
                        GameTemplate_Player.Instance.carUI = this;
                    }
                }
                else
                {
                    vehicleController.vehicleUI = this;
                    vehicleControllerNitro = vehicleController.GetComponent<CarNitro>();
                    dashboardUI = vehicleController.GetComponentInChildren<CarUIManager>();
                    switch (vehicleController.vehicleSettings.speedType)
                    {
                        case SpeedType.KPH:
                            distanceTypeText.text = "kph";
                            if (dashboardUI) dashboardUI.distanceTypeText.text = "kph";
                            break;
                        case SpeedType.MPH:
                            distanceTypeText.text = "mph";
                            if (dashboardUI) dashboardUI.distanceTypeText.text = "mph";
                            break;
                    }
                }
			}
			else
			{
				enabled = false;
			}
		}

        void Update()
        {
            if (templateCar)
            {
                currentSpeed = GameTemplate_Player.Instance.CurrentSpeed();
                gearFactor = GameTemplate_Player.Instance.GearFactor();
                rpmMinimum = GameTemplate_Player.Instance.RPM_Minimum();
                rpmLimit = GameTemplate_Player.Instance.RPM_Limit();
                manualTransmission = GameTemplate_Player.Instance.ManualTransmission();
                currentGear = GameTemplate_Player.Instance.CurrentGear();
                accelInput = GameTemplate_Player.Instance.AccelInput();
                brakeInput = GameTemplate_Player.Instance.BrakeInput();
                brakeInput = GameTemplate_Player.Instance.BrakeInput();
                isReversing = GameTemplate_Player.Instance.IsReversing();
                nitroAmount = GameTemplate_Player.Instance.NitroAmount();

                // speedometer needle
                if (dashboardUI) dashboardUI.speedometerNeedle.SetValue(currentSpeed);
                speedometerNeedle.SetValue(currentSpeed);
                // tachometer needle

                if (gearFactor < rpmMinimum) gearFactor = rpmMinimum;
                tachometerNeedle.SetValue(gearFactor * rpmLimit);
                if (dashboardUI) dashboardUI.tachometerNeedle.SetValue(gearFactor * rpmLimit);
                // speed text
                speedText.text = currentSpeed.ToString("F0");
                if (dashboardUI) dashboardUI.speedText.text = currentSpeed.ToString("F0");

                // nitro bar
                nitroSlider.value = nitroAmount;
                if (dashboardUI) dashboardUI.nitroSlider.value = nitroAmount;

                if (gearText && !manualTransmission)
                {
                    if (brakeInput > 0f && isReversing && currentSpeed >= 1)
                    {
                        gearText.text = "R";
                        if (dashboardUI) dashboardUI.gearText.text = "R";
                    }
                    else if (currentGear == 0)
                    {
                        gearText.text = "N";
                        if (dashboardUI) dashboardUI.gearText.text = "N";
                    }
                    if (accelInput > 0f)
                    {
                        gearText.text = (currentGear + 1f).ToString();
                        if (dashboardUI) dashboardUI.gearText.text = (currentGear + 1f).ToString();
                    }
                }
                else if (gearText)
                {
                    gearText.text = currentGear == 0 ? "N" : currentGear == -1 ? "R" : (currentGear).ToString();
                    if (dashboardUI) dashboardUI.gearText.text = currentGear == 0 ? "N" : currentGear == -1 ? "R" : (currentGear).ToString();
                }
            }
            else
            {
                // speedometer needle
                if (dashboardUI) dashboardUI.speedometerNeedle.SetValue(vehicleController.currentSpeed);
                speedometerNeedle.SetValue(vehicleController.currentSpeed);
                // tachometer needle
                gearFactor = vehicleController.gearFactor;
                if (gearFactor < vehicleController.vehicleSettings.minRPM) gearFactor = vehicleController.vehicleSettings.minRPM;
                tachometerNeedle.SetValue(gearFactor * vehicleController.vehicleSettings.RPMLimit);
                if (dashboardUI) dashboardUI.tachometerNeedle.SetValue(gearFactor * vehicleController.vehicleSettings.RPMLimit);
                // speed text
                speedText.text = vehicleController.currentSpeed.ToString("F0");
                if (dashboardUI) dashboardUI.speedText.text = vehicleController.currentSpeed.ToString("F0");

                if (vehicleControllerNitro) // nitro bar
                {
                    nitroSlider.value = vehicleControllerNitro.nitroAmount;
                    if (dashboardUI) dashboardUI.nitroSlider.value = vehicleControllerNitro.nitroAmount;
                }

                if (gearText && !vehicleController.vehicleSettings.manual)
                {
                    if (vehicleController.BrakeInput > 0f && vehicleController.isReversing && vehicleController.currentSpeed >= 1)
                    {
                        gearText.text = "R";
                        if (dashboardUI) dashboardUI.gearText.text = "R";
                    }
                    else if (vehicleController.currentGear == 0)
                    {
                        gearText.text = "N";
                        if (dashboardUI) dashboardUI.gearText.text = "N";
                    }
                    if (vehicleController.AccelInput > 0f)
                    {
                        gearText.text = (vehicleController.currentGear + 1f).ToString();
                        if (dashboardUI) dashboardUI.gearText.text = (vehicleController.currentGear + 1f).ToString();
                    }
                }
                else if (gearText)
                {
                    gearText.text = vehicleController.currentGear == 0 ? "N" : vehicleController.currentGear == -1 ? "R" : (vehicleController.currentGear).ToString();
                    if (dashboardUI) dashboardUI.gearText.text = vehicleController.currentGear == 0 ? "N" : vehicleController.currentGear == -1 ? "R" : (vehicleController.currentGear).ToString();
                }
            }
        }

		public void ToggleRearViewMirror(bool _setActive)
		{
			rearViewMirrorObject.SetActive(_setActive);
		}

	}
}