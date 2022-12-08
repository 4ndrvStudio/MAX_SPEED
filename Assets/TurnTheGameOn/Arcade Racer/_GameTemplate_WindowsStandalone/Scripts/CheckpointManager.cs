namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;
    using System.Collections.Generic;

    public class CheckpointManager : MonoBehaviour
    {
        public static CheckpointManager Instance;
        public List<Checkpoint> checkpoints;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        public void ResetAllDisabledCheckpoints()
        {
            for (int i = 0; i < checkpoints.Count; i++)
            {
                checkpoints[i].gameObject.SetActive(true);
            }
        }
    }
}