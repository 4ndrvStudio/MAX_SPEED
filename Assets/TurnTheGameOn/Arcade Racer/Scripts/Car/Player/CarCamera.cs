namespace TurnTheGameOn.ArcadeRacer
{
	using UnityEngine;
	using UnityEngine.EventSystems;

    public class CarCamera : MonoBehaviour
	{		
		public Transform cameraTarget;
        public Rigidbody vehicleRigidbody;
		public CarDriveSystem carController;
		public CarNitro carControllerNitro;
		public CarCameraSwitch cameraSwitchToggle;
		public float normalHeight;
		public float distance;
		public float DefaultFOV;
		public float rotation;
		public EventTrigger.Entry switchCameraEvent;
		public CameraType cameraType;
		public Camera carCamera;
		public float rotationDamping;
		public float heightDamping;
		public float zoomRatio;
		[Header("Camera Pivot")]
		public float max;
		public float value;
		public float minPivotMoveSpeed;
		public float maxPivotMoveSpeed;
		public float pivotMoveSpeed;
		public float normalZoomRation;
		public float nitroZoomRatio;
		public float nitroHeightMultiplier;
		private float speedFactor;
		private float currentHeight;
		private float eulerY;
		private float height;
		private Quaternion currentRotation;
		private Vector3 targetPivotPosition;
		private Vector3 rotationVector;
		private Vector3 position;
        private float cameraRotation;
        private float steerInput;
        public bool templateCar = false;
        private CarPlayerInput playerInput;

        void Start()
		{
            if (carController) playerInput = carController.GetComponent<CarPlayerInput>();
            cameraSwitchToggle.OnChangeState("ChaseCamera");
        }

        void LateUpdate()
        {
            if (templateCar)
            {
                if (cameraTarget != null)
                {
                    if (cameraType == CameraType.ChaseCamera)
                    {
                        eulerY = Mathf.LerpAngle(transform.eulerAngles.y, rotationVector.y, rotationDamping * Time.deltaTime);
                        height = Mathf.Lerp(transform.position.y, cameraTarget.position.y + currentHeight, heightDamping * Time.deltaTime);
                        currentRotation = Quaternion.Euler(0, eulerY, 0);
                        transform.position = cameraTarget.position;
                        transform.position -= currentRotation * Vector3.forward * distance;
                        position = transform.position;
                        position.y = height;
                        transform.position = Vector3.Lerp(transform.position, position, 1);
                        transform.LookAt(cameraTarget);
                        if (GameTemplate_Player.Instance.controllerType == GameTemplate_Player.ControllerType.Default)
                        {
                            steerInput = GameTemplate_Player.Instance.SteerInput();
                            speedFactor = GameTemplate_Player.Instance.SpeedFactor();
                        }
                        else if (GameTemplate_Player.Instance.controllerType == GameTemplate_Player.ControllerType.EVP)
                        {
                            steerInput = GameTemplate_Player.Instance.SteerInput();
                            speedFactor = GameTemplate_Player.Instance.SpeedFactor();
                        }
                        value = steerInput > 0 ? -max : steerInput < 0 ? max : 0;
                        pivotMoveSpeed = steerInput != 0 ? minPivotMoveSpeed : Mathf.Lerp(pivotMoveSpeed, maxPivotMoveSpeed, 1 * Time.deltaTime);
                        carCamera.transform.localRotation = Quaternion.Euler(carCamera.transform.localRotation.eulerAngles.x, rotation, carCamera.transform.localRotation.eulerAngles.z);

                        targetPivotPosition.x = Mathf.Lerp(targetPivotPosition.x, value, pivotMoveSpeed * Time.deltaTime);
                        carCamera.transform.localPosition = targetPivotPosition;



                        if (carControllerNitro)
                        {
                            if (carControllerNitro.nitroOn)
                            {
                                speedFactor *= 2.0f;
                                zoomRatio = Mathf.Lerp(zoomRatio, nitroZoomRatio, Time.deltaTime);
                                currentHeight = Mathf.Lerp(currentHeight, normalHeight + (nitroHeightMultiplier * speedFactor), Time.deltaTime);
                            }
                            else
                            {
                                zoomRatio = Mathf.Lerp(zoomRatio, normalZoomRation, Time.deltaTime);
                                currentHeight = Mathf.Lerp(currentHeight, normalHeight, Time.deltaTime);
                            }
                        }
                        else
                        {
                            zoomRatio = Mathf.Lerp(zoomRatio, normalZoomRation, Time.deltaTime);
                            currentHeight = Mathf.Lerp(currentHeight, normalHeight, Time.deltaTime);
                        }
                    }
                }
            }
            else
            {
                if (cameraTarget != null)
                {
                    if (cameraType == CameraType.ChaseCamera)
                    {
                        eulerY = Mathf.LerpAngle(transform.eulerAngles.y, rotationVector.y, rotationDamping * Time.deltaTime);
                        height = Mathf.Lerp(transform.position.y, cameraTarget.position.y + currentHeight, heightDamping * Time.deltaTime);
                        currentRotation = Quaternion.Euler(0, eulerY, 0);
                        transform.position = cameraTarget.position;
                        transform.position -= currentRotation * Vector3.forward * distance;
                        position = transform.position;
                        position.y = height;
                        transform.position = Vector3.Lerp(transform.position, position, 1);
                        transform.LookAt(cameraTarget);
                        value = Input.GetAxis(playerInput.playerInputSettings.inputAxes.steering) > 0 ? -max : Input.GetAxis(playerInput.playerInputSettings.inputAxes.steering) < 0 ? max : 0;
                        pivotMoveSpeed = Input.GetAxis(playerInput.playerInputSettings.inputAxes.steering) != 0 ? minPivotMoveSpeed : Mathf.Lerp(pivotMoveSpeed, maxPivotMoveSpeed, 1 * Time.deltaTime);
                        carCamera.transform.localRotation = Quaternion.Euler(carCamera.transform.localRotation.eulerAngles.x, rotation, carCamera.transform.localRotation.eulerAngles.z);

                        targetPivotPosition.x = Mathf.Lerp(targetPivotPosition.x, value, pivotMoveSpeed * Time.deltaTime);
                        carCamera.transform.localPosition = targetPivotPosition;

                        speedFactor = carController.currentSpeed / carController.vehicleSettings.topSpeed;

                        if (carControllerNitro)
                        {
                            if (carControllerNitro.nitroOn)
                            {
                                speedFactor *= 2.0f;
                                zoomRatio = Mathf.Lerp(zoomRatio, nitroZoomRatio, Time.deltaTime);
                                currentHeight = Mathf.Lerp(currentHeight, normalHeight + (nitroHeightMultiplier * speedFactor), Time.deltaTime);
                            }
                            else
                            {
                                zoomRatio = Mathf.Lerp(zoomRatio, normalZoomRation, Time.deltaTime);
                                currentHeight = Mathf.Lerp(currentHeight, normalHeight, Time.deltaTime);
                            }
                        }
                        else
                        {
                            zoomRatio = Mathf.Lerp(zoomRatio, normalZoomRation, Time.deltaTime);
                            currentHeight = Mathf.Lerp(currentHeight, normalHeight, Time.deltaTime);
                        }
                    }
                }
            }
        }

        void FixedUpdate()
        {
            if (cameraTarget && cameraType == CameraType.ChaseCamera)
            {
                if (templateCar)
                {
                    cameraRotation = GameTemplate_Player.Instance.HorizontalCameraRotation();

                    if (GameTemplate_Player.Instance.playerInputSettings.uIType == UIType.Mobile)
                    {
                        var localVelocity = cameraTarget.InverseTransformDirection(vehicleRigidbody.velocity);
                        if (localVelocity.z < -0.5f && Input.GetAxis(GameTemplate_Player.Instance.playerInputSettings.inputAxes.throttle) == -1)
                        {
                            rotationVector.y = cameraTarget.eulerAngles.y + cameraRotation + 180f;
                        }
                        else
                        {
                            rotationVector.y = cameraTarget.eulerAngles.y + cameraRotation;
                        }
                        var acc = vehicleRigidbody.velocity.magnitude;
                        carCamera.fieldOfView = DefaultFOV + acc * zoomRatio * Time.deltaTime;
                    }
                    else
                    {
                        var localVelocity = cameraTarget.InverseTransformDirection(vehicleRigidbody.velocity);
                        if (localVelocity.z < -0.5f && Input.GetAxis(GameTemplate_Player.Instance.playerInputSettings.inputAxes.throttle) == -1)
                        {
                            rotationVector.y = cameraTarget.eulerAngles.y + cameraRotation + 180f;
                        }
                        else
                        {
                            rotationVector.y = cameraTarget.eulerAngles.y + cameraRotation;
                        }
                        var acc = vehicleRigidbody.velocity.magnitude;
                        carCamera.fieldOfView = DefaultFOV + acc * zoomRatio * Time.deltaTime;
                    }
                }
                else
                {
                    cameraRotation = Input.GetAxis(playerInput.playerInputSettings.inputAxes.horizontalCameraRotation) * -90;
                    if (playerInput.playerInputSettings.uIType == UIType.Mobile)
                    {
                        var localVelocity = cameraTarget.InverseTransformDirection(vehicleRigidbody.velocity);
                        if (localVelocity.z < -0.5f && Input.GetAxis(playerInput.playerInputSettings.inputAxes.throttle) == -1)
                        {
                            rotationVector.y = cameraTarget.eulerAngles.y + cameraRotation + 180f;
                        }
                        else
                        {
                            rotationVector.y = cameraTarget.eulerAngles.y + cameraRotation;
                        }
                        var acc = vehicleRigidbody.velocity.magnitude;
                        carCamera.fieldOfView = DefaultFOV + acc * zoomRatio * Time.deltaTime;
                    }
                    else
                    {
                        var localVelocity = cameraTarget.InverseTransformDirection(vehicleRigidbody.velocity);
                        if (localVelocity.z < -0.5f && Input.GetAxis(playerInput.playerInputSettings.inputAxes.throttle) == -1)
                        {
                            rotationVector.y = cameraTarget.eulerAngles.y + cameraRotation + 180f;
                        }
                        else
                        {
                            rotationVector.y = cameraTarget.eulerAngles.y + cameraRotation;
                        }
                        var acc = vehicleRigidbody.velocity.magnitude;
                        carCamera.fieldOfView = DefaultFOV + acc * zoomRatio * Time.deltaTime;
                    }
                }

                
            }
        }

	}
}