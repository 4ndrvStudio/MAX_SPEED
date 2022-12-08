using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAi : Car
{
    public Record carAi;
    public Transform[] path;
    public CheckPoint[] checkPoints;
    public WheelColliders colliders;
    public WheelMeshes wheelMeshes;
    public float motorPower;
    public float speed;
    public Transform lookingPonit;
    public float avoidanceDistance;
    //public float smoothFactor;
    public float reachingRadius;

    private int destination = 0;
    private Vector3 desiredDirection;
    private void Start()
    {
        carAi.lap = 1;
        //speed = carAi.velocity.magnitude;
        //transform.gameObject.AddComponent<CarController>();
    }
    private void Update()
    {
        Position();
        if (IsBlocked(desiredDirection))
        {
            AvoidObstacle();
            StepForward();
        }
        StepForward();
        ApplyWheelPosition();
        checkLabForCarAI();
    }

    private void Position()
    {
        desiredDirection = path[destination].position - transform.position;
        if (desiredDirection.magnitude <= reachingRadius)
        {
            destination++;
            if (destination == path.Length)
            {
                destination = 0;
            }
        }
    }
    private void AvoidObstacle()
    {
        Vector3 lookingDirection;
        for (int i = 0; i < 36; i++)
        {
            lookingDirection = Quaternion.Euler(0, i * 10f, 0) * desiredDirection;
            if (!IsBlocked(lookingDirection))
            {
                desiredDirection = lookingDirection;
                break;
            }
        }
    }
    private void StepForward()
    {
        //UpdateRotation();
        ApplySteering();
        colliders.RRWheel.motorTorque = motorPower;
        colliders.RLWheel.motorTorque = motorPower;
        //transform.Translate(0, 0, speed * Time.deltaTime);
    }

    private bool IsBlocked(Vector3 lookingDirection)
    {
        Ray lookingRay = new Ray(lookingPonit.position, lookingDirection);
        return Physics.SphereCast(lookingRay, 0.5f, avoidanceDistance);
    }

    //private void UpdateRotation()
    //{
    //    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(desiredDirection), 2f * Time.deltaTime);
    //}

    float steeringAngle;

    void ApplySteering() 
    {
        Vector3 flatDestination = path[destination].position;
        flatDestination.y = transform.position.y;
        steeringAngle = Vector3.SignedAngle(transform.forward, flatDestination - transform.position, Vector3.up);
        steeringAngle = Mathf.Clamp(steeringAngle, -45, 45);
        print($"Sterring: {steeringAngle}");
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
    void UpdateWheel(WheelCollider coll, MeshRenderer wheelMesh)
    {
        Quaternion quat;
        Vector3 position;
        coll.GetWorldPose(out position, out quat);
        wheelMesh.transform.position = position;
        wheelMesh.transform.rotation = quat;
    }

    private void checkLabForCarAI()
    {
        if (carAi.checkPointId == checkPoints.Length)
        {
            carAi.lap++;
            carAi.checkPointId = 0;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "CheckPoint")
        {
            carAi.checkPointId++;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, path[destination].position);
    }
}
