namespace TurnTheGameOn.Timer
{
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.Events;
    using System;
    using UnityEngine.SceneManagement;

    [System.Serializable]
    public class DisplayOptions
    {
        public bool milliseconds = true, seconds = true, minutes = true, hours = true, days = true;
    }
    [System.Serializable]
    public class TimerFormat
    {
        public bool milliseconds = true, seconds = true, minutes = true, hours = true, days = true;
    }
    [System.Serializable]
    public class LeadingZero
    {
        public bool seconds = true, minutes = true, hours = true, days = true;
    }

    [ExecuteInEditMode]
    public class Timer : MonoBehaviour
    {
        public enum TimerType { CountUp, CountDown, CountUpInfinite }
        public enum LoadSceneOn { Disabled, TimesUp, SpecificTime }
        public enum TimerState { Disabled, Counting }
        public enum TextType { DefaultText, TextMeshProUGUI }
        public DisplayOptions displayOptions;
        public TimerFormat timerFormat;
        public LeadingZero leadingZero;

        private string ms, s, m, d, h;

        public TimerState timerState;
        public bool useSystemTime;
        private DateTime systemDateTime;
        public double gameTime;
        public TextType textType;
        public Text timerTextDefault;
        public TextMeshProUGUI timerTextTMPUGUI;
        public float timerSpeed = 1;
        public int day;
        public int hour;
        public int minute;
        public int second;
        public int millisecond;

        public bool setZeroTimescale;
        public bool setStartTimeForCountUp;
        public UnityEvent timesUpEvent;
        public GameObject[] destroyOnFinish;
        [HideInInspector()] public TimerType timerType;
        [HideInInspector()] public double startTime;
        [HideInInspector()] public double finishTime;
        [HideInInspector()] public LoadSceneOn loadSceneOn;
        [HideInInspector()] public string loadSceneName;
        [HideInInspector()] public double timeToLoadScene;

        string FormatSeconds(double elapsed)
        {
            if (timerType == TimerType.CountUpInfinite)
            {
                if (useSystemTime)
                {
                    CheckDisplayOptions();
                    gameTime = ((double)DateTime.Now.Hour + ((double)DateTime.Now.Minute) + (double)DateTime.Now.Second);
                    millisecond = (int)DateTime.Now.Millisecond;
                    second = (int)DateTime.Now.Second;
                    minute = (int)DateTime.Now.Minute;
                    hour = (int)DateTime.Now.Hour;
                    day = (int)DateTime.Now.DayOfYear;
                    return String.Format(d + h + m + s + ms, day, hour, minute, second, millisecond);
                }
                else
                {
                    CheckDisplayOptions();
                    TimeSpan t = TimeSpan.FromSeconds(elapsed);
                    day = t.Days;
                    hour = t.Hours;
                    minute = t.Minutes;
                    second = t.Seconds;
                    millisecond = t.Milliseconds;
                    return String.Format(d + h + m + s + ms, t.Days, t.Hours, t.Minutes, t.Seconds, t.Milliseconds);
                }
            }
            else
            {
                TimeSpan t = TimeSpan.FromSeconds(elapsed);
                CheckTimerOptions();
                day = t.Days;
                hour = t.Hours;
                minute = t.Minutes;
                second = t.Seconds;
                millisecond = t.Milliseconds;
                if (!leadingZero.seconds && t.Seconds < 10d && s == "{3:D2}") s = "{3:D1}";
                if (!leadingZero.minutes && t.Minutes < 10d && m == "{2:D2}:") m = "{2:D1}:";
                if (!leadingZero.hours && t.Hours < 10d && h == "{1:D2}:") h = "{1:D1}:";
                if (!leadingZero.days && t.Days < 10d && (d == "{0:D3}:" || d == "{0:D2}:")) d = "{0:D1}:";
                else if (!leadingZero.days && t.Days < 100d && (d == "{0:D3}:" || d == "{0:D1}:")) d = "{0:D2}:";
                return String.Format(d + h + m + s + ms, t.Days, t.Hours, t.Minutes, t.Seconds, t.Milliseconds);
            }
        }

        void UpdateUIText()
        {
            switch (textType)
            {
                case TextType.DefaultText:
                    if (timerTextDefault != null) timerTextDefault.text = FormatSeconds(gameTime);
                    break;
                case TextType.TextMeshProUGUI:
                    if (timerTextTMPUGUI != null) timerTextTMPUGUI.text = FormatSeconds(gameTime);
                    break;
            }
        }

        void CheckDisplayOptions()
        {
            ms = displayOptions.milliseconds ? ".{4:D3}" : "";
            s = displayOptions.seconds ? "{3:D2}" : "";
            m = displayOptions.minutes ? "{2:D2}:" : "";
            h = displayOptions.hours ? "{1:D2}:" : "";
            d = displayOptions.days ? "{0:D3}:" : "";
        }

        void CheckTimerOptions()
        {
            ms = timerFormat.milliseconds ? ".{4:D3}" : "";
            s = timerFormat.seconds ? "{3:D2}" : "";
            m = timerFormat.minutes ? "{2:D2}:" : "";
            h = timerFormat.hours ? "{1:D2}:" : "";
            d = timerFormat.days ? "{0:D0}:" : "";
        }

        void Start()
        {
            ClampGameTime();
            if (timerState == TimerState.Counting)
            {
                UpdateUIText();
                if (timerType == TimerType.CountUp || timerType == TimerType.CountUpInfinite) gameTime = setStartTimeForCountUp ? startTime : 0;
                if (timerType == TimerType.CountDown) gameTime = startTime;
            }
        }

        void Update()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                ClampGameTime();
                if (timerType == TimerType.CountUp) gameTime = 0;
                if (timerType == TimerType.CountDown) gameTime = (double)startTime;
                UpdateUIText();
            }
#endif
            if (timerTextDefault != null)
            {
                timerTextDefault.text = FormatSeconds(gameTime);
            }
            else if (useSystemTime)
            {
                gameTime = ((double)DateTime.Now.Hour + ((double)DateTime.Now.Minute) + (double)DateTime.Now.Second);
                second = (int)DateTime.Now.Second;
                minute = (int)DateTime.Now.Minute;
                hour = (int)DateTime.Now.Hour;
            }
            if (timerState == TimerState.Counting)
            {
                if (timerType == TimerType.CountUp)
                {
                    gameTime += 1 * (double)Time.deltaTime * (double)timerSpeed;
                    if (gameTime >= timeToLoadScene)
                    {
                        if (loadSceneOn == LoadSceneOn.SpecificTime) SceneManager.LoadScene(loadSceneName);
                    }
                }
                if (timerType == TimerType.CountDown)
                {
                    gameTime -= 1 * (double)Time.deltaTime * (double)timerSpeed;
                    if (gameTime <= timeToLoadScene)
                    {
                        if (loadSceneOn == LoadSceneOn.SpecificTime) SceneManager.LoadScene(loadSceneName);
                    }
                }
                if (timerType == TimerType.CountUpInfinite)
                {
                    if (!useSystemTime) gameTime += 1 * (double)Time.deltaTime * (double)timerSpeed;
                }
                if (timerType == TimerType.CountDown && gameTime <= 0)
                {
                    StopTimer();
                    timesUpEvent.Invoke();
                    for (int i = 0; i < destroyOnFinish.Length; i++) Destroy(destroyOnFinish[i]);
                    if (loadSceneOn == LoadSceneOn.TimesUp) SceneManager.LoadScene(loadSceneName);
                }
                if (timerType == TimerType.CountUp && gameTime >= finishTime)
                {
                    timesUpEvent.Invoke();
                    StopTimer();
                    for (int i = 0; i < destroyOnFinish.Length; i++) Destroy(destroyOnFinish[i]);
                    if (loadSceneOn == LoadSceneOn.TimesUp) SceneManager.LoadScene(loadSceneName);
                }
                UpdateUIText();
            }
        }

        public double GetTimerValue() { return gameTime; }
        public void AddTime(float value) { gameTime += (double)value; }

        [ContextMenu("Start Timer")] public void StartTimer() { timerState = TimerState.Counting; }

        [ContextMenu("Stop Timer")]
        public void StopTimer()
        {
            timerState = TimerState.Disabled;
            if (setZeroTimescale) Time.timeScale = 0;
            if (gameTime < 0.0f) gameTime = 0.0f;
            UpdateUIText();
        }

        [ContextMenu("Restart Timer")]
        public void RestartTimer()
        {
            if (timerType == TimerType.CountDown) gameTime = (double)startTime;
            else gameTime = 0;
            UpdateUIText();
            StartTimer();
        }

        [ContextMenu("Reset Timer")]
        public void ResetTimer()
        {
            if (timerType == TimerType.CountDown) gameTime = (double)startTime;
            else gameTime = 0;
            UpdateUIText();
        }

        void ClampGameTime()
        {
            if (startTime > 922337193600)
            {
                startTime = 922337193600;
                Debug.LogWarning("startTime exceeds max allowed value, it has been clamped to max value");
            }
            if (finishTime > 922337193600)
            {
                finishTime = 922337193600;
                Debug.LogWarning("finishTime exceeds max allowed value, it has been clamped to max value");
            }
            if (gameTime > 922337193600)
            {
                gameTime = 922337193600;
                Debug.LogWarning("gameTime exceeds max allowed value, it has been clamped to max value");
            }
        }

    }
}