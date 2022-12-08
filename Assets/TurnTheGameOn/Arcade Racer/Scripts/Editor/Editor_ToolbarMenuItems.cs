namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEditor;
    using UnityEngine;

    public class Editor_ToolbarMenuItems : Editor
    {
        [MenuItem("Tools/TurnTheGameOn/Arcade Racer/Create/AI_WaypointRoute %&r")]
        private static void SpawnWaypointRoute()
        {
            GameObject _AI_WaypointRoute = Instantiate(Resources.Load("CarAIWaypointRoute")) as GameObject;
            _AI_WaypointRoute.name = "CarAIWaypointRoute";
            GameObject[] newSelection = new GameObject[1];
            newSelection[0] = _AI_WaypointRoute;
            Selection.objects = newSelection;
        }

        [MenuItem("Tools/TurnTheGameOn/Arcade Racer/Create/TrafficLightManager %&l")]
        private static void SpawnTrafficLightManager()
        {
            GameObject _TrafficLightManager = Instantiate(Resources.Load("TrafficLightManager")) as GameObject;
            _TrafficLightManager.name = "TrafficLightManager";
            GameObject[] newSelection = new GameObject[1];
            newSelection[0] = _TrafficLightManager;
            Selection.objects = newSelection;
        }

        [MenuItem("Tools/TurnTheGameOn/Arcade Racer/PlayerPrefs/DeleteAll")]
        private static void DeletePlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
        }

        [MenuItem("Tools/TurnTheGameOn/Arcade Racer/EditorPrefs/DeleteAll")]
        private static void DeleteEditorPrefs()
        {
            EditorPrefs.DeleteAll();
            EditorData.ProjectInfo.ResetWelcome();
        }

    }
}