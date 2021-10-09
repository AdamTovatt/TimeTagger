using Newtonsoft.Json;
using System;

namespace TimeTagger
{
    public class TimeChunk
    {
        [JsonProperty("startTime")]
        public DateTime StartTime { get; private set; }
        [JsonProperty("endTime")]
        public DateTime EndTime { get; private set; }
        [JsonProperty("tag")]
        public string Tag { get; private set; }

        [JsonIgnore]
        public double DurationSeconds { get { if (EndTime == default(DateTime)) return (DateTime.Now - StartTime).TotalSeconds; else return (EndTime - StartTime).TotalSeconds; } }

        public TimeChunk() { }

        public TimeChunk(string tag)
        {
            Tag = tag.ToLower();
            StartTime = DateTime.Now;
        }

        public void Stop()
        {
            EndTime = DateTime.Now;
        }

        public static string GetFormattedString(double seconds)
        {
            TimeSpan duration = TimeSpan.FromSeconds(seconds);
            
            if(duration.Days < 1)
                return string.Format("{0} hours, {1} minutes, {2} seconds", duration.Hours, duration.Minutes, duration.Seconds);
            else
                return string.Format("{3} days, {0} hours, {1} minutes, {2} seconds", duration.Hours, duration.Minutes, duration.Seconds, duration.Days);
        }
    }
}
