namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;

    public class Checkpoint : MonoBehaviour
    {
        public int addTime;

        private void Start()
        {
            if (RaceManager.Instance.raceData.useDefeatTimer == false)
            {
                gameObject.SetActive(false);
            }
            else
            {
                CheckpointManager.Instance.checkpoints.Add(this);
            }
        }

        void OnTriggerEnter(Collider col)
        {
            if (col.transform.root.name == GameTemplate_Player.Instance.name)
            {
                RaceCountDownTimer.Instance.AddTime(addTime);
                gameObject.SetActive(false);
            }
        }
    }
}