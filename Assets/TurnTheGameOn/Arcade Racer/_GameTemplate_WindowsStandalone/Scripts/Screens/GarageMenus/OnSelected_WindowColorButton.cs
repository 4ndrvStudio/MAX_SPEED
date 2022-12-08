namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;

    public class OnSelected_WindowColorButton : MonoBehaviour
    {
        public string colorHex;
        public void OnSelectedEvent()
        {
            GameTemplate_PlayerCustomization.Instance.SetNewWindowColor(colorHex);
        }
    }
}