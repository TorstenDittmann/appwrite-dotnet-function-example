using Appwrite;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

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
            var client = new Client();
            client
            .SetEndPoint(Environment.GetEnvironmentVariable("ENDPOINT"))
            .SetProject(Environment.GetEnvironmentVariable("PROJECT"))
            .SetKey(Environment.GetEnvironmentVariable("KEY"));
            var database = new Database(client);
            var list = new List<object>();
            database.UpdateDocument(document.Id, document.Collection, analytics, list, list);
            try
            {
                Console.WriteLine(Environment.GetEnvironmentVariable("PROJECT"));
                var response = RunTask(database.UpdateDocument(document.Id, document.Collection, analytics, list, list)).GetAwaiter().GetResult();
                Console.WriteLine(response);
                Console.WriteLine(JsonSerializer.Serialize(analytics, options));

            }
            catch (System.Exception e)
            {
                Console.WriteLine($"Error: {e}");
                throw;
            }
        }
        static async Task<string> RunTask(Task<HttpResponseMessage> task) 
        {
            HttpResponseMessage response = await task;
            return await response.Content.ReadAsStringAsync();
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
