using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace TimeTagger
{
    class Program
    {
        private const string filePath = "storage.json";

        static void Main(string[] args)
        {
            TimeChunkStorage storage = null;

            if (File.Exists(filePath))
                storage = TimeChunkStorage.FromJson(File.ReadAllText(filePath));
            else
                storage = new TimeChunkStorage();

            while(true)
            {
                Console.Clear();

                foreach (DateTime date in storage.TimeChunks.Keys)
                {
                    Console.WriteLine(date.ToString().Split()[0] + " " + date.DayOfWeek);

                    foreach (string key in storage.TimeChunks[date].Keys)
                    {
                        double sumSeconds = 0;
                        foreach (TimeChunk chunk in storage.TimeChunks[date][key])
                            sumSeconds += chunk.DurationSeconds;

                        Console.WriteLine(string.Format("{0}: {1}", key, TimeChunk.GetFormattedString(sumSeconds)));
                    }

                    Console.WriteLine();
                }

                Console.WriteLine("\nStart a new chunk with a tag: ");
                string tagInput = Console.ReadLine();

                TimeChunk currentChunk = new TimeChunk(tagInput);

                CancellationTokenSource tokenSource = new CancellationTokenSource();
                Task printTask = Task.Run(() => { PrintTime(currentChunk, tokenSource.Token); }, tokenSource.Token);

                Console.WriteLine("Press enter to stop current chunk");
                Console.ReadLine();

                currentChunk.Stop();
                tokenSource.Cancel();

                storage.AddChunk(currentChunk);
                File.WriteAllText(filePath, storage.ToJson());
            }
        }

        private static void PrintTime(TimeChunk timeChunk, CancellationToken cancellationToken)
        {
            while(!cancellationToken.IsCancellationRequested)
            {
                Console.Clear();
                Console.WriteLine("Current chunk: " + timeChunk.Tag);
                Console.WriteLine(TimeChunk.GetFormattedString(timeChunk.DurationSeconds));
                Thread.Sleep(1000);
            }
        }
    }
}
