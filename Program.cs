using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using IBM.Cloud.SDK.Core.Authentication.Iam;
using IBM.Watson.ToneAnalyzer.v3;
using IBM.Watson.ToneAnalyzer.v3.Model;


namespace appwrite_dotnet_function_example
{
    class Program
    {
        static void Main(string[] args)
        {
            Document document = JsonSerializer.Deserialize<Document>(Environment.GetEnvironmentVariable("APPWRITE_FUNCTION_EVENT_PAYLOAD"));
            IamAuthenticator authenticator = new IamAuthenticator(Environment.GetEnvironmentVariable("IBM_API_KEY"));
            ToneAnalyzerService toneAnalyzer = new ToneAnalyzerService("2017-09-21", authenticator);
            toneAnalyzer.SetServiceUrl(Environment.GetEnvironmentVariable("IBM_API_URL"));

            ToneInput toneInput = new ToneInput()
            {
                Text = document.Content
            };

            var result = toneAnalyzer.Tone(toneInput);

            Console.WriteLine(result.Response);
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
