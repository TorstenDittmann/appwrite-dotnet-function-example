using System;
using IBM.Cloud.SDK.Core.Authentication.Iam;
using IBM.Watson.ToneAnalyzer.v3;
using IBM.Watson.ToneAnalyzer.v3.Model;

namespace appwrite_dotnet_function_example
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Environment.GetEnvironmentVariable("APPWRITE_FUNCTION_EVENT_PAYLOAD"));
            Environment.Exit(0);
            if (args.Length == 0) 
            {
                Console.WriteLine("Wrong arguments passed!");
                Environment.Exit(0xA0);
            }
            IamAuthenticator authenticator = new IamAuthenticator(Environment.GetEnvironmentVariable("IBM_API_KEY"));
            ToneAnalyzerService toneAnalyzer = new ToneAnalyzerService("2017-09-21", authenticator);
            toneAnalyzer.SetServiceUrl(Environment.GetEnvironmentVariable("IBM_API_URL"));
            
            ToneInput toneInput = new ToneInput()
            {
                Text = args[0]
            };

            var result = toneAnalyzer.Tone(toneInput);

            Console.WriteLine(result.Response);
        }
    }
}
