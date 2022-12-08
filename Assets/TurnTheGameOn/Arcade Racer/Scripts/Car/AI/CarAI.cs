namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;

    public class CarAI : MonoBehaviour
    {
        public CarAISettings m_AISettings;
        public CarAIWaypointRoute waypointRoute;
        private CarDriveSystem driveSystem;
        public GameObject sensors;
        public Transform driveTarget;
        public bool startDrivingOnStart;
        public float startDrivingTime;
        public int startingRoutePointIndex;
        public CarAISensorInfo[] detectionData;
        private CarAIWaypoint nextWaypoint;
        public bool isDriving { get; private set; }
        private bool canTurnLeft, canTurnRight, canChangeLanes;
        public float cautionAmount;
        public float desiredSpeed;
        public float routeDistanceTraveled;
        private float changeLaneCooldownTimer;
        public float waypointTimer;
        public float stuckTimer;
        private Vector3 relativePoint;
        public float steer;
        public Vector3 offsetTargetPos;
        public float targetAngle;
        public Vector3 localTarget;
        public float accelBrakeSensitivity;
        public float accel;
        private bool canStop;
        public int currentRoutePointIndex;
        public bool useSpeedLimit;
        public float speedLimit;
        public bool useBrakeTrigger;
        public bool releaseBrakeWhenStopped;
        public float brakeAmount;
        bool findDesiredRoute;
        int desiredRouteIndex;
        public float minCaution;
        public bool customResetPoint;
        public CarAIWaypointRoute stuckResetRoute;
        public int stuckResetPoint;
        public Transform transformCached;
        public Rigidbody rigidbodyCached;

        private int PossibleTargetDirection(Transform _transform)
        {
            relativePoint = transform.InverseTransformPoint(_transform.position);
            if (relativePoint.x < 0.0) return -1;
            else if (relativePoint.x > 0.0) return 1;
            else return 0;
        }

        public virtual void Awake()
        {
            transformCached = transform;
            rigidbodyCached = GetComponent<Rigidbody>();
            driveSystem = GetComponent<CarDriveSystem>();
            currentRoutePointIndex = startingRoutePointIndex;
            for (int i = 0; i < detectionData.Length; i++)
            {
                detectionData[i].sensor.updateInterval = m_AISettings.updateInterval;
                detectionData[i].sensor.layerMask = m_AISettings.detectionLayers;
            }
        }

        void Start()
        {
            if (startDrivingOnStart) Invoke("StartDriving", startDrivingTime);
            UpdateSensors();
        }

        void FixedUpdate()
        {
            RouteProgress();
            CheckIfLost();
            RouteLogic();
            CheckAutoCautionDistanceThreshold();
            Move();
        }

        void RouteLogic()
        {
            if (m_AISettings.enableSensors && isDriving)
            {
                if (findDesiredRoute)
                {
                    FindDesiredRoute();
                }
                else
                {
                    ChangeLaneIfObstacleIsDetected();
                }
            }
        }

        void RouteProgress()
        {
            if (currentRoutePointIndex < waypointRoute.waypointDataList.Count)
            {
                if (m_AISettings.useWaypointDistanceThreshold)
                {
                    if (Vector3.Distance(transformCached.position, waypointRoute.waypointDataList[currentRoutePointIndex]._transform.position) <= m_AISettings.waypointDistanceThreshold)
                    {
                        waypointRoute.waypointDataList[currentRoutePointIndex]._waypoint.TriggerNextWaypoint(this);
                    }
                }
                driveTarget.position = waypointRoute.waypointDataList[currentRoutePointIndex]._transform.position;
            }
        }

        void ChangeLaneCooldownTimer()
        {
            changeLaneCooldownTimer += 1.0f * Time.deltaTime;
            if (changeLaneCooldownTimer > m_AISettings.changeLaneCooldown)
            {
                canChangeLanes = true;
                changeLaneCooldownTimer = 0.0f;
            }
        }

        void CheckIfCanChangeLanes()
        {
            if (!canChangeLanes) ChangeLaneCooldownTimer();
            for (int i = 0; i < detectionData.Length; i++)
            {
                detectionData[i].hit = detectionData[i].sensor.hit;
                detectionData[i].distance = detectionData[i].sensor.hitDistance;
                detectionData[i].tag = detectionData[i].sensor.hitTag;
                detectionData[i].layer = detectionData[i].sensor.hitLayer;
            }
            canTurnLeft = (detectionData[9].hit == true || detectionData[0].hit == true || detectionData[1].hit == true) ? false : true;
            canTurnRight = (detectionData[10].hit == true || detectionData[7].hit == true || detectionData[8].hit == true) ? false : true;
        }

        void FindDesiredRoute()
        {
            for (int i = 0; i < waypointRoute.waypointDataList.Count; i++) // get next waypoint to determine what the connecting junctions are
            {
                if (routeDistanceTraveled <= waypointRoute.waypointDataList[i]._waypoint.onReachWaypointSettings.distanceFromStart)
                {
                    nextWaypoint = waypointRoute.waypointDataList[i]._waypoint;
                    break;
                }
            }
            if (desiredRouteIndex == waypointRoute.routeIndex)
            {
                findDesiredRoute = false;
            }
            if (desiredRouteIndex > waypointRoute.routeIndex) // need to go right 
            {
                for (int i = 0; i < nextWaypoint.junctionPoint.Length; i++)
                {
                    if (nextWaypoint.junctionPoint[i].onReachWaypointSettings.parentRoute.routeIndex > waypointRoute.routeIndex && canTurnRight)
                    {
                        waypointRoute = nextWaypoint.junctionPoint[i].onReachWaypointSettings.parentRoute;
                        routeDistanceTraveled = nextWaypoint.onReachWaypointSettings.distanceFromStart;
                        currentRoutePointIndex = nextWaypoint.onReachWaypointSettings.waypointIndexnumber;
                        canChangeLanes = false;
                        findDesiredRoute = false;
                        break;
                    }
                }
            }
            else if (desiredRouteIndex < waypointRoute.routeIndex) // ned to go left
            {
                for (int i = 0; i < nextWaypoint.junctionPoint.Length; i++)
                {
                    if (nextWaypoint.junctionPoint[i].onReachWaypointSettings.parentRoute.routeIndex < waypointRoute.routeIndex && canTurnLeft)
                    {
                        waypointRoute = nextWaypoint.junctionPoint[i].onReachWaypointSettings.parentRoute;
                        routeDistanceTraveled = nextWaypoint.onReachWaypointSettings.distanceFromStart;
                        currentRoutePointIndex = nextWaypoint.onReachWaypointSettings.waypointIndexnumber;
                        canChangeLanes = false;
                        findDesiredRoute = false;
                        break;
                    }
                }
            }
            if (desiredRouteIndex == waypointRoute.routeIndex)
            {
                findDesiredRoute = false;
            }
        }

        void ChangeLaneIfObstacleIsDetected()
        {
            CheckIfCanChangeLanes();
            if (detectionData[3].hit == true && detectionData[4].hit == true && detectionData[5].hit == true)
            {
                if (detectionData[3].distance < m_AISettings.changeLaneDistance || detectionData[4].distance < m_AISettings.changeLaneDistance || detectionData[5].distance < m_AISettings.changeLaneDistance)
                {
                    ChangeLane();
                }
            }
        }

        public void ChangeLane()
        {
            if (canChangeLanes)
            {
                canChangeLanes = false;
                for (int i = 0; i < waypointRoute.waypointDataList.Count; i++) // get next waypoint to determine what the connecting junctions are
                {
                    if (routeDistanceTraveled <= waypointRoute.waypointDataList[i]._waypoint.onReachWaypointSettings.distanceFromStart)
                    {
                        nextWaypoint = waypointRoute.waypointDataList[i]._waypoint;
                        break;
                    }
                }
                if (nextWaypoint != null) // if there's a junction we'll use it as an alternate route
                {
                    if (nextWaypoint.junctionPoint.Length > 0)  // take the first alternate route - setup new route data
                    {
                        if (PossibleTargetDirection(nextWaypoint.junctionPoint[0].transform) == -1 && canTurnLeft) // Debug.Log (nextWaypoint.name); Debug.Log (nextWaypoint.junctionPoint [0].name);
                        {
                            waypointRoute = nextWaypoint.junctionPoint[0].onReachWaypointSettings.parentRoute;
                            routeDistanceTraveled = nextWaypoint.onReachWaypointSettings.distanceFromStart;
                            currentRoutePointIndex = nextWaypoint.onReachWaypointSettings.waypointIndexnumber;
                            canChangeLanes = false;
                        }
                        else if (PossibleTargetDirection(nextWaypoint.junctionPoint[0].transform) == 1 && canTurnRight) // Debug.Log (nextWaypoint.name); Debug.Log (nextWaypoint.junctionPoint [0].name);
                        {
                            waypointRoute = nextWaypoint.junctionPoint[0].onReachWaypointSettings.parentRoute;
                            routeDistanceTraveled = nextWaypoint.onReachWaypointSettings.distanceFromStart;
                            currentRoutePointIndex = nextWaypoint.onReachWaypointSettings.waypointIndexnumber;
                            canChangeLanes = false;
                        }
                    }
                }
            }
        }

        public virtual void CheckAutoCautionDistanceThreshold()
        {
            if (waypointRoute.stopForTrafficLight && routeDistanceTraveled > 0 && currentRoutePointIndex >= waypointRoute.waypointDataList.Count - 1)
            {
                if (cautionAmount != 1)
                {
                    minCaution = Mathf.Clamp(minCaution += m_AISettings.cautionSlowSpeed * Time.deltaTime, minCaution, 1.0f);
                    cautionAmount = minCaution;
                    driveSystem.overrideBrake = true;
                    driveSystem.overrideBrakePower = minCaution;
                    driveSystem.overrideAcceleration = true;
                    driveSystem.overrideAccelerationPower = 0;
                    rigidbodyCached.drag = 1.5f;
                    rigidbodyCached.angularDrag = 1;
                }
                else if (driveSystem.currentSpeed == 0)
                {
                    rigidbodyCached.drag = driveSystem._vehicleSettings.minDrag;
                    rigidbodyCached.angularDrag = driveSystem._vehicleSettings.minAngularDrag;
                }
            }
            else if ((
                detectionData[3].hit == true ||
                detectionData[4].hit == true ||
                detectionData[5].hit == true
                ) && (
                detectionData[3].distance < m_AISettings.autoCautionDistanceThreshold ||
                detectionData[4].distance < m_AISettings.autoCautionDistanceThreshold ||
                detectionData[5].distance < m_AISettings.autoCautionDistanceThreshold))
            {
                if (cautionAmount != 1)
                {
                    minCaution = Mathf.Clamp(minCaution += m_AISettings.cautionSlowSpeed * Time.deltaTime, minCaution, 1.0f);
                    cautionAmount = minCaution;
                    driveSystem.overrideBrake = true;
                    driveSystem.overrideBrakePower = minCaution;
                    driveSystem.overrideAcceleration = true;
                    driveSystem.overrideAccelerationPower = 0;
                    driveSystem._rigidbody.drag = 1.5f;
                    driveSystem._rigidbody.angularDrag = 1;
                }
                else if (driveSystem.currentSpeed == 0)
                {
                    driveSystem._rigidbody.drag = driveSystem._vehicleSettings.minDrag;
                    driveSystem._rigidbody.angularDrag = driveSystem._vehicleSettings.minAngularDrag;
                }
            }
            else //if (cautionAmount != minCaution)
            {
                minCaution = 0;
                cautionAmount = minCaution;
                driveSystem.overrideBrakePower = 0;
                driveSystem.overrideBrake = false;
                driveSystem.overrideAcceleration = false;
                driveSystem._rigidbody.drag = driveSystem._vehicleSettings.minDrag;
                driveSystem._rigidbody.angularDrag = driveSystem._vehicleSettings.minAngularDrag;
            }
        }

        public virtual void Move()
        {
            offsetTargetPos = driveTarget.position; // our target position starts off as the 'real' target position
            if (isDriving)
            {
                desiredSpeed = Mathf.Lerp(0.0f, driveSystem.topSpeed * (1 - cautionAmount), 1);
                offsetTargetPos += driveTarget.right * (Mathf.PerlinNoise(Time.time * 0.2f, 0.2f) * 2 - 1) * 0.1f; // no need for evasive action, we can just wander across the path-to-target in a random way, // // which can help prevent AI from seeming too uniform and robotic in their driving
                if (useSpeedLimit && desiredSpeed > speedLimit) desiredSpeed = speedLimit;
                accelBrakeSensitivity = (desiredSpeed < driveSystem.currentSpeed) ? m_AISettings.brakeSensitivity : m_AISettings.accelerationSensitivity; // use different sensitivity depending on whether accelerating or braking:                
                accel = Mathf.Clamp((desiredSpeed - driveSystem.currentSpeed) * accelBrakeSensitivity, -1, 1); // decide the actual amount of accel/brake input to achieve desired speed.
                localTarget = transform.InverseTransformPoint(offsetTargetPos); // calculate the local-relative position of the target, to steer towards                
                targetAngle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg; // work out the local angle towards the target
                steer = Mathf.Clamp(targetAngle * m_AISettings.steeringSensitivity, -1, 1) * Mathf.Sign(driveSystem.currentSpeed);  // get the amount of steering needed to aim the car towards the target                
                if (useBrakeTrigger)
                {
                    if (releaseBrakeWhenStopped && driveSystem.currentSpeed < 1)
                    {
                        brakeAmount = 0;
                    }
                    driveSystem.Move(steer, accel, brakeAmount, 0f); // feed input to the car controller.
                }
                else
                {
                    driveSystem.Move(steer, accel, accel, 0f); // feed input to the car controller.
                }
            }
            else
            {
                if (driveSystem.currentSpeed > 2)
                {
                    offsetTargetPos += driveTarget.right; // no need for evasive action, drive toward target //else offsetTargetPos += driveTarget.right* (Mathf.PerlinNoise(Time.time*m_LateralWanderSpeed, m_RandomPerlin)*2 - 1)* m_LateralWanderDistance; // no need for evasive action, we can just wander across the path-to-target in a random way, which can help prevent AI from seeming too uniform and robotic in their driving
                    localTarget = transform.InverseTransformPoint(offsetTargetPos); // calculate the local-relative position of the target, to steer towards                    
                    targetAngle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg; // work out the local angle towards the target                    
                    steer = Mathf.Clamp(targetAngle * m_AISettings.steeringSensitivity, -1, 1) * Mathf.Sign(driveSystem.currentSpeed); // get the amount of steering needed to aim the car towards the target                    
                    driveSystem.Move(steer, 0, -1f, 1f); // Car should not be moving, use handbrake to stop
                }
                else driveSystem.Move(0, 0, -1f, 1f); // Car should not be moving, use handbrake to stop 
            }
        }

        void OnDrawGizmos()
        {
            if (m_AISettings.showEditorGizmos)
            {
                if (Application.isPlaying)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawLine(transform.position, driveTarget.position);
                    //Gizmos.DrawWireSphere(waypointRoute.GetRoutePosition(routeDistanceTraveled), 1);
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawLine(driveTarget.position, driveTarget.position + driveTarget.forward);
                }
            }
        }

        public void OnReachedWaypoint(CarAIOnReachWaypointInfo onReachWaypointSettings)
        {
            if (onReachWaypointSettings.parentRoute == waypointRoute)
            {
                ProcessReachedWaypoint(onReachWaypointSettings);
            }
            else if (m_AISettings.useAnyWaypoint && onReachWaypointSettings.parentRoute != null)
            {
                waypointRoute = onReachWaypointSettings.parentRoute;
                routeDistanceTraveled = onReachWaypointSettings.distanceFromStart;
                currentRoutePointIndex = 0;
                ProcessReachedWaypoint(onReachWaypointSettings);
            }
        }

        void ProcessReachedWaypoint(CarAIOnReachWaypointInfo onReachWaypointSettings)
        {
            onReachWaypointSettings.OnReachWaypointEvent.Invoke();
            useSpeedLimit = onReachWaypointSettings.useSpeedLimit;
            speedLimit = onReachWaypointSettings.speedLimit;
            useBrakeTrigger = onReachWaypointSettings.useBrakeTrigger;
            brakeAmount = onReachWaypointSettings.brakeAmount * -1;
            releaseBrakeWhenStopped = onReachWaypointSettings.releaseBrakeWhenStopped;
            waypointTimer = stuckTimer = 0.0f;
            routeDistanceTraveled = onReachWaypointSettings.distanceFromStart;
            if (onReachWaypointSettings.newRoutePoints.Length > 0)
            {
                int randomIndex = Random.Range(0, onReachWaypointSettings.newRoutePoints.Length);
                if (randomIndex == onReachWaypointSettings.newRoutePoints.Length) randomIndex -= 1;
                waypointRoute = onReachWaypointSettings.newRoutePoints[randomIndex].onReachWaypointSettings.parentRoute;
                routeDistanceTraveled = onReachWaypointSettings.newRoutePoints[randomIndex].onReachWaypointSettings.distanceFromStart;
                currentRoutePointIndex = onReachWaypointSettings.newRoutePoints[randomIndex].onReachWaypointSettings.waypointIndexnumber;
            }
            else
            {
                if (onReachWaypointSettings.waypointIndexnumber < onReachWaypointSettings.parentRoute.waypointDataList.Count) currentRoutePointIndex = onReachWaypointSettings.waypointIndexnumber;
            }
            if (onReachWaypointSettings.desiredRouteIndexes.Length > 0)
            {
                desiredRouteIndex = onReachWaypointSettings.desiredRouteIndexes[0];
                findDesiredRoute = true;
            }
            else
            {
                findDesiredRoute = false;
            }
            if (onReachWaypointSettings.stopDriving) StopDriving();
        }

        [ContextMenu("StartDriving")] public void StartDriving() { isDriving = canStop = true; }
        [ContextMenu("StopDriving")] public void StopDriving() { if (canStop) isDriving = false; }

        [ContextMenu("UpdateSensors")]
        public void UpdateSensors()
        {
            for (int i = 0; i < detectionData.Length; i++)
            {
                detectionData[i].sensor.number = i;
                detectionData[i].sensor.updateInterval = m_AISettings.updateInterval;
                detectionData[i].sensor.layerMask = m_AISettings.detectionLayers;
                detectionData[i].sensor.centerWidth = m_AISettings.sensorFCenterWidth;
                detectionData[i].sensor.height = m_AISettings.sensorHeight;
                detectionData[i].sensor.showEditorGizmos = m_AISettings.showEditorGizmos;

                switch (i)
                {
                    case 0:
                        detectionData[i].sensor.width = m_AISettings.sensorFSideWidth - 0.02f;
                        detectionData[i].sensor.length = m_AISettings.sensorFSideLength;
                        break;
                    case 1:
                        detectionData[i].sensor.width = m_AISettings.sensorFSideWidth - 0.02f;
                        detectionData[i].sensor.length = m_AISettings.sensorFSideLength;
                        break;
                    case 2:
                        detectionData[i].sensor.width = m_AISettings.sensorFSideWidth - 0.02f;
                        detectionData[i].sensor.length = m_AISettings.sensorFSideLength;
                        break;
                    case 3:
                        detectionData[i].sensor.width = m_AISettings.sensorFCenterWidth - 0.02f;
                        detectionData[i].sensor.length = m_AISettings.sensorFCenterLength;
                        break;
                    case 4:
                        detectionData[i].sensor.width = m_AISettings.sensorFCenterWidth - 0.02f;
                        detectionData[i].sensor.length = m_AISettings.sensorFCenterLength;
                        break;
                    case 5:
                        detectionData[i].sensor.width = m_AISettings.sensorFCenterWidth - 0.02f;
                        detectionData[i].sensor.length = m_AISettings.sensorFCenterLength;
                        break;
                    case 6:
                        detectionData[i].sensor.width = m_AISettings.sensorFSideWidth - 0.02f;
                        detectionData[i].sensor.length = m_AISettings.sensorFSideLength;
                        break;
                    case 7:
                        detectionData[i].sensor.width = m_AISettings.sensorFSideWidth - 0.02f;
                        detectionData[i].sensor.length = m_AISettings.sensorFSideLength;
                        break;
                    case 8:
                        detectionData[i].sensor.width = m_AISettings.sensorFSideWidth - 0.02f;
                        detectionData[i].sensor.length = m_AISettings.sensorFSideLength;
                        break;
                    case 9:
                        detectionData[i].sensor.width = m_AISettings.sensorLRWidth - 0.02f;
                        detectionData[i].sensor.length = m_AISettings.sensorLRLength;
                        break;
                    case 10:
                        detectionData[i].sensor.width = m_AISettings.sensorLRWidth - 0.02f;
                        detectionData[i].sensor.length = m_AISettings.sensorLRLength;
                        break;
                }
            }
            for (int i = 0; i < detectionData.Length; i++) detectionData[i].sensor.UpdateSensorTransform();
        }

        public virtual void CheckIfLost()
        {
            if (isDriving)
            {
                waypointTimer += Time.deltaTime * 1;
                if (m_AISettings.useWaypointReset && waypointTimer >= m_AISettings.waypointReset) MoveVehicleToTarget();
                if (m_AISettings.useStuckReset && stuckTimer >= m_AISettings.stuckReset) MoveVehicleToTarget();
                stuckTimer = driveSystem.currentSpeed < 5.0f ? stuckTimer + (Time.deltaTime * 1) : 0;
            }
            else waypointTimer = stuckTimer = 0.0f;
        }

        public void MoveVehicleToTarget()
        {
            waypointTimer = stuckTimer = 0;
            if (customResetPoint)
            {
                waypointRoute = stuckResetRoute;
                transformCached.position = waypointRoute.waypointDataList[stuckResetPoint - 1]._transform.position;//stuckResetPoint.position;
                transformCached.LookAt(waypointRoute.waypointDataList[stuckResetPoint]._transform);
                RouteProgress();
            }
            else
            {
                transformCached.position = driveTarget.position;
                RouteProgress();
                transformCached.rotation = driveTarget.rotation;
            }
        }

    }
}