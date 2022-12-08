namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;

    public class ShakeTransform_OnCollisionTrigger : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            float impactMagnitude = collision.relativeVelocity.magnitude;
            ShakeTransformManager.Instance.DoDefaultShake(impactMagnitude);
        }
    }
}