namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "PlayerVehicles", menuName = "TurnTheGameOn/Arcade Racer/PlayerVehicles")]
    public class PlayerVehicles : ScriptableObject
    {
        public int currentVehicleSelection { get; private set; }
        public PlayerVehicle[] playerVehicles;

        private void OnEnable()
        {
            currentVehicleSelection = 0;
        }

        [ContextMenu("SelectNextVehicle")]
        public void SelectNextVehicle()
        {
            if (playerVehicles.Length > 0)
            {
                if (currentVehicleSelection == playerVehicles.Length - 1)
                {
                    currentVehicleSelection = 0;
                }
                else
                {
                    currentVehicleSelection += 1;
                }
            }
            
        }

        [ContextMenu("SelectPreviousVehicle")]
        public void SelectPreviousVehicle()
        {
            if (playerVehicles.Length > 0)
            {
                if (currentVehicleSelection == 0)
                {
                    currentVehicleSelection = playerVehicles.Length - 1;
                }
                else
                {
                    currentVehicleSelection -= 1;
                }
            }
        }

        public void SelectVehicleIndex(int _index)
        {
            if (_index > playerVehicles.Length - 1 || _index < 0)
            {
                currentVehicleSelection = 0;
            }
            else
            {
                currentVehicleSelection = _index;
            }
        }
    }
}