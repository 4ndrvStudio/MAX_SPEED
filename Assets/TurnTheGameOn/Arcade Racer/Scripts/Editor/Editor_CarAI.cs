namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;
    using UnityEditor;

    [CustomEditor(typeof(CarAI))]
    public class Editor_CarAI : Editor
    {
        public override void OnInspectorGUI()
        {
            CarAI vehicleAI = (CarAI)target;
            DrawDefaultInspector();

            if (GUILayout.Button("Update Sensors"))
            {
                vehicleAI.UpdateSensors();
            }
        }
    }
}