namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;
    using UnityEditor;

    [CustomEditor(typeof(AvatarDriver))]
    public class Editor_AvatarDriver : Editor
    {
        private bool showCurrentIKDriverTargets, showCurrentIKTargetObjects, showIKSteeringWheelTargets, showOtherIKTargetObjects;

        public override void OnInspectorGUI()
        {
            GUI.enabled = false;
            EditorGUILayout.ObjectField("Script:", MonoScript.FromMonoBehaviour((AvatarDriver)target), typeof(AvatarDriver), false);
            GUI.enabled = true;

            EditorGUILayout.BeginVertical("Box");
            EditorGUI.BeginChangeCheck();

            SerializedProperty avatarSettings = serializedObject.FindProperty("avatarSettings");
            EditorGUILayout.PropertyField(avatarSettings, true);

            SerializedProperty animator = serializedObject.FindProperty("animator");
            EditorGUILayout.PropertyField(animator, true);

            SerializedProperty steeringWheel = serializedObject.FindProperty("steeringWheel");
            EditorGUILayout.PropertyField(steeringWheel, true);

            SerializedProperty readOnlySteeringWheel = serializedObject.FindProperty("readOnlySteeringWheel");
            EditorGUILayout.PropertyField(readOnlySteeringWheel, true);

            SerializedProperty vehicleRigidbody = serializedObject.FindProperty("vehicleRigidbody");
            EditorGUILayout.PropertyField(vehicleRigidbody, true);

            SerializedProperty gearText = serializedObject.FindProperty("gearText");
            EditorGUILayout.PropertyField(gearText, true);


            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();

            showCurrentIKDriverTargets = EditorGUILayout.Foldout(showCurrentIKDriverTargets, "IK Control Points", true);
            if (showCurrentIKDriverTargets)
            {
                EditorGUILayout.BeginVertical("Box");
                EditorGUI.BeginChangeCheck();

                SerializedProperty headLookIKCP = serializedObject.FindProperty("headLookIKCP");
                EditorGUILayout.PropertyField(headLookIKCP, true);

                SerializedProperty targetLeftHandIK = serializedObject.FindProperty("targetLeftHandIK");
                EditorGUILayout.PropertyField(targetLeftHandIK, true);

                SerializedProperty targetRightHandIK = serializedObject.FindProperty("targetRightHandIK");
                EditorGUILayout.PropertyField(targetRightHandIK, true);

                SerializedProperty targetLeftFootIK = serializedObject.FindProperty("targetLeftFootIK");
                EditorGUILayout.PropertyField(targetLeftFootIK, true);

                SerializedProperty targetRightFootIK = serializedObject.FindProperty("targetRightFootIK");
                EditorGUILayout.PropertyField(targetRightFootIK, true);

                if (EditorGUI.EndChangeCheck())
                    serializedObject.ApplyModifiedProperties();
                EditorGUILayout.EndVertical();
            }

            showCurrentIKTargetObjects = EditorGUILayout.Foldout(showCurrentIKTargetObjects, "Current IK Targets", true);
            if (showCurrentIKTargetObjects)
            {
                EditorGUILayout.BeginVertical("Box");
                EditorGUI.BeginChangeCheck();

                SerializedProperty leftHandObj = serializedObject.FindProperty("leftHandObj");
                EditorGUILayout.PropertyField(leftHandObj, true);

                SerializedProperty rightHandObj = serializedObject.FindProperty("rightHandObj");
                EditorGUILayout.PropertyField(rightHandObj, true);

                SerializedProperty leftFootObj = serializedObject.FindProperty("leftFootObj");
                EditorGUILayout.PropertyField(leftFootObj, true);

                SerializedProperty rightFootObj = serializedObject.FindProperty("rightFootObj");
                EditorGUILayout.PropertyField(rightFootObj, true);

                if (EditorGUI.EndChangeCheck())
                    serializedObject.ApplyModifiedProperties();
                EditorGUILayout.EndVertical();
            }

            showIKSteeringWheelTargets = EditorGUILayout.Foldout(showIKSteeringWheelTargets, "IK Targets", true);
            if (showIKSteeringWheelTargets)
            {
                EditorGUILayout.BeginVertical("Box");
                EditorGUI.BeginChangeCheck();

                SerializedProperty lhswt_W = serializedObject.FindProperty("lhswt_W");
                EditorGUILayout.PropertyField(lhswt_W, true);

                SerializedProperty lhswt_NW = serializedObject.FindProperty("lhswt_NW");
                EditorGUILayout.PropertyField(lhswt_NW, true);

                SerializedProperty lhswt_N = serializedObject.FindProperty("lhswt_N");
                EditorGUILayout.PropertyField(lhswt_N, true);

                SerializedProperty lhswt_NE = serializedObject.FindProperty("lhswt_NE");
                EditorGUILayout.PropertyField(lhswt_NE, true);

                SerializedProperty lhswt_E = serializedObject.FindProperty("lhswt_E");
                EditorGUILayout.PropertyField(lhswt_E, true);

                SerializedProperty lhswt_SE = serializedObject.FindProperty("lhswt_SE");
                EditorGUILayout.PropertyField(lhswt_SE, true);

                SerializedProperty lhswt_S = serializedObject.FindProperty("lhswt_S");
                EditorGUILayout.PropertyField(lhswt_S, true);

                SerializedProperty lhswt_SW = serializedObject.FindProperty("lhswt_SW");
                EditorGUILayout.PropertyField(lhswt_SW, true);

                SerializedProperty rhswt_W = serializedObject.FindProperty("rhswt_W");
                EditorGUILayout.PropertyField(rhswt_W, true);

                SerializedProperty rhswt_NW = serializedObject.FindProperty("rhswt_NW");
                EditorGUILayout.PropertyField(rhswt_NW, true);

                SerializedProperty rhswt_N = serializedObject.FindProperty("rhswt_N");
                EditorGUILayout.PropertyField(rhswt_N, true);

                SerializedProperty rhswt_NE = serializedObject.FindProperty("rhswt_NE");
                EditorGUILayout.PropertyField(rhswt_NE, true);

                SerializedProperty rhswt_E = serializedObject.FindProperty("rhswt_E");
                EditorGUILayout.PropertyField(rhswt_E, true);

                SerializedProperty rhswt_SE = serializedObject.FindProperty("rhswt_SE");
                EditorGUILayout.PropertyField(rhswt_SE, true);

                SerializedProperty rhswt_S = serializedObject.FindProperty("rhswt_S");
                EditorGUILayout.PropertyField(rhswt_S, true);

                SerializedProperty rhswt_SW = serializedObject.FindProperty("rhswt_SW");
                EditorGUILayout.PropertyField(rhswt_SW, true);

                SerializedProperty handShift = serializedObject.FindProperty("handShift");
                EditorGUILayout.PropertyField(handShift, true);

                SerializedProperty footBrake = serializedObject.FindProperty("footBrake");
                EditorGUILayout.PropertyField(footBrake, true);

                SerializedProperty footGas = serializedObject.FindProperty("footGas");
                EditorGUILayout.PropertyField(footGas, true);

                SerializedProperty leftFootIdle = serializedObject.FindProperty("leftFootIdle");
                EditorGUILayout.PropertyField(leftFootIdle, true);

                SerializedProperty footClutch = serializedObject.FindProperty("footClutch");
                EditorGUILayout.PropertyField(footClutch, true);

                SerializedProperty rightFootIdle = serializedObject.FindProperty("rightFootIdle");
                EditorGUILayout.PropertyField(rightFootIdle, true);

                SerializedProperty returnShiftTarget = serializedObject.FindProperty("returnShiftTarget");
                EditorGUILayout.PropertyField(returnShiftTarget, true);

                if (EditorGUI.EndChangeCheck())
                    serializedObject.ApplyModifiedProperties();
                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.EndVertical();
        }
    }
}