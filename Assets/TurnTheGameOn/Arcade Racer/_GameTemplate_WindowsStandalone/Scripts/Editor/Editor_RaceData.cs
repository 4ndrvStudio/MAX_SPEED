namespace TurnTheGameOn.ArcadeRacer
{
	using UnityEditor;

    [CustomEditor(typeof(RaceData))]
	public class Editor_RaceData : Editor
	{
		public override void OnInspectorGUI()
		{
            RaceData quickRace_Data = (RaceData)target;

            SerializedProperty id = serializedObject.FindProperty ("id");
			EditorGUI.BeginChangeCheck ();
			EditorGUILayout.PropertyField (id, true);
			if (EditorGUI.EndChangeCheck ())
				serializedObject.ApplyModifiedProperties ();

            EditorGUILayout.HelpBox
                    (
                    "Id is used by the load/save system, create unique Ids for each race.",
                    MessageType.Info,
                    true
                    );

            SerializedProperty sceneName = serializedObject.FindProperty("sceneName");
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(sceneName, true);
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();

            SerializedProperty raceName = serializedObject.FindProperty("raceName");
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(raceName, true);
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();

            SerializedProperty description = serializedObject.FindProperty("description");
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(description, true);
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();

            SerializedProperty menuImage = serializedObject.FindProperty("menuImage");
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(menuImage, true);
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();

            SerializedProperty laps = serializedObject.FindProperty("laps");
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(laps, true);
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();

            SerializedProperty preRaceTime = serializedObject.FindProperty("preRaceTime");
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(preRaceTime, true);
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();

            SerializedProperty racerInfo = serializedObject.FindProperty("racerInfo");
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(racerInfo, true);
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();

            SerializedProperty prize = serializedObject.FindProperty("prize");
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(prize, true);
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();

            SerializedProperty miniMapSettings = serializedObject.FindProperty("miniMapSettings");
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(miniMapSettings, true);
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();

            SerializedProperty unlockPrice = serializedObject.FindProperty("unlockPrice");
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(unlockPrice, true);
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();

            SerializedProperty locked = serializedObject.FindProperty("locked");
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(locked, true);
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();

            SerializedProperty showWaypointArrow = serializedObject.FindProperty("showWaypointArrow");
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(showWaypointArrow, true);
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();

            SerializedProperty showWrongWayWarning = serializedObject.FindProperty("showWrongWayWarning");
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(showWrongWayWarning, true);
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();

            SerializedProperty usePlayerVictoryScene = serializedObject.FindProperty("usePlayerVictoryScene");
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(usePlayerVictoryScene, true);
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();

            SerializedProperty victorySceneName = serializedObject.FindProperty("victorySceneName");
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(victorySceneName, true);
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();

            SerializedProperty defeatSceneName = serializedObject.FindProperty("defeatSceneName");
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(defeatSceneName, true);
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();

            EditorUtility.SetDirty (quickRace_Data);
		}
	}
}