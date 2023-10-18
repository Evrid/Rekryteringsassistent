using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Rekryteringsassistent.Models;
using Rekryteringsassistent.Services;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;  // If using Newtonsoft.Json for serialization


namespace Rekryteringsassistent.Pages;

public class AnalysisModel : PageModel
{
    private readonly AIAnalysisService _aiAnalysisService;
    private readonly HttpClient _httpClient;
    private readonly ILogger<AnalysisModel> _logger;


    public AnalysisModel(AIAnalysisService aiAnalysisService, HttpClient httpClient, ILogger<AnalysisModel> logger)
    {
        _aiAnalysisService = aiAnalysisService;
        _httpClient = httpClient;
        _logger = logger; // Initialize logger
    }


    //[JsonProperty(PropertyName = "Candidate")]
    [BindProperty]
    public Candidate Candidate { get; set; }

    // [JsonProperty(PropertyName = "Criteria")]
    [BindProperty]
    public Criteria Criteria { get; set; }

    // [BindProperty]
    public IFormFile CV_File { get; set; }

    public AIAnalysisResult AnalysisResult { get; set; }

    public void OnGet()
    {
        // Initialize Candidate and Criteria 
        Candidate = new Candidate();
        Criteria = new Criteria();
           
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }




  

        var formData = new MultipartFormDataContent
        {
            // For Candidate class
            { new StringContent(Candidate.Id?.ToString() ?? ""), "Candidate.Id" },
            { new StringContent(Candidate.FirstName ?? ""), "Candidate.FirstName" },
            { new StringContent(Candidate.LastName ?? ""), "Candidate.LastName" },
            { new StringContent(Candidate.Email ?? ""), "Candidate.Email" },
 
            { new StringContent(Candidate.CV_Text ?? ""), "Candidate.CV_Text" },
            // For Criteria class
            { new StringContent(Criteria.MinimumExperience?.ToString() ?? ""), "Criteria.MinimumExperience" },
            { new StringContent(Criteria.Skills != null ? string.Join(",", Criteria.Skills) : ""), "Criteria.Skills" },
            { new StringContent(Criteria.Qualifications != null ? string.Join(",", Criteria.Qualifications) : ""), "Criteria.Qualifications" },

        };


        using (var memoryStream = new MemoryStream())
        {
            await CV_File.CopyToAsync(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            var streamContent = new StreamContent(memoryStream);
            formData.Add(streamContent, "CV_File", CV_File.FileName);



            Debug.WriteLine(JsonConvert.SerializeObject(Candidate));
            Debug.WriteLine(JsonConvert.SerializeObject(Criteria));

            // Make the HTTP POST request
            var response = await _httpClient.PostAsync("https://localhost:7031/api/Analysis", formData);

            if (response.IsSuccessStatusCode)
            {
                // Deserialize the response if needed
                var responseJson = await response.Content.ReadAsStringAsync();
                AnalysisResult = JsonConvert.DeserializeObject<AIAnalysisResult>(responseJson);
            }
            else
            {
                // Log error
                var reasonPhrase = response.ReasonPhrase; // Read the reason phrase
                var statusCode = response.StatusCode; // Read status code
                _logger.LogError($"An error occurred while making the API call. StatusCode: {statusCode}, ReasonPhrase: {reasonPhrase}");

            }

        }

        return Page();
    }
}