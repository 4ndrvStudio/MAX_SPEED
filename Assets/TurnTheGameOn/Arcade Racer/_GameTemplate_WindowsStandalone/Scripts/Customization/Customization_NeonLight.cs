namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;
    
    [RequireComponent(typeof(ParticleSystem))]
    public class Customization_NeonLight : MonoBehaviour
    {
        void OnEnable()
        {
            ParticleSystem ps = GetComponent<ParticleSystem>();
            GameTemplate_PlayerCustomization.Instance.RegisterNeonLight(ps);
        }
    }
}