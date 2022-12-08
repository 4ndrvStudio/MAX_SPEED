namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections.Generic;

    public class VehicleCustomizationMenu : ScreenBase
    {
        public static VehicleCustomizationMenu Instance = null;

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
            VehicleSelectionMenu.Instance.Open();
        }

        public void Button_BodyWrap()
        {
            Close();
            VehicleBodyWrapMenu.Instance.Open();
        }

        public void Button_BodyPaint()
        {
            Close();
            VehicleBodyPaintMenu.Instance.Open();
        }

        public void Button_WindowTint()
        {
            Close();
            VehicleWindowTintMenu.Instance.Open();
        }

        public void Button_RimColor()
        {
            Close();
            VehicleRimPaintMenu.Instance.Open();
        }

        public void Button_NeonLight()
        {
            Close();
            VehicleNeonLightMenu.Instance.Open();
        }
    }
}