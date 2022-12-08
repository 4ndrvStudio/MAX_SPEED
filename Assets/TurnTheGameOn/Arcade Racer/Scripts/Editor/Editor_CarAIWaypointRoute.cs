namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;
    using UnityEditor;
    using UnityEditorInternal;

    [CanEditMultipleObjects]
    [CustomEditor(typeof(CarAIWaypointRoute))]
    public class Editor_CarAIWaypointRoute : Editor
    {
        CarAIWaypointRoute circuit;
        ReorderableList reorderableList;
        float lineHeight;
        float lineHeightSpace;
        private bool showGizmoSettings;
        private bool showWaypoints;
        private bool showAutoAssignJunctionHelper;
        private CarAIWaypointRoute autoJunctionGetPointsTarget;
        private bool canShowAutoAssignJunctionsButton;
        private bool didAutoConfigureJunctions;

        void OnEnable()
        {
            lineHeight = EditorGUIUtility.singleLineHeight;
            lineHeightSpace = lineHeight + 0;

            circuit = (CarAIWaypointRoute)target;
            reorderableList = new ReorderableList(serializedObject, serializedObject.FindProperty("waypointDataList"), false, true, false, true);
            reorderableList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                SerializedProperty element = reorderableList.serializedProperty.GetArrayElementAtIndex(index);

                EditorGUI.LabelField(new Rect(rect.x, rect.y, rect.width, lineHeight), element.FindPropertyRelative("_name").stringValue);
                EditorGUI.indentLevel = 7;
                GUI.enabled = false;
                EditorGUI.PropertyField(new Rect(rect.x, rect.y + (lineHeightSpace * 0), rect.width, lineHeight), element.FindPropertyRelative("_transform"));
                GUI.enabled = true;
                EditorGUI.indentLevel = 0;
                reorderableList.elementHeightCallback = (int _index) => lineHeightSpace * 2;
            };
            reorderableList.drawHeaderCallback = (Rect rect) =>
            {
                EditorGUI.LabelField(new Rect(rect.x, rect.y, rect.width, lineHeight), "Waypoints");
            };
            reorderableList.onRemoveCallback = (ReorderableList list) =>
            {
                GameObject go = circuit.waypointDataList[reorderableList.index]._transform.gameObject;
                circuit.waypointDataList.RemoveAt(reorderableList.index);
            };
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.BeginVertical(EditorStyles.inspectorFullWidthMargins);
            EditorStyles.label.wordWrap = true;


            EditorGUILayout.HelpBox("Alt + Left Click in scene view on a Collider to add new points to the route", MessageType.None);

            EditorGUILayout.BeginVertical("Box");

            SerializedProperty _AIWaypointRouteSettings = serializedObject.FindProperty("_AIWaypointRouteSettings");
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(_AIWaypointRouteSettings, true);
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();

            SerializedProperty routeIndex = serializedObject.FindProperty("routeIndex");
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(routeIndex, true);
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();

            EditorGUILayout.EndVertical();

            showWaypoints = EditorGUILayout.Foldout(showWaypoints, "Waypoints", true);
            if (showWaypoints)
            {
                EditorGUILayout.BeginVertical("Box");
                EditorGUI.BeginChangeCheck();
                reorderableList.DoLayoutList();
                if (EditorGUI.EndChangeCheck())
                    serializedObject.ApplyModifiedProperties();
                EditorGUILayout.EndVertical();
            }

            showAutoAssignJunctionHelper = EditorGUILayout.Foldout(showAutoAssignJunctionHelper, "Automatic Junction Configuration Helper", true);
            if (showAutoAssignJunctionHelper)
            {
                if (didAutoConfigureJunctions)
                {
                    EditorGUILayout.HelpBox(
                    "<color=green>Success</color>: " + "Junctions have been auto assigned to this route, you may repeat this process to assign additional junction points.\n\n" +
                    "Assign a route to get and auto assign junction points from.\n\n" +
                    "The route to get points from must have at least the same amount of points as this route."
                    , MessageType.None);
                }
                else
                {
                    EditorGUILayout.HelpBox(
                    "Assign a route to get and auto assign junction points from.\n\n" +
                    "The route to get points from must have at least the same amount of points as this route."
                    , MessageType.None);
                }

                if (!autoJunctionGetPointsTarget)
                {
                    canShowAutoAssignJunctionsButton = false;
                    EditorGUILayout.HelpBox("Assign a waypoint route to map it's waypoints as junction points for this routes waypoints.", MessageType.Warning);
                }
                else
                {
                    if (autoJunctionGetPointsTarget.waypointDataList.Count < circuit.waypointDataList.Count)
                    {
                        canShowAutoAssignJunctionsButton = false;
                        EditorGUILayout.HelpBox("The asssigned route has less points than this route, the route to get points from must have at least the same amount of points as this route.", MessageType.Error);
                    }
                    if (autoJunctionGetPointsTarget == circuit)
                    {
                        canShowAutoAssignJunctionsButton = false;
                        EditorGUILayout.HelpBox("The asssigned route is the same as this route, this is not supported. Please assign a unique waypoint route to retrieve targets from.", MessageType.Error);
                    }
                    else
                    {
                        canShowAutoAssignJunctionsButton = true;
                    }
                }
                autoJunctionGetPointsTarget = (CarAIWaypointRoute)EditorGUILayout.ObjectField("Get Targets From", autoJunctionGetPointsTarget, typeof(CarAIWaypointRoute), true);
                if (canShowAutoAssignJunctionsButton)
                {
                    if (didAutoConfigureJunctions == false)
                    {
                        if (GUILayout.Button("Auto Configure Junction Points"))
                        {
                            didAutoConfigureJunctions = true;
                            CarAIWaypointRouteJunctionConfigurationHelper.AssignNextWaypointJunction(autoJunctionGetPointsTarget, circuit);
                        }
                    }
                }
            }

            SerializedProperty spawnTrafficVehicles = serializedObject.FindProperty("spawnTrafficVehicles");
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(spawnTrafficVehicles, true);
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();
            EditorGUILayout.EndVertical();
        }

        void OnSceneGUI()
        {
            CarAIWaypointRoute waypointManager = (CarAIWaypointRoute)target;

            for (int i = 0; i < waypointManager.waypointDataList.Count; i++)
            {
                if (waypointManager.waypointDataList[i]._waypoint)
                {
                    GUIStyle style = new GUIStyle();
                    string target = "";
                    style.normal.textColor = Color.green;
                    if (waypointManager.waypointDataList[i]._waypoint.onReachWaypointSettings.desiredRouteIndexes.Length > 0)
                    {
                        target = "      Target: " + waypointManager.waypointDataList[i]._waypoint.onReachWaypointSettings.desiredRouteIndexes[0];
                        style.normal.textColor = Color.red;
                    }
                    Handles.Label(waypointManager.waypointDataList[i]._waypoint.transform.position + new Vector3(0, 0.25f, 0),
                    "    Waypoint:   " + waypointManager.waypointDataList[i]._waypoint.onReachWaypointSettings.waypointIndexnumber.ToString() + "\n" +
                    "    SpeedLimit: " + waypointManager.waypointDataList[i]._waypoint.onReachWaypointSettings.speedLimit + "\n" +
                    target,
                    //"    AI Speed: " + (waypointManager.waypointDataList[i]._waypoint.onReachWaypointSettings.AISpeedFactor * 100).ToString() + "%\n" +
                    //"    Junctions: " + waypointManager.waypointDataList[i]._waypoint.junctionPoint.Length + "\n",
                    style
                    );
                }
            }

            // Handles.BeginGUI();
            // if (GUILayout.Button("Test Button", GUILayout.Width(100)))
            // {
            // 	//
            // }
            // Handles.EndGUI();

            Event e = Event.current;
            if (e.type == EventType.MouseDown && e.button == 0 && e.alt && e.button == 0 && e.control)
            {
                Ray worldRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
                RaycastHit hitInfo;
                if (Physics.Raycast(worldRay, out hitInfo))
                {
                    //serializedObject.UpdateIfRequiredOrScript();
                    waypointManager.ClickToInsertSpawnNextWaypoint(hitInfo.point);
                    //serializedObject.UpdateIfRequiredOrScript();
                }
            }
            else if (e.type == EventType.MouseDown && e.button == 0 && e.alt)
            {
                Ray worldRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
                RaycastHit hitInfo;
                if (Physics.Raycast(worldRay, out hitInfo))
                {
                    //serializedObject.UpdateIfRequiredOrScript();
                    waypointManager.ClickToSpawnNextWaypoint(hitInfo.point);
                    //serializedObject.UpdateIfRequiredOrScript();
                }
            }

        }

    }
}