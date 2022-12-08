namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;

    public class MiniMapCamera : MonoBehaviour
    {
        public Camera miniMapCamera { get; private set; }
        public bool fixedRotation;
        public Transform target;
        public Vector3 offset = new Vector3(0f, 7.5f, 0f);
        public bool followTarget;
        private bool forceFixedPositionAndRotation;
        private Transform transformCached;
        private Vector3 newPositionCached;

        private void OnEnable()
        {
            miniMapCamera = GetComponent<Camera>();
            transformCached = transform;
        }

        void Start()
        {
            if (target == null)
            {
                Debug.LogWarning("MiniMapCamera target is not set, this script will be disabled.");
                enabled = false;
            }
            transform.parent = null;
        }

        private void LateUpdate()
        {
            if (forceFixedPositionAndRotation)
            {
                fixedRotation = true;
            }
            else
            {
                if (fixedRotation)
                {
                    transformCached.position = target.position + offset;
                    transformCached.LookAt(target);
                }
                else
                {
                    newPositionCached.x = 90;
                    newPositionCached.y = target.rotation.eulerAngles.y;
                    newPositionCached.z = 0;
                    transformCached.eulerAngles = newPositionCached;
                    transformCached.position = target.position + offset;
                }
            }
        }

        public void SetFixedPositionAndRotation(Vector3 _position, Vector3 _rotation, float _cameraSize)
        {
            forceFixedPositionAndRotation = true;
            followTarget = false;
            transformCached.position = _position;
            transformCached.eulerAngles = _rotation;
            miniMapCamera.orthographicSize = _cameraSize;
        }

    }
}