﻿namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "CarSettings", menuName = "TurnTheGameOn/Arcade Racer/CarSettings")]
    public class CarSettings : ScriptableObject
    {
        public CarDriveType carDriveType = CarDriveType.FourWheelDrive;
        public SpeedType speedType = SpeedType.MPH;
        public bool manual;
        public Vector3 centerOfMass;
        public float maxSteerAngle = 25.0f;
        [Range(0, 3)] public float steerSensitivity = 0.15f;
        [Range(0, 1)] public float steerHelper = 0.775f; // 0 is raw physics , 1 the car will grip in the direction it is facing
        [Range(0, 1)] public float tractionControl = 1.0f; // 0 is no traction control, 1 is full interference
        public float fullTorqueOverAllWheels = 2000.0f;
        public float reverseTorque = 1500.0f;
        public bool enableDownForce;
        public float downforce = 100f;
        public float topSpeed = 140.0f;
        public float topSpeedReverse = 45.0f;
        public int noOfGears = 5;
        public float brakeTorque = 1500.0f;
        public float minRPM = 0.2f;
        public float maxRPM = 1.0f;
        public float RPMLimit = 8.0f;
        public AnimationCurve torqueCurve = AnimationCurve.EaseInOut(0, 0.85f, 1, 0.65f);
        public AnimationCurve gearSpeedLimitCurve = AnimationCurve.Linear(0, 0, 1, 1);
        public bool enableDragOverride;
        public float maxDrag, maxAngularDrag;
        public float minDrag, minAngularDrag;
    }
}