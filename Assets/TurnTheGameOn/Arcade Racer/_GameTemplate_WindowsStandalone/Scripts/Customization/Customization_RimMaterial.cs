namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;
    
    [RequireComponent(typeof(MeshRenderer))]
    public class Customization_RimMaterial : MonoBehaviour
    {
        public int materialIndex;
        void OnEnable()
        {
            MeshRenderer meshRend = GetComponent<MeshRenderer>();
            GameTemplate_PlayerCustomization.Instance.RegisterRimMesh(meshRend, materialIndex);
        }
    }
}