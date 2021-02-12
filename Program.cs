using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace appwrite_dotnet_function_example
{
    class Program
    {
        static void Main(string[] args)
        {
            var document = JsonSerializer.Deserialize<Document>(Environment.GetEnvironmentVariable("APPWRITE_FUNCTION_EVENT_PAYLOAD"));
            var result = TextStatistics.TextStatistics.Parse(document.Content);
            var analytics = new Dictionary<string, double>();
            analytics.Add("LetterCount", result.LetterCount);
            analytics.Add("WordCount", result.WordCount);
            analytics.Add("SentenceCount", result.SentenceCount);
            analytics.Add("ColemanLiauIndex", result.ColemanLiauIndex());
            analytics.Add("FleschKincaidGradeLevel", result.FleschKincaidGradeLevel());
            analytics.Add("FleschKincaidReadingEase", result.FleschKincaidReadingEase());
            analytics.Add("GunningFogScore", result.GunningFogScore());
            analytics.Add("SMOGIndex", result.SMOGIndex());
            analytics.Add("ReadingTime", result.ReadingTime());
            analytics.Add("SpeakingTime", result.SpeakingTime());

            var options = new JsonSerializerOptions() {
                WriteIndented = true
            };
            Console.WriteLine(JsonSerializer.Serialize(analytics, options));
        }
    }

    public class Document
    {
        [JsonPropertyNameAttribute("content")]
        public string Content { get; set; }

    }
}
