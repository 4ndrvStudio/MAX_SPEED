namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;
    using System.Collections.Generic;

    [System.Serializable]
    public class CarAIWaypointRoute : MonoBehaviour
    {
        public CarAIWaypointRouteSettings _AIWaypointRouteSettings;
        public List<CarAIWaypoinInfo> waypointDataList;
        public int routeIndex;
        public float totalRouteDistance { get; private set; }
        public bool stopForTrafficLight { get; private set; }
        public GameObject[] spawnTrafficVehicles;

        #region Awake
        private void Awake()
        {
            for (int i = 0; i < waypointDataList.Count; i++)
            {
                waypointDataList[i]._transform.hasChanged = false;
            }
            if (waypointDataList.Count > 2) CachePositionsAndDistances(); // ensure route data is updated and cached
        }

        private void Start()
        {
            SpawnTrafficVehicles();
        }

        [HideInInspector] private Vector3 pointA, pointB; // temp variables - cached to prevent runtime gc
        [HideInInspector] private float accumulateDistance = 0; // temp variables - cached to prevent runtime gc
        private void CachePositionsAndDistances()
        {
            totalRouteDistance = waypointDataList[waypointDataList.Count - 1]._waypoint.onReachWaypointSettings.distanceFromStart;
            accumulateDistance = 0;
            for (int i = 1; i < waypointDataList.Count + 1; i++)
            {
                waypointDataList[i - 1]._waypoint.onReachWaypointSettings.parentRoute = this;
                pointA = waypointDataList[i - 1]._transform.position;
                if (i == 1) pointB = waypointDataList[i - 1]._transform.position;
                else pointB = (i < waypointDataList.Count) ? waypointDataList[i]._transform.position : waypointDataList[0]._transform.position;
                accumulateDistance += (pointA - pointB).magnitude;
                waypointDataList[i - 1]._waypoint.onReachWaypointSettings.distanceFromStart = accumulateDistance;
                waypointDataList[i - 1]._waypoint.onReachWaypointSettings.distanceFromStart = accumulateDistance;
                waypointDataList[i - 1]._waypoint.onReachWaypointSettings.parentRoute = this;
            }
        }
        #endregion

        #region Traffic Control
        public void StopForTrafficlight(bool _stop)
        {
            stopForTrafficLight = _stop;
        }

        public void SpawnTrafficVehicles()
        {
            for (int i = 0, j = 0; i < spawnTrafficVehicles.Length && j < waypointDataList.Count - 1; i++, j++)
            {
                Vector3 spawnPosition = waypointDataList[j]._transform.position;
                spawnPosition.y += 3;
                GameObject spawnedTrafficVehicle = Instantiate(spawnTrafficVehicles[i], spawnPosition, waypointDataList[j]._transform.rotation);
                spawnedTrafficVehicle.GetComponent<CarAI>().waypointRoute = this;
                spawnedTrafficVehicle.transform.LookAt(waypointDataList[j + 1]._transform);
                j += 1; // increase j again tospawn vehicles with more space between
            }
        }
        #endregion

        #region Unity Editor Helper Methods
        bool isBetweenPoints;

        bool IsCBetweenAB(Vector3 A, Vector3 B, Vector3 C)
        {
            return (
                Vector3.Dot((B - A).normalized, (C - B).normalized) < 0f && Vector3.Dot((A - B).normalized, (C - A).normalized) < 0f &&
                Vector3.Distance(A, B) >= Vector3.Distance(A, C) &&
                Vector3.Distance(A, B) >= Vector3.Distance(B, C)
                );
        }

        [HideInInspector] private GameObject newWaypoint;
        public void ClickToSpawnNextWaypoint(Vector3 _position)
        {
            newWaypoint = Instantiate(Resources.Load("CarAIWaypoint"), _position, Quaternion.identity, gameObject.transform) as GameObject;
            CarAIWaypoinInfo newPoint = new CarAIWaypoinInfo();
            newPoint._name = newWaypoint.name = "CarAIWaypoint " + (waypointDataList.Count + 1);
            newPoint._transform = newWaypoint.transform;
            newPoint._waypoint = newWaypoint.GetComponent<CarAIWaypoint>();
            newPoint._waypoint.onReachWaypointSettings.waypointIndexnumber = waypointDataList.Count + 1;
            waypointDataList.Add(newPoint);
        }

        public void ClickToInsertSpawnNextWaypoint(Vector3 _position)
        {
            isBetweenPoints = false;
            int insertIndex = 0;
            if (waypointDataList.Count >= 2)
            {
                for (int i = 0; i < waypointDataList.Count - 1; i++)
                {
                    Vector3 point_A = waypointDataList[i]._transform.position;
                    Vector3 point_B = waypointDataList[i + 1]._transform.position;
                    isBetweenPoints = IsCBetweenAB(point_A, point_B, _position);
                    insertIndex = i + 1;
                    if (isBetweenPoints) break;
                }
            }

            newWaypoint = Instantiate(Resources.Load("CarAIWaypoint"), _position, Quaternion.identity, gameObject.transform) as GameObject;
            CarAIWaypoinInfo newPoint = new CarAIWaypoinInfo();
            newPoint._transform = newWaypoint.transform;
            newPoint._waypoint = newWaypoint.GetComponent<CarAIWaypoint>();
            if (isBetweenPoints)
            {
                newPoint._transform.SetSiblingIndex(insertIndex);
                newPoint._name = newWaypoint.name = "CarAIWaypoint " + (insertIndex + 1);
                waypointDataList.Insert(insertIndex, newPoint);
                for (int i = 0; i < waypointDataList.Count; i++)
                {
                    int newIndexName = i + 1;
                    waypointDataList[i]._transform.gameObject.name = "CarAIWaypoint " + newIndexName;
                    waypointDataList[i]._waypoint.onReachWaypointSettings.waypointIndexnumber = i + 1;
                    waypointDataList[i]._waypoint.onReachWaypointSettings.waypointIndexnumber = i;
                }
            }
            else
            {
                newPoint._name = newWaypoint.name = "CarAIWaypoint " + (waypointDataList.Count + 1);
                newPoint._waypoint.onReachWaypointSettings.waypointIndexnumber = waypointDataList.Count + 1;
                waypointDataList.Add(newPoint);
            }
        }
        #endregion

        #region Gizmos
        private void OnDrawGizmos() { if (_AIWaypointRouteSettings.alwaysDrawGizmos) DrawGizmos(false); }
        private void OnDrawGizmosSelected() { if (_AIWaypointRouteSettings.alwaysDrawGizmos) DrawGizmos(true); }

        [HideInInspector] Transform arrowPointer;
        private Transform junctionPosition;
        private Matrix4x4 previousMatrix;
        private int lookAtIndex;
        private float updateGizmoTimer;

        private void DrawGizmos(bool selected)
        {
            if (_AIWaypointRouteSettings.canUpdateGizmos)
            {
                for (int i = 0; i < waypointDataList.Count; i++)
                {
                    if (waypointDataList[i]._transform != null)
                    {
                        if (waypointDataList[i]._transform.hasChanged)
                        {
                            waypointDataList[i]._transform.hasChanged = false;
                            _AIWaypointRouteSettings.canUpdateGizmos = false;
                            updateGizmoTimer = 0.0f;
                            break;
                        }
                    }
                    else { break; }
                }
            }
            else if (_AIWaypointRouteSettings.canUpdateGizmos == false)
            {
                updateGizmoTimer += Time.deltaTime;
                if (updateGizmoTimer >= _AIWaypointRouteSettings.updateGizmoCoolDown)
                {
                    _AIWaypointRouteSettings.canUpdateGizmos = true;
                }
            }

            if (_AIWaypointRouteSettings.canUpdateGizmos)
            {
                {
                    if (waypointDataList.Count > 2)
                    {
                        if (waypointDataList.Count > 2) CachePositionsAndDistances();
                        Gizmos.color = selected ? _AIWaypointRouteSettings.selectedPathColor : _AIWaypointRouteSettings.pathColor;
                        for (int i = 1; i < waypointDataList.Count; i++)
                        {
                            Gizmos.DrawLine(waypointDataList[i - 1]._transform.position, waypointDataList[i]._transform.position);
                        }
                        if (!arrowPointer)
                        {
                            arrowPointer = new GameObject("IK_Driver_Waypoint_Gizmo_Helper").transform;
                            arrowPointer.hideFlags = HideFlags.HideAndDontSave;
                        }
                        for (int i = 0; i < waypointDataList.Count; i++) // Draw Arrows to junctions
                        {
                            if (waypointDataList[i]._waypoint != null)
                            {
                                Gizmos.color = selected ? _AIWaypointRouteSettings.selectedJunctionColor : _AIWaypointRouteSettings.junctionColor;
                                if (waypointDataList[i]._waypoint.onReachWaypointSettings.newRoutePoints.Length > 0)
                                {
                                    for (int j = 0; j < waypointDataList[i]._waypoint.onReachWaypointSettings.newRoutePoints.Length; j++)
                                    {
                                        Gizmos.DrawLine(waypointDataList[i]._transform.position, waypointDataList[i]._waypoint.onReachWaypointSettings.newRoutePoints[j].transform.position);
                                    }
                                }
                                for (int j = 0; j < waypointDataList[i]._waypoint.junctionPoint.Length; ++j)
                                {
                                    Gizmos.DrawLine(waypointDataList[i]._transform.position, waypointDataList[i]._waypoint.junctionPoint[j].transform.position);
                                    junctionPosition = waypointDataList[i]._waypoint.junctionPoint[j].transform;
                                    previousMatrix = Gizmos.matrix;
                                    arrowPointer.position = waypointDataList[i]._waypoint.junctionPoint[j].transform.position; //waypointData [i]._transform.position;
                                    arrowPointer.LookAt(waypointDataList[i]._transform);
                                    Gizmos.matrix = Matrix4x4.TRS(junctionPosition.position, arrowPointer.rotation, _AIWaypointRouteSettings.arrowHeadScale);
                                    Gizmos.DrawFrustum(Vector3.zero, _AIWaypointRouteSettings.arrowHeadWidth, _AIWaypointRouteSettings.arrowHeadLength, 0.0f, 5.0f);
                                    Gizmos.matrix = previousMatrix;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                        Gizmos.color = selected ? _AIWaypointRouteSettings.selectedPathColor : _AIWaypointRouteSettings.pathColor;
                        for (int i = 0; i < waypointDataList.Count; i++) // Draw Arrows to connecting waypoints
                        {
                            previousMatrix = Gizmos.matrix;
                            if (waypointDataList[waypointDataList.Count - 2]._waypoint != null && waypointDataList[i]._waypoint != null)
                            {
                                arrowPointer.position = i == 0 ? waypointDataList[waypointDataList.Count - 2]._waypoint.transform.position : waypointDataList[i]._waypoint.transform.position;
                                lookAtIndex = i == 0 ? waypointDataList.Count - 1 : i - 1;
                                if (i == 0)
                                {
                                    arrowPointer.LookAt(waypointDataList[waypointDataList.Count - 1]._waypoint.transform);
                                    arrowPointer.position = waypointDataList[i]._waypoint.transform.position;
                                    arrowPointer.Rotate(0, 180, 0);
                                }
                                else arrowPointer.LookAt(waypointDataList[lookAtIndex]._waypoint.transform);
                                Gizmos.matrix = Matrix4x4.TRS(waypointDataList[lookAtIndex]._waypoint.transform.position, arrowPointer.rotation, _AIWaypointRouteSettings.arrowHeadScale);
                                Gizmos.DrawFrustum(Vector3.zero, _AIWaypointRouteSettings.arrowHeadWidth, _AIWaypointRouteSettings.arrowHeadLength, 0.0f, 5.0f);
                            }
                            else
                            {
                                break;
                            }
                            previousMatrix = Gizmos.matrix;
                        }

                    }

                }
            }

        }
        #endregion

    }
}