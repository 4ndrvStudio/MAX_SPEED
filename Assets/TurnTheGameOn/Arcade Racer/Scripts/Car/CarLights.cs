namespace TurnTheGameOn.ArcadeRacer
{
	using UnityEngine;

	public class CarLights : MonoBehaviour
	{		
		public GameObject brakeLightsObject;
		public GameObject reverseLightsObject;
		private CarDriveSystem driveSystem;
        public Light headLightL;
        public Light headLightR;
        public Light headLightFlareL;
        public Light headLightFlareR;
        public Light tailLightL;
        public Light tailLightR;

        void OnEnable ()
		{
			if (!driveSystem) driveSystem = GetComponent <CarDriveSystem>();
            driveSystem.OnSetIsBraking += OnSetIsBraking;
            driveSystem.OnSetIsNotBraking += OnSetIsNotBraking;
            driveSystem.OnSetIsReversing += OnSetIsReversing;
            driveSystem.OnSetIsNotReversing += OnSetIsNotReversing;
		}

		void OnDisable ()
		{
            driveSystem.OnSetIsBraking -= OnSetIsBraking;
            driveSystem.OnSetIsNotBraking -= OnSetIsNotBraking;
            driveSystem.OnSetIsReversing -= OnSetIsReversing;
            driveSystem.OnSetIsNotReversing -= OnSetIsNotReversing;
		}

        void OnSetIsBraking ()
		{
			TurnOnBrakeLights();
		}

		void OnSetIsNotBraking ()
		{
			TurnOffBrakeLights();
		}

		void OnSetIsReversing ()
		{
			TurnOnReverseLights();
		}

		void OnSetIsNotReversing ()
		{
			TurnOffReverseLights();
		}

		void TurnOff()
		{
			TurnOffReverseLights ();
			TurnOffBrakeLights ();
		}

		void TurnOnBrakeLights ()
		{
			TurnOffReverseLights ();
			brakeLightsObject.SetActive(true);
		}

		void TurnOffBrakeLights ()
		{
			brakeLightsObject.SetActive(false);
		}

		void TurnOnReverseLights ()
		{
			TurnOffBrakeLights ();
			reverseLightsObject.SetActive(true);
		}

		void TurnOffReverseLights ()
		{
			reverseLightsObject.SetActive(false);
		}

		public void EnableHeadLights ()
        {
			headLightL.enabled = true;
			headLightR.enabled = true;
			headLightFlareL.enabled = true;
			headLightFlareR.enabled = true;
		}

		public void DisableHeadLights ()
        {
			headLightL.enabled = false;
			headLightR.enabled = false;
			headLightFlareL.enabled = false;
			headLightFlareR.enabled = false;
		}
		
	}
}