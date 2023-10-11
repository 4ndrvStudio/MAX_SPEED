using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using FishNet.Object.Prediction;
using FishNet.Transporting;
using UnityEngine;
using FishNet;
using static FishNet.Component.Transforming.NetworkTransform;

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

    [SerializeField] private MoveData _clientMoveData;
    [SerializeField] private ReconcileData _clientReconcileData;

    private void Awake()
    {
        InstanceFinder.TimeManager.OnTick += TimeManager_OnTick;
        InstanceFinder.TimeManager.OnPostTick += TimeManager_OnPostTick;


        player = GetComponent<Rigidbody>();
        controller = GetComponent<CarController>();

    }

    private void OnDestroy()
    {
        if (InstanceFinder.TimeManager != null)
        {
            InstanceFinder.TimeManager.OnTick -= TimeManager_OnTick;
            InstanceFinder.TimeManager.OnPostTick -= TimeManager_OnPostTick;

        }
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
        AssignAnimationIDs();

    }
    private void AssignAnimationIDs()
    {
        //_animIDSpeed = Animator.StringToHash("movement");
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
           Debug.Log(md.GasInput);
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
        }

    }
    


    private void Update()
    {
        if (!IsServer && !IsOwner)
        {
            transform.position = _clientReconcileData.Position;
            transform.rotation = _clientReconcileData.Rotation;
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
