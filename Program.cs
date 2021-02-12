using Appwrite;
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
            var analytics = new Dictionary<string, object>();

            analytics.Add("letterCount", result.LetterCount);
            analytics.Add("wordCount", result.WordCount);
            analytics.Add("sentenceCount", result.SentenceCount);
            analytics.Add("colemanLiauIndex", result.ColemanLiauIndex());
            analytics.Add("fleschKincaidGradeLevel", result.FleschKincaidGradeLevel());
            analytics.Add("fleschKincaidReadingEase", result.FleschKincaidReadingEase());
            analytics.Add("gunningFogScore", result.GunningFogScore());
            analytics.Add("smogIndex", result.SMOGIndex());
            analytics.Add("readingTime", result.ReadingTime());
            analytics.Add("speakingTime", result.SpeakingTime());

            var options = new JsonSerializerOptions()
            {
                WriteIndented = true
            };
            update(document.Id, document.Collection, analytics);
            Console.WriteLine(JsonSerializer.Serialize(analytics, options));
        }
        static async void update(string document, string collection, Dictionary<string, object> data)
        {
            var client = new Client();
            client
            .SetEndPoint(Environment.GetEnvironmentVariable("ENDPOINT"))
            .SetProject(Environment.GetEnvironmentVariable("PROJECT"))
            .SetKey(Environment.GetEnvironmentVariable("KEY"));
            var database = new Database(client);
            var list = new List<object>();
            await database.UpdateDocument(document, collection, data, list, list);
        }
    }

    public class Document
    {
        [JsonPropertyNameAttribute("$id")]
        public string Id { get; set; }
        [JsonPropertyNameAttribute("$collection")]
        public string Collection { get; set; }
        [JsonPropertyNameAttribute("content")]
        public string Content { get; set; }

    }
}
