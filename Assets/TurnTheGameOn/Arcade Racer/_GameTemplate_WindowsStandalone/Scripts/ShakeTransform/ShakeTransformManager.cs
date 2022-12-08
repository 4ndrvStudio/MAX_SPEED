namespace TurnTheGameOn.ArcadeRacer
{
    using System.Collections.Generic;
    using UnityEngine;

    public class ShakeTransformManager : MonoBehaviour
    {
        public static ShakeTransformManager Instance;
        public List<ShakeTransform> shakeTransforms;
        public float maxIntensity = 50;
        public AnimationCurve intensityCurve;
        public ShakeSettings defaultShakeData;
        private ShakeSettings shakeEventData;
        private float collisionIntensity;

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

            shakeEventData = ScriptableObject.CreateInstance<ShakeSettings>();
            shakeEventData.Init(
                defaultShakeData.amplitude,
                defaultShakeData.frequency,
                defaultShakeData.duration,
                defaultShakeData.blendOverLifetime,
                defaultShakeData.target);
        }

        public void DoDefaultShake(float intensity)
        {
            collisionIntensity = Mathf.Clamp(intensity, 0, maxIntensity);
            shakeEventData.amplitude = (defaultShakeData.amplitude * collisionIntensity) * intensityCurve.Evaluate(collisionIntensity/maxIntensity);
            if (defaultShakeData != null)
            {
                for (int i = 0; i < shakeTransforms.Count; i++)
                {
                    if (shakeTransforms[i] != null)
                    {
                        shakeTransforms[i].AddShakeEvent(shakeEventData);
                    }
                }
            }
        }

    }
}