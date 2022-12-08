namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;

    [ExecuteInEditMode][System.Serializable]
	public class CarAISensor : MonoBehaviour
    {

		public bool hit { get; private set; }
		public float hitDistance { get; private set; }
		public string hitTag { get; private set; }
		public string hitLayer { get; private set; }
		public Transform hitTransform { get; private set; }
		// options
		public Color detectColor;
		public Color normalColor;
		// set through custom inspector
		[HideInInspector] public LayerMask layerMask;
		[HideInInspector] public Vector3 boxSize;
		[HideInInspector] public int number;
		[HideInInspector] public float updateInterval;
		[HideInInspector] public float height;
		[HideInInspector] public float length;
		[HideInInspector] public float width;
		[HideInInspector] public float centerWidth;
		// cached to prevent gc
		[HideInInspector] private Vector3 origin;
		[HideInInspector] private Vector3 direction;
		[HideInInspector] private Quaternion rotation;
		[HideInInspector] private Vector3 sensorPosition;
		[HideInInspector] private RaycastHit boxHit;
		[HideInInspector] private Color color;
		[HideInInspector] private Vector3 offset;
		private Transform transformCached;
		[HideInInspector] private float updateIntervalTimer;
        public bool showEditorGizmos;
        [HideInInspector] private Transform previousHitTransform;

        private void Awake()
        {
            transformCached = transform;
            ResetHitBox();
        }
        void Update ()
		{
			updateIntervalTimer -= Time.deltaTime;
			if (updateIntervalTimer <= 0.0)
            {
				updateIntervalTimer = updateInterval;
				BoxCast ();					
			}
		}

		void BoxCast ()
		{
			origin = transformCached.position;
			direction = transformCached.forward;
			rotation = transformCached.rotation;
			if (Physics.BoxCast (origin, boxSize, direction, out boxHit, rotation, length, layerMask, QueryTriggerInteraction.UseGlobal))
            {
                hitTransform = boxHit.transform; // cache transform lookup
                if (hitTransform != previousHitTransform)
                {
                    previousHitTransform = hitTransform;
                    hitTag = hitTransform.tag;
                    hitLayer = LayerMask.LayerToName(hitTransform.gameObject.layer);
                }
                hitDistance = boxHit.distance;
				hit = true;
			}
			else
            {
                if (hit != false)
                {
                    ResetHitBox();
                }
			}
		}

        void ResetHitBox()
        {
            hitDistance = length;
            hit = false;
            hitLayer = "";
            hitTag = "";
        }

		public void OnDrawGizmos ()
		{
            if (showEditorGizmos)
            {
                if (hitDistance == length)
                {
                    color = normalColor;
                }
                else
                {
                    color = detectColor;
                }
                Gizmos.color = color;
                offset = new Vector3(boxSize.x * 2.0f, boxSize.y * 2.0f, hitDistance);
                DrawCube(origin + direction * (hitDistance / 2), transformCached.rotation, offset);
            }
		}

		public static void DrawCube(Vector3 position, Quaternion rotation, Vector3 scale)
		{
			Matrix4x4 cubeTransform = Matrix4x4.TRS(position, rotation, scale);
			Matrix4x4 oldGizmosMatrix = Gizmos.matrix;
			Gizmos.matrix *= cubeTransform;
			Gizmos.DrawCube(Vector3.zero, Vector3.one);
			Gizmos.matrix = oldGizmosMatrix;
		}

		public void UpdateSensorTransform ()
		{
			boxSize.x = width;
			boxSize.y = height;
			switch (number)
			{
			case 0:
                    sensorPosition = transformCached.localPosition;
                    sensorPosition.x = ((centerWidth + 0.01f) * -2.0f) - (centerWidth + 0.01f) - ((width + 0.03f) * 5);
                    transformCached.localPosition = sensorPosition;
				break;
			case 1:
				    sensorPosition = transformCached.localPosition;
				    sensorPosition.x = ((centerWidth + 0.01f) * -2.0f) - (centerWidth + 0.01f) - ((width + 0.02f) * 3);
                    transformCached.localPosition = sensorPosition;
				break;
			case 2:
				    sensorPosition = transformCached.localPosition;
				    sensorPosition.x = ((centerWidth + 0.01f) * -2.0f) - (centerWidth + 0.01f) - (width + 0.01f);
                    transformCached.localPosition = sensorPosition;
				break;
			case 3:
				    sensorPosition = transformCached.localPosition;
				    sensorPosition.x = ((centerWidth + 0.01f) * -2.0f);
                    transformCached.localPosition = sensorPosition;
				break;
			case 4:
				    sensorPosition = transformCached.localPosition;
				    sensorPosition.x = 0;
                    transformCached.localPosition = sensorPosition;
				break;
			case 5:
				    sensorPosition = transformCached.localPosition;
				    sensorPosition.x = (centerWidth + 0.01f) * 2.0f;
                    transformCached.localPosition = sensorPosition;
				break;
			case 6:
				    sensorPosition = transformCached.localPosition;
				    sensorPosition.x = ((centerWidth + 0.01f) * 2.0f) + (centerWidth + 0.01f) + (width + 0.01f);
                    transformCached.localPosition = sensorPosition;
				break;
			case 7:
				    sensorPosition = transformCached.localPosition;
				    sensorPosition.x = ((centerWidth + 0.01f) * 2.0f) + (centerWidth + 0.01f) + ((width + 0.02f) * 3);
                    transformCached.localPosition = sensorPosition;
				break;
			case 8:
				    sensorPosition = transformCached.localPosition;
				    sensorPosition.x = ((centerWidth + 0.01f) * 2.0f) + (centerWidth + 0.01f) + ((width + 0.03f) * 5);
                    transformCached.localPosition = sensorPosition;
				break;
			}
		}

	}
}