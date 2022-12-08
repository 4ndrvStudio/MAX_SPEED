namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;
    using TMPro;
    using UnityEngine.Events;
    using UnityEngine.SceneManagement;

    public class RaceCountDownTimer : MonoBehaviour
    {
        public static RaceCountDownTimer Instance;
        public TextMeshProUGUI addTimeText;
        public TextMeshProUGUI countDownTinerText;
        private float timer;
        private bool countDown;
        public UnityEvent onPlayerLostEvent;
        public string playerLostScene;

        void OnEnable()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            if (countDown)
            {
                timer -= Time.deltaTime;
                if (timer <= 0.0f)
                {
                    timer = 0.0f;
                    StopTimer();
                    //PlayerLost();
                    SeriesManager.Instance.LoadDefeatScene(); 
                }
                countDownTinerText.text = timer.ToString("F0");
            }
        }

        void PlayerLost()
        {
            GameTemplate_Player.Instance.DisableControl();
            onPlayerLostEvent.Invoke();
        }

        public void InitializeTimer(float t)
        {
            timer = t;
            countDownTinerText.text = timer.ToString("F0");
            HideAddTimeText();
        }

        public void StartTimer()
        {
            countDown = true;
        }

        public void StopTimer()
        {
            countDown = false;
        }

        public void AddTime(float t)
        {
            timer += t;
            addTimeText.text = "+" + t.ToString();
            Invoke("HideAddTimeText", 2.0f);
        }

        public void SubtractTime(float t)
        {
            timer -= t;
            addTimeText.text = "-" + t.ToString();
            Invoke("HideAddTimeText", 2.0f);
        }

        public void HideAddTimeText()
        {
            addTimeText.text = "";
        }
    }
}