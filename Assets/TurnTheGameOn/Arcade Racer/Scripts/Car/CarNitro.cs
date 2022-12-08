namespace TurnTheGameOn.ArcadeRacer
{
    using System.Collections;
    using UnityEngine;

    public class CarNitro : MonoBehaviour
    {
        public GameObject nitroEffectObject;
        private CarDriveSystem driveSystem;
        private bool canUpdateNitroAmount;
        public bool nitroOn;
        public float nitroAmount;
        public float nitroSpendRate = 1;
        public float nitroRefillRate = 0.3f;
        public float nitroDuration = 10;
        public bool enableNitro = true;
        public float nitroTopSpeed = 20;
        public float nitroFullTorque = 4000;


        [ContextMenu("NiroON")]
        public virtual void NitroOn()
        {
            if (enableNitro && !nitroOn && nitroAmount > 2.0f)
            {
                nitroEffectObject.SetActive(true);
                driveSystem.topSpeed = driveSystem.vehicleSettings.topSpeed + nitroTopSpeed;
                driveSystem._vehicleSettings.fullTorqueOverAllWheels = driveSystem._vehicleSettings.fullTorqueOverAllWheels + nitroFullTorque;
                nitroOn = true;
            }
        }

        [ContextMenu("NiroOFF")]
        public virtual void NitroOff()
        {
            if (nitroOn && enableNitro)
            {
                nitroEffectObject.SetActive(false);
                driveSystem.topSpeed = driveSystem.vehicleSettings.topSpeed - nitroTopSpeed;
                driveSystem._vehicleSettings.fullTorqueOverAllWheels = driveSystem._vehicleSettings.fullTorqueOverAllWheels - nitroFullTorque;
                nitroOn = false;
            }
        }

        public virtual void Start()
        {
            if (!driveSystem) driveSystem = GetComponent<CarDriveSystem>();
            if (enableNitro)
            {
                nitroAmount = nitroDuration;
                StartCoroutine("UpdateNitroAmount");
            }
        }

        void OnDisable()
        {
            canUpdateNitroAmount = false;
            StopAllCoroutines();
        }

        IEnumerator UpdateNitroAmount()
        {
            canUpdateNitroAmount = true;
            while (canUpdateNitroAmount)
            {
                if (!nitroOn && nitroAmount < nitroDuration)
                {
                    nitroAmount += nitroRefillRate * Time.deltaTime;
                    if (nitroAmount > nitroDuration) nitroAmount = nitroDuration;
                }
                else
                {
                    nitroAmount -= nitroSpendRate * Time.deltaTime;
                    if (nitroAmount < 0)
                    {
                        nitroAmount = 0;
                        NitroOff();
                    }
                }
                yield return null;
            }
        }
    }
}