using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : Car
{
    private Rigidbody player;
    //public CarWheelSkid WheelSkid;
    public WheelColliders colliders;
    public WheelMeshes wheelMeshes;
    public WheelParticles wheelParticles;
    public float gasInput;
    public float brakeInput;
    public float steeringInput;
    public GameObject smokePrefab;
    public GameObject skidPrefab;
    public float motorPower;
    public float brakePower;
    public float slipAngle;
    private float speed;
    public AnimationCurve steeringCurve;

    public CarMyByButton gasPedal;
    public CarMyByButton brakePedal;
    public CarMyByButton leftButton;
    public CarMyByButton rightButton;
    void Start()
    {
        player = gameObject.GetComponent<Rigidbody>();
        InstantiateSmokeAndSkid();
    }
    void InstantiateSmokeAndSkid()
    {
        wheelParticles.FRWheel = Instantiate(smokePrefab, colliders.FRWheel.transform.position - Vector3.up * colliders.FRWheel.radius, Quaternion.identity, colliders.FRWheel.transform)
            .GetComponent<ParticleSystem>();
        wheelParticles.FLWheel = Instantiate(smokePrefab, colliders.FLWheel.transform.position - Vector3.up * colliders.FRWheel.radius, Quaternion.identity, colliders.FLWheel.transform)
            .GetComponent<ParticleSystem>();
        wheelParticles.RRWheel = Instantiate(smokePrefab, colliders.RRWheel.transform.position - Vector3.up * colliders.FRWheel.radius, Quaternion.identity, colliders.RRWheel.transform)
            .GetComponent<ParticleSystem>();
        wheelParticles.RLWheel = Instantiate(smokePrefab, colliders.RLWheel.transform.position - Vector3.up * colliders.FRWheel.radius, Quaternion.identity, colliders.RLWheel.transform)
            .GetComponent<ParticleSystem>();

        wheelParticles.FRSkid = Instantiate(skidPrefab, colliders.FRWheel.transform.position - Vector3.up * colliders.FRWheel.radius, Quaternion.identity, colliders.FRWheel.transform)
            .GetComponentInChildren<TrailRenderer>();
        wheelParticles.FLSkid = Instantiate(skidPrefab, colliders.FLWheel.transform.position - Vector3.up * colliders.FRWheel.radius, Quaternion.identity, colliders.FLWheel.transform)
            .GetComponentInChildren<TrailRenderer>();
        wheelParticles.RRSkid = Instantiate(skidPrefab, colliders.RRWheel.transform.position - Vector3.up * colliders.FRWheel.radius, Quaternion.identity, colliders.RRWheel.transform)
            .GetComponentInChildren<TrailRenderer>();
        wheelParticles.RLSkid = Instantiate(skidPrefab, colliders.RLWheel.transform.position - Vector3.up * colliders.FRWheel.radius, Quaternion.identity, colliders.RLWheel.transform).
            GetComponentInChildren<TrailRenderer>();
    }
    void FixedUpdate()
    {
        speed = player.velocity.magnitude;
        CheckInput();
        ApplySteering();
        ApplyMotor();
        ApplyBrake();
        CheckParticles();
        ApplyWheelPosition();
        
    }
    void CheckInput()
    {
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
        if (movingDirection < -0.5f && gasInput > 0)
        {
            brakeInput = Mathf.Abs(gasInput);
        }
        else if (movingDirection > 0.5f && gasInput < 0)
        {
            brakeInput = Mathf.Abs(gasInput);
        }
        //else if (!gasPedal.isPressed && !brakePedal.isPressed && !rightButton.isPressed && !leftButton.isPressed)
        //{
        //    brakeInput = 0.1f;
        //}
        //else
        //{
        //    brakeInput = 0;
        //}
    }
    void ApplyBrake()
    {
        colliders.FRWheel.brakeTorque = brakePower * brakeInput * 0.7f;
        colliders.FLWheel.brakeTorque = brakePower * brakeInput * 0.7f;
        colliders.RRWheel.brakeTorque = brakePower * brakeInput * 0.3f;
        colliders.RLWheel.brakeTorque = brakePower * brakeInput * 0.3f;
    }
    void ApplyMotor()
    {
        colliders.RRWheel.motorTorque = motorPower * gasInput;
        colliders.RLWheel.motorTorque = motorPower * gasInput;
    }
    void ApplySteering()
    {
        float steeringAngle = steeringInput * steeringCurve.Evaluate(speed);
        if (slipAngle < 120f)
        {
            steeringAngle += Vector3.SignedAngle(transform.forward, player.velocity + transform.forward, Vector3.up);
        }
        steeringAngle = Mathf.Clamp(steeringAngle, -90f, 90f);
        colliders.FRWheel.steerAngle = steeringAngle;
        colliders.FLWheel.steerAngle = steeringAngle;
    }
    void ApplyWheelPosition()
    {
        UpdateWheel(colliders.FRWheel, wheelMeshes.FRWheel);
        UpdateWheel(colliders.FLWheel, wheelMeshes.FLWheel);
        UpdateWheel(colliders.RRWheel, wheelMeshes.RRWheel);
        UpdateWheel(colliders.RLWheel, wheelMeshes.RLWheel);
    }
    void CheckParticles()
    {
        WheelHit[] wheelHits = new WheelHit[4];
        colliders.FRWheel.GetGroundHit(out wheelHits[0]);
        colliders.FLWheel.GetGroundHit(out wheelHits[1]);

        colliders.RRWheel.GetGroundHit(out wheelHits[2]);
        colliders.RLWheel.GetGroundHit(out wheelHits[3]);

        float slipAllowance = 0.5f;
        if ((Mathf.Abs(wheelHits[0].sidewaysSlip) + Mathf.Abs(wheelHits[0].forwardSlip) > slipAllowance))
        {
            wheelParticles.FRWheel.Play();
            wheelParticles.FRSkid.emitting = true;
        }
        else
        {
            wheelParticles.FRWheel.Stop();
            wheelParticles.FRSkid.emitting = false;
        }
        if ((Mathf.Abs(wheelHits[1].sidewaysSlip) + Mathf.Abs(wheelHits[1].forwardSlip) > slipAllowance))
        {
            wheelParticles.FLWheel.Play();
            wheelParticles.FLSkid.emitting = true;

        }
        else
        {
            wheelParticles.FLWheel.Stop();
            wheelParticles.FLSkid.emitting = false;
        }
        if ((Mathf.Abs(wheelHits[2].sidewaysSlip) + Mathf.Abs(wheelHits[2].forwardSlip) > slipAllowance))
        {
            wheelParticles.RRWheel.Play();
            wheelParticles.RRSkid.emitting = true;
        }
        else
        {
            wheelParticles.RRWheel.Stop();
            wheelParticles.RRSkid.emitting = false;
        }
        if ((Mathf.Abs(wheelHits[3].sidewaysSlip) + Mathf.Abs(wheelHits[3].forwardSlip) > slipAllowance))
        {
            wheelParticles.RLWheel.Play();
            wheelParticles.RLSkid.emitting = true;
        }
        else
        {
            wheelParticles.RLWheel.Stop();
            wheelParticles.RLSkid.emitting = false;
        }
    }
    void UpdateWheel(WheelCollider coll, MeshRenderer wheelMesh)
    {
        Quaternion quat;
        Vector3 position;
        coll.GetWorldPose(out position, out quat);
        wheelMesh.transform.position = position;
        wheelMesh.transform.rotation = quat;
    }
}
[System.Serializable]
public class WheelColliders
{
    public WheelCollider FRWheel;
    public WheelCollider FLWheel;
    public WheelCollider RRWheel;
    public WheelCollider RLWheel;
}
[System.Serializable]
public class WheelMeshes
{
    public MeshRenderer FRWheel;
    public MeshRenderer FLWheel;
    public MeshRenderer RRWheel;
    public MeshRenderer RLWheel;
}

[System.Serializable]
public class WheelParticles
{
    public ParticleSystem FRWheel;
    public ParticleSystem FLWheel;
    public ParticleSystem RRWheel;
    public ParticleSystem RLWheel;
    public TrailRenderer FRSkid;
    public TrailRenderer FLSkid;
    public TrailRenderer RRSkid;
    public TrailRenderer RLSkid;

}