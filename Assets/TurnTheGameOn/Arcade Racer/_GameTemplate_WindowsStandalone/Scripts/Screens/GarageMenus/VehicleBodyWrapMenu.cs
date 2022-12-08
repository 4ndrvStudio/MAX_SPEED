namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections.Generic;

    public class VehicleBodyWrapMenu : ScreenBase
    {
        public static VehicleBodyWrapMenu Instance = null;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {

        }

        public void Button_Back()
        {
            Close();
            VehicleCustomizationMenu.Instance.Open();
        }

        public void Button_BodyWrap()
        {
        }

        public void Button_BodyPaint()
        {
        }

        public void Button_WindowTint()
        {
        }

        public void Button_RimColor()
        {
        }

        public void Button_NeonGlow()
        {
        }
    }
}