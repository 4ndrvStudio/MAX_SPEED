namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;

    public class OnSelected_RimColorButton : MonoBehaviour
    {
        public string colorHex;
        public void OnSelectedEvent()
        {
            GameTemplate_PlayerCustomization.Instance.SetNewRimColor(colorHex);
        }
    }
}