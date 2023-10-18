using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Rekryteringsassistent.Models;

namespace Rekryteringsassistent.Services;

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

    //below is code for real ChatGPT response, to use it we need to uncomment it and comment the mock one below
    //public async Task<AIAnalysisResult> AnalyzeCandidate(Candidate candidate, Criteria criteria)
    //{
    //    var prompt = $"Analyze the candidate named {candidate.FirstName} {candidate.LastName} with the following CV:\n{candidate.CV_Text}\nSkills required: {string.Join(", ", criteria.Skills)}\nMinimum experience required: {criteria.MinimumExperience} years. Output below format:\n\n About the candidate: \n\nStrengths:        \n\nWeaknesses:       \n\nCandidate rating:  . \n\n note rating of the candidate is a value between 0 to 10. ";

    //    // Adjust the payload to conform to the chat-based model endpoint
    //    var payload = new
    //    { 
    //        model = "gpt-3.5-turbo",
    //        messages = new[]
    //        {
    //            new { role = "system", content = "You are a HR."},
    //            new { role = "user", content = prompt }
    //        }
    //    };

    //    string jsonPayload = JsonConvert.SerializeObject(payload);
    //    var data = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

    //    httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {openAiKey}");

    //    Console.WriteLine("=== Headers ===");
    //    foreach (var header in httpClient.DefaultRequestHeaders)
    //    {
    //        Console.WriteLine($"Header: {header.Key}, Value: {string.Join(",", header.Value)}");
    //    }

    //    var response = await httpClient.PostAsync("https://api.openai.com/v1/chat/completions", data);

    //    if (response.IsSuccessStatusCode)
    //    {
    //        var result = await response.Content.ReadAsStringAsync();
    //        var gpt3Response = JsonConvert.DeserializeObject<GPT3Response>(result);
    //      

    //        var analysisResult = gpt3ResponseProcessor.ProcessGPT3Response(gpt3Response.choices[0]?.message?.content);

    //        return analysisResult;
    //    }
    //    else
    //    {
    //        // Handle errors
    //        return null;
    //    }
    //}

    //below is code for mock ChatGPT response, if want real the comment below and uncomment above
    public async Task<AIAnalysisResult> AnalyzeCandidate(Candidate candidate, Criteria criteria)
    {
        // Mock result for development or debugging
        var mockResult = new AIAnalysisResult
        {
            // Fill in the properties according to your AIAnalysisResult class
            Id = 1,
            CandidateId=1,
            Candidate=candidate,
            // AboutTheCandidate = "Based on the provided CV, the candidate, named " + candidate.FirstName + " " + candidate.LastName + ", seems to have some relevant qualifications for the position.",
            Strengths = "- Proficiency in Java: The candidate's expertise in Java is certainly a strength as it aligns with the required skill set for the position.",
            Weaknesses = "- Lack of detail in experience: The candidate's CV does not provide any additional information about their experience apart from the one year mentioned.",
            Rating = 7,
            StringRating="I would give 7 because he seems OK"
        };

        return await Task.FromResult(mockResult);

            
    }

}