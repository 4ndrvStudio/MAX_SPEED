namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;

    public class DisableObjectOnRaceComplete : MonoBehaviour
    {
        void Start()
        {
            if (RaceManager.Instance != null)
            {
                RaceManager.Instance.disableObjectsOnRaceComplete.Add(this.gameObject);
            }
        }
    }
}