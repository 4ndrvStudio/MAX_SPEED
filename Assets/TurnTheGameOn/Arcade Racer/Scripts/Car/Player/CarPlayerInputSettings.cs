namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "CarPlayerInputSettings", menuName = "TurnTheGameOn/Arcade Racer/CarPlayerInputSettings")]
    public class CarPlayerInputSettings : ScriptableObject
    {
        public CarUIManager mobileCanvas;
        public CarUIManager defaultCanvas;
        public UIType uIType;
        public MobileSteeringType mobileSteeringType;
        public CarPlayerInputInfo inputAxes;
        public UIInputModuleSettings inputModuleSettings;
        public string pause;
    }
}