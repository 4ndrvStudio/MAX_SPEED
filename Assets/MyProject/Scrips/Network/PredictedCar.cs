using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using FishNet.Object.Prediction;
using FishNet.Transporting;
using FishNet.Managing.Timing;

using UnityEngine;
using FishNet;
using System;

[System.Serializable]
public struct MoveData : IReplicateData
{
    public float GasInput;
    public float BrakeInput;
    public float SteeringInput;
    public float SlipAngle;

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

    public ReconcileData(Vector3 position, Quaternion rotation)
    {
        Position = position;
        Rotation = rotation;
        _tick = 0;
    }

    private uint _tick;
    public void Dispose() { }
    public uint GetTick() => _tick;
    public void SetTick(uint value) => _tick = value;

}

public class PredictedCar : NetworkBehaviour
{

    private Rigidbody player;
    private CarController controller;

    private float gasInput;
    private float brakeInput;
    private float steeringInput;
    private float slipAngle;

    public Transform _carVisualRootObject;

    private Vector3 _previousPosition;
    private Quaternion _previousRotation;

    private Vector3 _instantiatedLocalPosition;
    private Quaternion _instantiatedLocalRotation;

    [Range(0.01f, 0.5f)]
    public float SmoothingDuration = 0.05f;
    private Vector3 _smoothingPositionVelocity = Vector3.zero;
    private float _smoothingRotationVelocity;


    [SerializeField] private MoveData _clientMoveData;
    [SerializeField] private ReconcileData _clientReconcileData;

    private bool _subscribed = false;



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
            base.TimeManager.OnPreTick += TimeManager_OnPreTick;
            base.TimeManager.OnPostTick += TimeManager_OnPostTick;
            base.PredictionManager.OnPreReconcile += TimeManager_OnPreReconcile;
            base.PredictionManager.OnPostReconcile += TimeManager_OnPostReconcile;
            //base.TimeManager.OnPreReplicateReplay += TimeManager_OnPreReplicateReplay;
        }
        else
        {
            base.TimeManager.OnTick -= TimeManager_OnTick;
            base.TimeManager.OnPreTick -= TimeManager_OnPreTick;
            base.TimeManager.OnPostTick -= TimeManager_OnPostTick;
            base.PredictionManager.OnPreReconcile -= TimeManager_OnPreReconcile;
            base.PredictionManager.OnPostReconcile -= TimeManager_OnPostReconcile;
            //base.TimeManager.OnPreReplicateReplay -= TimeManager_OnPreReplicateReplay;
        }
    }

    private void Start()
    {
        ChangeSubscriptions(true);
    }


    public override void OnStartClient()
    {
        base.OnStartClient();

        if (base.IsOwner)
        {
            //_mainCamera = CameraManager.Instance.MainCamera.gameObject;
        }
    }

    public override void OnStartNetwork()
    {
        base.OnStartNetwork();
        _instantiatedLocalPosition = _carVisualRootObject.localPosition;
        _instantiatedLocalRotation = _carVisualRootObject.localRotation;
    }

    public override void OnStopNetwork()
    {
        base.OnStopNetwork();
        ChangeSubscriptions(false);
    }

    private void Awake()
    {
        player = GetComponent<Rigidbody>();
        controller = GetComponent<CarController>();
        SetPreviousTransformProperties();
    }

    private void OnDestroy()
    {
        ChangeSubscriptions(false);
    }

    private void SetPreviousTransformProperties()
    {
        //_previousPosition = _carVisualRootObject.position;
        //_previousRotation = _carVisualRootObject.rotation;
    }

    private void ResetToTransformPreviousProperties()
    {
        //_carVisualRootObject.position = _previousPosition;
        //_carVisualRootObject.rotation = _previousRotation;
    }

    private void CheckInput(out MoveData md)
    {
        md = default;

        CarMyByButton gasPedal = CarUI.Instance.gasPedal;
        CarMyByButton brakePedal = CarUI.Instance.brakePedal;
        CarMyByButton rightButton = CarUI.Instance.rightButton;
        CarMyByButton leftButton = CarUI.Instance.leftButton;

        gasInput = Input.GetAxis("Vertical");

        if (gasPedal.isPressed)
        {
            gasInput += gasPedal.dampenPress;
        }
        if (brakePedal.isPressed)
        {
            gasInput -= brakePedal.dampenPress;
        }

        steeringInput = Input.GetAxis("Horizontal");

        if (rightButton.isPressed)
        {
            steeringInput += rightButton.dampenPress;
        }
        if (leftButton.isPressed)
        {
            steeringInput -= leftButton.dampenPress;
        }
        slipAngle = Vector3.Angle(transform.forward, player.velocity - transform.forward);

        float movingDirection = Vector3.Dot(transform.forward, player.velocity);

        brakeInput = 0;

        if (movingDirection < -0.5f && gasInput > 0)
        {
            brakeInput = Mathf.Abs(gasInput);
        }
        else if (movingDirection > 0.5f && gasInput < 0)
        {
            brakeInput = Mathf.Abs(gasInput);
        }
        else if (!gasPedal.isPressed && !brakePedal.isPressed && !rightButton.isPressed && !leftButton.isPressed)
        {
            brakeInput = 0.1f;
        }
        else
        {
            brakeInput = 0;
        }

        md = new MoveData()
        {
            BrakeInput = brakeInput,
            GasInput = gasInput,
            SteeringInput = steeringInput,
            SlipAngle = slipAngle,
        };
    }

    private void TimeManager_OnTick()
    {
        if (base.IsOwner)
        {
           Reconciliation(default, false);
           CheckInput(out MoveData md);
           Move(md, false);
        }
        if (base.IsServer)
        {
           Move(default, true);
        }

        if (!IsServer && !IsOwner)
        {
           controller.Simulate(_clientMoveData, (float)TimeManager.TickDelta, true);
        }

    }
    private void TimeManager_OnPostTick()
    {
        if (IsServer)
        {
            ReconcileData data = new ReconcileData()
            {
                Position = transform.position,
                Rotation = transform.rotation,
            };

            ObserversMoveData(_clientMoveData, data);
            Reconciliation(data, true);
        }

        ResetToTransformPreviousProperties();

    }
    private void TimeManager_OnPreReconcile(NetworkBehaviour obj)
    {
       // SetPreviousTransformProperties();
    }
    private void TimeManager_OnPostReconcile(NetworkBehaviour obj)
    {
       // _carVisualRootObject.SetPositionAndRotation(_previousPosition, _previousRotation);
    }
    private void TimeManager_OnPreTick()
    {
        //SetPreviousTransformProperties();
    }

    private void Update()
    {
        if(!IsServer && !IsOwner)
        {
            MoveToTarget();
        }
     
    }

    [Replicate]
    private void Move(MoveData md, bool asServer, Channel channel = Channel.Unreliable, bool replaying = false)
    {
        if (asServer || replaying)
        {
            _clientMoveData = md;
        }
        
        controller.Simulate(md, (float)TimeManager.TickDelta, false);
    
    }

    [Reconcile]
    private void Reconciliation(ReconcileData rd, bool asServer, Channel channel = Channel.Unreliable)
    {
        transform.position = rd.Position;
        transform.rotation = rd.Rotation;
        //transform.rotation = SmoothDampQuaternion(transform.rotation, rd.Rotation, ref _smoothingRotationVelocity, SmoothingDuration);
    }

    public static Quaternion SmoothDampQuaternion(Quaternion current, Quaternion target, ref float AngularVelocity, float smoothTime)
    {
        var delta = Quaternion.Angle(current, target);

        if (delta > 0.0f)
        {
            var t = Mathf.SmoothDampAngle(delta, 0.0f, ref AngularVelocity, smoothTime);
            t = 1.0f - t / delta;
            return Quaternion.Slerp(current, target, t);
        }

        return current;
    }

    private void MoveToTarget()
    {
       transform.position = Vector3.SmoothDamp(transform.position, _clientReconcileData.Position, ref _smoothingPositionVelocity, SmoothingDuration);
       transform.rotation = SmoothDampQuaternion(transform.rotation, _clientReconcileData.Rotation, ref _smoothingRotationVelocity, SmoothingDuration);
        //Transform t = _carVisualRootObject.transform;
        // t.localPosition = Vector3.SmoothDamp(t.localPosition, _instantiatedLocalPosition, ref _smoothingPositionVelocity, SmoothingDuration);
        // t.localRotation = SmoothDampQuaternion(t.localRotation, _instantiatedLocalRotation, ref _smoothingRotationVelocity, SmoothingDuration);
    }


    [ObserversRpc(ExcludeOwner = true, BufferLast = true)]
    private void ObserversMoveData(MoveData lastMove, ReconcileData rd, Channel channel = Channel.Unreliable)
    {
        if (!base.IsServer && !base.IsOwner)
        {
            _clientMoveData = lastMove;
            _clientReconcileData = rd;
        }
    }


}
