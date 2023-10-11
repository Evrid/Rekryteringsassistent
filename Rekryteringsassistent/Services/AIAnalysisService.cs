using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Rekryteringsassistent.Models;

namespace Rekryteringsassistent.Services
{
    public class AIAnalysisService
    {
        private readonly HttpClient httpClient;
        private readonly string openAiKey;
        private readonly GPT3ResponseProcessor gpt3ResponseProcessor;

        public AIAnalysisService(IConfiguration configuration, GPT3ResponseProcessor gpt3ResponseProcessor)
        {
            httpClient = new HttpClient();
            openAiKey = configuration["openAiKey"];
            this.gpt3ResponseProcessor = gpt3ResponseProcessor;
        }

        public async Task<AIAnalysisResult> AnalyzeCandidate(Candidate candidate, Criteria criteria)
        {
            var prompt = $"Analyze the candidate named {candidate.FirstName} {candidate.LastName} with the following CV:\n{candidate.CV_Text}\nSkills required: {string.Join(", ", criteria.Skills)}\nMinimum experience required: {criteria.MinimumExperience} years.";

            // Adjust the payload to conform to the chat-based model endpoint
            var payload = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
                    new { role = "system", content = "You are a helpful assistant."},
                    new { role = "user", content = prompt }
                }
            };

            string jsonPayload = JsonConvert.SerializeObject(payload);
            var data = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {openAiKey}");

            Console.WriteLine("=== Headers ===");
            foreach (var header in httpClient.DefaultRequestHeaders)
            {
                Console.WriteLine($"Header: {header.Key}, Value: {string.Join(",", header.Value)}");
            }

            var response = await httpClient.PostAsync("https://api.openai.com/v1/chat/completions", data);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var gpt3Response = JsonConvert.DeserializeObject<GPT3Response>(result);
                //something wrong with above

                var analysisResult = gpt3ResponseProcessor.ProcessGPT3Response(gpt3Response.choices[0]?.message?.content);

                return analysisResult;
            }
            else
            {
                // Handle errors
                return null;
            }
        }
    }
}
