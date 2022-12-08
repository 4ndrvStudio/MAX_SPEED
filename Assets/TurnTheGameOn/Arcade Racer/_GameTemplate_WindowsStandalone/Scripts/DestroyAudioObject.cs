namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;
    [RequireComponent(typeof(AudioSource))]
    public class DestroyAudioObject : MonoBehaviour
    {
        private AudioSource audioRef;

        void Start()
        {
            audioRef = GetComponent<AudioSource>();
        }

        void Update()
        {
            if (audioRef.isPlaying)
            {
            }
            else
            {
                if (PauseScreen.Instance != null)
                {
                    if (PauseScreen.Instance.isPaused)
                    {
                    }
                    else
                    {
                        Destroy(gameObject);
                    }
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}