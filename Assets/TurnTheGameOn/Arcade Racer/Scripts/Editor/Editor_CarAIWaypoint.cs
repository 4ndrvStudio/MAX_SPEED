namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;
    using UnityEditor;

    [CustomEditor(typeof(CarAIWaypoint)), CanEditMultipleObjects]
    public class Editor_CarAIWaypoint : Editor
    {
        void OnSceneGUI()
        {
            CarAIWaypoint waypoint = (CarAIWaypoint)target;

            Handles.Label(waypoint.transform.position + new Vector3(0, 0.25f, 0),
            "    Waypoint Number: " + waypoint.onReachWaypointSettings.waypointIndexnumber.ToString() + "\n"
            );
            Vector3 position = waypoint.transform.position + Vector3.up * 2f;
            float size = 0.2f;
            float pickSize = size * 0.2f;

            if (Handles.Button(position, Quaternion.identity, size, pickSize, Handles.RectangleHandleCap))
                Debug.Log("The button was pressed!");

            //Handles.BeginGUI();
            //if (GUILayout.Button("Test Button", GUILayout.Width(100)))
            //{
            //    //
            //}
            //Handles.EndGUI();
        }
    }
}