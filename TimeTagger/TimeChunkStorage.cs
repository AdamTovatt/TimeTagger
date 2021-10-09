using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TimeTagger
{
    public class TimeChunkStorage
    {
        [JsonProperty("timeChunks")]
        public Dictionary<DateTime, Dictionary<string, List<TimeChunk>>> TimeChunks { get; private set; }

        public TimeChunkStorage()
        {
            TimeChunks = new Dictionary<DateTime, Dictionary<string, List<TimeChunk>>>();
        }

        public void AddChunk(TimeChunk timeChunk)
        {
            if (timeChunk.EndTime == default(DateTime))
                timeChunk.Stop();

            DateTime date = timeChunk.EndTime.Date;

            if (!TimeChunks.ContainsKey(date))
                TimeChunks.Add(date, new Dictionary<string, List<TimeChunk>>());

            if (!TimeChunks[date].ContainsKey(timeChunk.Tag))
                TimeChunks[date].Add(timeChunk.Tag, new List<TimeChunk>());

            TimeChunks[date][timeChunk.Tag].Add(timeChunk);
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static TimeChunkStorage FromJson(string json)
        {
            return JsonConvert.DeserializeObject<TimeChunkStorage>(json);
        }
    }
}
