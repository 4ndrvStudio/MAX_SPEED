using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using FishNet.Object.Prediction;
using FishNet.Transporting;
using FishNet.Managing.Timing;
using FishNet.Component.Prediction;

using UnityEngine;
using FishNet;
using System;
using Unity.VisualScripting;
using Cinemachine;
using System.Linq;
using Utilities;

[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor;
    public bool steering;
    public WheelFrictionCurve originalForwardFriction;
    public WheelFrictionCurve originalSidewayFriction;

}


[System.Serializable]
public struct MoveData : IReplicateData
{
    public Vector2 MoveDirection;
    public bool IsBraking;

    private uint _tick;
    public void Dispose() { }
    public uint GetTick() => _tick;
    public void SetTick(uint value) => _tick = value;
}

//ReconcileData for Reconciliation
public struct ReconcileData : IReconcileData
{
    public Vector3 Position;
    public Quaternion Rotation;
    public Vector3 Velocity;
    public Vector3 AngularVelocity;

    public ReconcileData(Vector3 position, Quaternion rotation, Vector3 velocity, Vector3 angularVelocity)
    {
        Position = position;
        Rotation = rotation;
        Velocity = velocity;
        AngularVelocity = angularVelocity;
        _tick = 0;
    }

    private uint _tick;
    public void Dispose() { }
    public uint GetTick() => _tick;
    public void SetTick(uint value) => _tick = value;

}

public class PredictedCar : NetworkBehaviour
{
    [Header("Refs")]
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private CinemachineVirtualCamera _playerCamera;

    [Space]
    [Header("Axle Info")]
    [SerializeField] private AxleInfo[] _axleInfos;

    [Header("Motor Attributes")]
    [SerializeField] private float _maxMotorTorque = 3000f;
    [SerializeField] float _maxSpeed;

    [Header("Braking Attr")]
    [SerializeField] private float _brakeTorque = 10000f;
    [SerializeField] private float _driftSteerMultiplier = 1.5f;


    [Header("Sterring Attr")]
    [SerializeField] private float _maxSteeringAngle = 30f;
    [SerializeField] private AnimationCurve _turnCurve;
    [SerializeField] private float _turnStrength = 1500f;

    [Header("Car Physics")]
    [SerializeField] private Transform _centerOfMass;
    [SerializeField] private float _downForce;
    [SerializeField] private float _gravity = Physics.gravity.y;
    [SerializeField] private float _lateralGScale = 10f;

    [Header("Banking")]
    [SerializeField] private float _maxBankAngle = 5f;
    [SerializeField] private float _bankSpeed = 2f;

    private float _brakeVelocity;
    private Vector3 _carVelocity;
    private float _driftVelocity;

    RaycastHit _hit;
    const float _thresholdSpeed = 10f;
    const float _centerOfMassOffset = -0.5f;
    Vector3 _originalCenterOfMass;

    public bool IsGrounded = true;
    public Vector3 Velocity => _carVelocity;
    public float MaxSpeed => _maxSpeed;

    private bool _subscribed = false;

    private MoveData _clientMoveData;

    private void ChangeSubscriptions(bool subscribe)
    {
        if (base.TimeManager == null)
            return;
        if (subscribe == _subscribed)
            return;

        _subscribed = subscribe;

        if (subscribe)
        {
            base.TimeManager.OnTick += TimeManager_OnTick;
        }
        else
        {
            base.TimeManager.OnTick -= TimeManager_OnTick;
        }
    }

    private void Start()
    {
        ChangeSubscriptions(true);
    }

    private void Awake()
    {
        _rb.centerOfMass = _centerOfMass.localPosition;
        _originalCenterOfMass = _centerOfMass.localPosition;
        foreach (AxleInfo axleInfo in _axleInfos)
        {
            axleInfo.originalForwardFriction = axleInfo.leftWheel.forwardFriction;
            axleInfo.originalSidewayFriction = axleInfo.leftWheel.sidewaysFriction;
        }

    }

 
    public override void OnStartClient()
    {
        base.OnStartClient();


        if (!IsOwner)
        {
            _playerCamera.Priority = 0;
            return;
        }

        _playerCamera.Priority = 100;
    }

    public override void OnStopNetwork()
    {
        base.OnStopNetwork();
        ChangeSubscriptions(false);
    }

    private void OnDestroy()
    {
        ChangeSubscriptions(false);
    }

    private void CheckInput(out MoveData md)
    {
        md = default;

        CarMyByButton gasPedal = CarUI.Instance.gasPedal;
        CarMyByButton brakePedal = CarUI.Instance.brakePedal;
        CarMyByButton rightButton = CarUI.Instance.rightButton;
        CarMyByButton leftButton = CarUI.Instance.leftButton;

        //float gasInput = Input.GetAxis("Vertical");

        //if (gasPedal.isPressed)
        //{
        //    gasInput += gasPedal.dampenPress;
        //}
        //if (brakePedal.isPressed)
        //{
        //    gasInput -= brakePedal.dampenPress;
        //}

        //float steeringInput = Input.GetAxis("Horizontal");

        //if (rightButton.isPressed)
        //{
        //    steeringInput += rightButton.dampenPress;
        //}
        //if (leftButton.isPressed)
        //{
        //    steeringInput -= leftButton.dampenPress;
        //}

        //float brakeInput = 0;

        //float movingDirection = Vector3.Dot(transform.forward, _rb.velocity);
        //if (movingDirection < -0.5f && gasInput > 0)
        //{
        //    brakeInput = Mathf.Abs(gasInput);
        //}
        //else if (movingDirection > 0.5f && gasInput < 0)
        //{
        //    brakeInput = Mathf.Abs(gasInput);
        //}
        //else if (!gasPedal.isPressed && !brakePedal.isPressed && !rightButton.isPressed && !leftButton.isPressed)
        //{
        //    brakeInput = 0.1f;
        //}
        //else
        //{
        //    brakeInput = 0;
        //}

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector2 moveDirection = new Vector2(horizontal, vertical);
        Debug.Log(moveDirection);

        md = new MoveData()
        {
            MoveDirection = moveDirection,
            IsBraking = false,
        };
    }

    private void TimeManager_OnTick()
    {
        if (base.IsOwner)
        {
            ///Reconciliation(default, false);
            CheckInput(out MoveData md);
            Move(md, false);
            _clientMoveData = md;
        }
        if (base.IsServer)
        {
            Move(default, true);
        }

    }


    [Replicate]
    private void Move(MoveData md, bool asServer, Channel channel = Channel.Unreliable, bool replaying = false)
    {
        if (asServer || replaying)
        {
            _clientMoveData = md;
        }

        
    }

    [Reconcile]
    private void Reconciliation(ReconcileData rd, bool asServer, Channel channel = Channel.Unreliable)
    {
        //transform.position = rd.Position;
        //transform.rotation = rd.Rotation;
        //player.velocity = rd.Velocity;
        //player.angularVelocity = rd.AngularVelocity;
        //transform.rotation = SmoothDampQuaternion(transform.rotation, rd.Rotation, ref _smoothingRotationVelocity, SmoothingDuration);
    }

    private void ProcessMove(MoveData md)
    {
        float verticalInput = AdjustInput(md.MoveDirection.y);
        float horizontalInput = AdjustInput(md.MoveDirection.x);

        float motor = _maxMotorTorque * verticalInput;
        float steering = _maxSteeringAngle * horizontalInput;

        UpdateAxles(motor, steering, md.IsBraking);
        UpdateBanking(horizontalInput);

        _carVelocity = transform.InverseTransformDirection(_rb.velocity);

        HandleMovement(verticalInput, horizontalInput, md.IsBraking);
      
    }

    private void FixedUpdate()
    {
        if(IsOwner)
        ProcessMove(_clientMoveData);
    }

    float AdjustInput(float input)
    {
        return input switch
        {
            >= .7f => 1f,
            <= -.7f => -1f,
            _ => input
        };
    }

    private void HandleMovement(float verticalInput, float horizontalInput, bool isBraking)
    {
        // Turn logic
        if (Mathf.Abs(verticalInput) > 0.1f || Mathf.Abs(_carVelocity.z) > 1)
        {
            float turnMultiplier = Mathf.Clamp01(_turnCurve.Evaluate(_carVelocity.magnitude / _maxSpeed));
            _rb.AddTorque(Vector3.up * (horizontalInput * Mathf.Sign(_carVelocity.z) * _turnStrength * turnMultiplier));
        }

        // Acceleration Logic
        if (!isBraking)
        {
            float targetSpeed = verticalInput * _maxSpeed;
            Vector3 forwardWithoutY = transform.forward.With(y: 0).normalized;
            _rb.velocity = Vector3.Lerp(_rb.velocity, forwardWithoutY * targetSpeed, Time.deltaTime);
        }

        // Downforce - always push the cart down, using lateral Gs to scale the force if the Kart is moving sideways fast
        float speedFactor = Mathf.Clamp01(_rb.velocity.magnitude / _maxSpeed);
        float lateralG = Mathf.Abs(Vector3.Dot(_rb.velocity, transform.right));
        float downForceFactor = Mathf.Max(speedFactor, lateralG / _lateralGScale);
        _rb.AddForce(-transform.up * (_downForce * _rb.mass * downForceFactor));

        // Shift Center of Mass
        float speed = _rb.velocity.magnitude;
        Vector3 centerOfMassAdjustment = (speed > _thresholdSpeed)
            ? new Vector3(0f, 0f, Mathf.Abs(verticalInput) > 0.1f ? Mathf.Sign(verticalInput) * _centerOfMassOffset : 0f)
            : Vector3.zero;
        _rb.centerOfMass = _originalCenterOfMass + centerOfMassAdjustment;

    }

    private void UpdateAxles(float motor, float steering, bool isBraking)
    {
        foreach (AxleInfo axleInfo in _axleInfos)
        {
            HandleSteering(axleInfo, steering);
            HandleMotor(axleInfo, motor);
            HandleBrakeAndDrift(axleInfo, isBraking);
            UpdateWheelVisuals(axleInfo.leftWheel);
            UpdateWheelVisuals(axleInfo.rightWheel);
        }
    }

    private void UpdateBanking(float horizontalInput)
    {
        float targetBankAngle = horizontalInput * -_maxBankAngle;
        Vector3 currentEuler = transform.localEulerAngles;
        currentEuler.z = Mathf.LerpAngle(currentEuler.z, targetBankAngle, Time.deltaTime * _bankSpeed);
        transform.localEulerAngles = currentEuler;
    }
    private void HandleSteering(AxleInfo axleInfo, float steering)
    {
        if (axleInfo.steering)
        {
            axleInfo.leftWheel.steerAngle = steering;
            axleInfo.rightWheel.steerAngle = steering;
        }
    }

    private void HandleMotor(AxleInfo axleInfo, float motor)
    {
        if (axleInfo.motor)
        {
            axleInfo.leftWheel.motorTorque = motor;
            axleInfo.rightWheel.motorTorque = motor;
        }
    }

    private void HandleBrakeAndDrift(AxleInfo axleInfo, bool isBraking)
    {
        if (axleInfo.motor)
        {
            if (isBraking)
            {
                _rb.constraints = RigidbodyConstraints.FreezeRotationX;

                float newZ = Mathf.SmoothDamp(_rb.velocity.z, 0, ref _brakeVelocity, 1f);
                _rb.velocity = _rb.velocity.With(z: newZ);

                axleInfo.leftWheel.brakeTorque = _brakeTorque;
                axleInfo.rightWheel.brakeTorque = _brakeTorque;
                ApplyDriftFriction(axleInfo.leftWheel,isBraking);
                ApplyDriftFriction(axleInfo.rightWheel, isBraking);
            }
            else
            {
                _rb.constraints = RigidbodyConstraints.None;

                axleInfo.leftWheel.brakeTorque = 0;
                axleInfo.rightWheel.brakeTorque = 0;
                ResetDriftFriction(axleInfo.leftWheel);
                ResetDriftFriction(axleInfo.rightWheel);
            }
        }
    }
    void ResetDriftFriction(WheelCollider wheel)
    {
        AxleInfo axleInfo = _axleInfos.FirstOrDefault(axle => axle.leftWheel == wheel || axle.rightWheel == wheel);
        if (axleInfo == null) return;

        wheel.forwardFriction = axleInfo.originalForwardFriction;
        wheel.sidewaysFriction = axleInfo.originalSidewayFriction;
    }

    void ApplyDriftFriction(WheelCollider wheel, bool isBraking)
    {
        if (wheel.GetGroundHit(out var hit))
        {
            wheel.forwardFriction = UpdateFriction(wheel.forwardFriction, isBraking);
            wheel.sidewaysFriction = UpdateFriction(wheel.sidewaysFriction, isBraking);
            IsGrounded = true;
        }
    }

    WheelFrictionCurve UpdateFriction(WheelFrictionCurve friction, bool isBraking)
    {
        friction.stiffness = isBraking ? Mathf.SmoothDamp(friction.stiffness, .5f, ref _driftVelocity, Time.deltaTime * 2f) : 1f;
        return friction;
    }

    private void UpdateWheelVisuals(WheelCollider wheelCollider)
    {
        if (wheelCollider.transform.childCount == 0) return;
        Transform visualWheel = wheelCollider.transform.GetChild(0);
        Vector3 position;
        Quaternion rotation;

        wheelCollider.GetWorldPose(out position, out rotation);
        visualWheel.position = position;
        visualWheel.rotation = rotation;
    }











}
