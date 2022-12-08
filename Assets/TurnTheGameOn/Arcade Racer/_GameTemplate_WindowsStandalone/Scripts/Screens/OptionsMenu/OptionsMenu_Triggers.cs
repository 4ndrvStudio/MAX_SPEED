namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;

    public class OptionsMenu_Triggers : MonoBehaviour
    {
        public void OpenOptionsScreen()
        {
            OptionsMenu.Instance.Open();
        }

        public void CloseOptionsScreen()
        {
            OptionsMenu.Instance.Open();
        }
    }
}