namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;

    public class OnTriggerEnter_FinishRace : MonoBehaviour
    {
        public string triggerTag = "Player";

        public void OnTriggerEnter(Collider other)
        {
            if (other.tag == triggerTag)
            {
                RaceManager.Instance.ForceFinishRace();
            }
        }
    }
}