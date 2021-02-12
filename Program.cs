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
            var analytics = new Dictionary<string, string>();

            analytics.Add("letterCount", result.LetterCount.ToString());
            analytics.Add("wordCount", result.WordCount.ToString());
            analytics.Add("sentenceCount", result.SentenceCount.ToString());
            analytics.Add("colemanLiauIndex", result.ColemanLiauIndex().ToString());
            analytics.Add("fleschKincaidGradeLevel", result.FleschKincaidGradeLevel().ToString());
            analytics.Add("fleschKincaidReadingEase", result.FleschKincaidReadingEase().ToString());
            analytics.Add("gunningFogScore", result.GunningFogScore().ToString());
            analytics.Add("smogIndex", result.SMOGIndex().ToString());
            analytics.Add("readingTime", result.ReadingTime().ToString());
            analytics.Add("speakingTime", result.SpeakingTime().ToString());

            var options = new JsonSerializerOptions()
            {
                WriteIndented = true,

            };
            var client = new Client();
            client.SetEndPoint(Environment.GetEnvironmentVariable("ENDPOINT"));
            client.SetProject(Environment.GetEnvironmentVariable("PROJECT"));
            client.SetKey(Environment.GetEnvironmentVariable("KEY"));
            var database = new Database(client);
            var list = new List<object>() {"*"};
            database.UpdateDocument(document.Id, document.Collection, analytics, list, list);
            try
            {
                RunTask(database.UpdateDocument(document.Collection, document.Id, analytics, list, list)).GetAwaiter().GetResult();
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
