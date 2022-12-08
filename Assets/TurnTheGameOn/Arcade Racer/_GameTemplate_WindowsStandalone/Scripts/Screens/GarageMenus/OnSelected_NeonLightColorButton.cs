namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;

    public class OnSelected_NeonLightColorButton : MonoBehaviour
    {
        public string colorHex;
        public void OnSelectedEvent()
        {
            GameTemplate_PlayerCustomization.Instance.SetNewNeonLightColor(colorHex);
        }
    }
}