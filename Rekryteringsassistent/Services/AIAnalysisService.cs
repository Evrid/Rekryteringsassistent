
using System.Net.Http;
using System.Text;
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
            // Prepare the payload for GPT-3
            //var prompt = $"Analyze the candidate with the following skills: {string.Join(", ", criteria.Skills)} and experience: {criteria.MinimumExperience} years.";
            // Prepare the prompt to include the CV_Text
            var prompt = $"Analyze the candidate named {candidate.FirstName} {candidate.LastName} with the following CV:\n{candidate.CV_Text}\nSkills required: {string.Join(", ", criteria.Skills)}\nMinimum experience required: {criteria.MinimumExperience} years.";

            var payload = new { prompt = prompt, max_tokens = 100 };

            // Serialize payload to JSON
            var jsonPayload = JsonConvert.SerializeObject(payload);
            var data = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            // Set the API key in the headers
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {openAiKey}");

            // Make the API call
            var response = await httpClient.PostAsync("https://api.openai.com/v1/engines/davinci-codex/completions", data);

            // Handle the API response
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var gpt3Response = JsonConvert.DeserializeObject<GPT3Response>(result);

                // Process GPT-3's response
                var analysisResult = gpt3ResponseProcessor.ProcessGPT3Response(gpt3Response.choices[0]?.text);

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


