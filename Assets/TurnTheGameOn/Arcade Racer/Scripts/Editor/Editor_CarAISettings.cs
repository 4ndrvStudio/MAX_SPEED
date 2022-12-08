namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEditor;

    [CustomEditor(typeof(CarAISettings))]
    public class Editor_CarAISettings : Editor
    {
        private bool showCautionSettings;
        private bool showSensorSettings;
        private bool showInputSensitivitySettings;
        private bool showLookAheadSettings;
        private bool showStuckOrLostSettings;
        private bool showChangeLaneSettings;

        public override void OnInspectorGUI()
        {
            SerializedProperty showEditorGizmos = serializedObject.FindProperty("showEditorGizmos");
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(showEditorGizmos, true);
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();

            SerializedProperty useAnyWaypoint = serializedObject.FindProperty("useAnyWaypoint");
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(useAnyWaypoint, true);
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();

            #region Waypoint Distance Threshold
            EditorGUILayout.BeginVertical("Box");

            SerializedProperty useWaypointDistanceThreshold = serializedObject.FindProperty("useWaypointDistanceThreshold");
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(useWaypointDistanceThreshold, true);
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();

            SerializedProperty waypointDistanceThreshold = serializedObject.FindProperty("waypointDistanceThreshold");
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(waypointDistanceThreshold, true);
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();

            EditorGUILayout.EndVertical();
            #endregion

            #region Stuck Or Lost Settings
            EditorGUILayout.BeginVertical("Box");
            SerializedProperty useWaypointReset = serializedObject.FindProperty("useWaypointReset");
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(useWaypointReset, true);
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();

            SerializedProperty useStuckReset = serializedObject.FindProperty("useStuckReset");
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(useStuckReset, true);
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();

            SerializedProperty waypointReset = serializedObject.FindProperty("waypointReset");
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(waypointReset, true);
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();

            SerializedProperty stuckReset = serializedObject.FindProperty("stuckReset");
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(stuckReset, true);
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();

            EditorGUILayout.EndVertical();
            #endregion

            #region Change Lane Settings
            EditorGUILayout.BeginVertical("Box");

            SerializedProperty changeLaneCooldown = serializedObject.FindProperty("changeLaneCooldown");
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(changeLaneCooldown, true);
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();

            SerializedProperty changeLaneDistance = serializedObject.FindProperty("changeLaneDistance");
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(changeLaneDistance, true);
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();

            EditorGUILayout.EndVertical();
            #endregion

            

            #region Caution Settings
            EditorGUILayout.BeginVertical("Box");

            SerializedProperty autoCautionDistanceThreshold = serializedObject.FindProperty("autoCautionDistanceThreshold");
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(autoCautionDistanceThreshold, true);
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();

            SerializedProperty cautionSlowSpeed = serializedObject.FindProperty("cautionSlowSpeed");
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(cautionSlowSpeed, true);
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();

            EditorGUILayout.EndVertical();
            #endregion

            #region Input Sensitivity
            EditorGUILayout.BeginVertical("Box");

            SerializedProperty steeringSensitivity = serializedObject.FindProperty("steeringSensitivity");
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(steeringSensitivity, true);
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();

            SerializedProperty accelerationSensitivity = serializedObject.FindProperty("accelerationSensitivity");
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(accelerationSensitivity, true);
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();

            SerializedProperty brakeSensitivity = serializedObject.FindProperty("brakeSensitivity");
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(brakeSensitivity, true);
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();
            EditorGUILayout.EndVertical();
            #endregion



            #region Sensor Settings
            EditorGUILayout.BeginVertical("Box");
            SerializedProperty enableSensors = serializedObject.FindProperty("enableSensors");
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(enableSensors, true);
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();

            SerializedProperty updateInterval = serializedObject.FindProperty("updateInterval");
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(updateInterval, true);
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();

            SerializedProperty detectionLayers = serializedObject.FindProperty("detectionLayers");
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(detectionLayers, true);
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();

            SerializedProperty sensorHeight = serializedObject.FindProperty("sensorHeight");
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(sensorHeight, true);
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();

            SerializedProperty sensorFCenterWidth = serializedObject.FindProperty("sensorFCenterWidth");
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(sensorFCenterWidth, true);
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();

            SerializedProperty sensorFSideWidth = serializedObject.FindProperty("sensorFSideWidth");
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(sensorFSideWidth, true);
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();

            SerializedProperty sensorLRWidth = serializedObject.FindProperty("sensorLRWidth");
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(sensorLRWidth, true);
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();

            SerializedProperty sensorFCenterLength = serializedObject.FindProperty("sensorFCenterLength");
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(sensorFCenterLength, true);
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();

            SerializedProperty sensorFSideLength = serializedObject.FindProperty("sensorFSideLength");
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(sensorFSideLength, true);
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();

            SerializedProperty sensorLRLength = serializedObject.FindProperty("sensorLRLength");
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(sensorLRLength, true);
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();
            EditorGUILayout.EndVertical();
            #endregion

        }
    }
}