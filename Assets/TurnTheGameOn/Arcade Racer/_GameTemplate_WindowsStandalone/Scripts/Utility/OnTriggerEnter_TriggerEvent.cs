namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;
    using UnityEngine.Events;

    public class OnTriggerEnter_TriggerEvent : MonoBehaviour
    {
        public string triggerTag = "Player";
        public UnityEvent triggerEnterEvent;

        public void OnTriggerEnter(Collider other)
        {
            if (other.tag == triggerTag)
            {
                if (triggerEnterEvent != null)
                {
                    triggerEnterEvent.Invoke();
                }
            }
        }
    }
}