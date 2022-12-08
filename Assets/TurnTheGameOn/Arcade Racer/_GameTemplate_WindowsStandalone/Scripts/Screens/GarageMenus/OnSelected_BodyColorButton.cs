namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;

    public class OnSelected_BodyColorButton : MonoBehaviour
    {
        public string colorHex;
        public void OnSelectedEvent()
        {
            GameTemplate_PlayerCustomization.Instance.SetNewBodyColor(colorHex);
        }
    }
}