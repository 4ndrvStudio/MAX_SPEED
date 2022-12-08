namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEditor;

    [CustomEditor(typeof(CarPlayerInputSettings))]
    public class Editor_CarPlayerInputSettings : Editor
    {
        public override void OnInspectorGUI()
        {
            CarPlayerInputSettings vehicleInput = (CarPlayerInputSettings)target;

            SerializedProperty defaultCanvas = serializedObject.FindProperty("defaultCanvas");
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(defaultCanvas, true);
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();

            SerializedProperty mobileCanvas = serializedObject.FindProperty("mobileCanvas");
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(mobileCanvas, true);
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();

            SerializedProperty uIType = serializedObject.FindProperty("uIType");
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(uIType, true);
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();

            if (vehicleInput.uIType == UIType.Mobile)
            {
                SerializedProperty mobileSteeringType = serializedObject.FindProperty("mobileSteeringType");
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(mobileSteeringType, true);
                if (EditorGUI.EndChangeCheck())
                    serializedObject.ApplyModifiedProperties();
            }

            SerializedProperty inputAxes = serializedObject.FindProperty("inputAxes");
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(inputAxes, true);
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();

            SerializedProperty inputModuleSettings = serializedObject.FindProperty("inputModuleSettings");
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(inputModuleSettings, true);
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();

            SerializedProperty pause = serializedObject.FindProperty("pause");
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(pause, true);
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();

            EditorUtility.SetDirty(vehicleInput);
        }
    }
}