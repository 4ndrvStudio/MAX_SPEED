namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;

    public class RotateTransform : MonoBehaviour
    {
        private Transform _transform;
        public Vector3 speed;
        public float manualSpeed;
        public CarPlayerInputSettings playerInputSettings;

        private void Start()
        {
            _transform = transform;
        }

        void Update()
        {
            _transform.Rotate(speed.x * Time.deltaTime, speed.y * Time.deltaTime + (Input.GetAxis(playerInputSettings.inputAxes.horizontalCameraRotation) * manualSpeed), speed.z * Time.deltaTime, Space.Self);
        }
    }
}