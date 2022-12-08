namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;

    public class CustomMaterial : MonoBehaviour
    {
        void Start()
        {
            if (CustomMaterialManager.Instance != null)
            {
                MeshRenderer mesh = GetComponent<MeshRenderer>();
                mesh.material = CustomMaterialManager.Instance.GetMaterial();
            }
            Destroy(this);
        }
    }
}