namespace TurnTheGameOn.ArcadeRacer
{
    using System;

    public static class FormatGameTime
    {
        public static string ConvertFromSeconds(double seconds)
        {
            TimeSpan interval = TimeSpan.FromSeconds(seconds);
            string timeInterval = string.Format("{0:D2}:{1:D2}", interval.Minutes, interval.Seconds);
            string timeString = timeInterval + "." + interval.Milliseconds.ToString("D3");
            return timeString;
        }
    }
}