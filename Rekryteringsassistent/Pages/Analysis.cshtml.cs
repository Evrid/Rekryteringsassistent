using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Rekryteringsassistent.Models;
using Rekryteringsassistent.Services;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;  // If using Newtonsoft.Json for serialization


namespace Rekryteringsassistent.Pages
{
    public class AnalysisModel : PageModel
    {
        private readonly AIAnalysisService _aiAnalysisService;
        private readonly HttpClient _httpClient;

        public AnalysisModel(AIAnalysisService aiAnalysisService, HttpClient httpClient)
        {
            _aiAnalysisService = aiAnalysisService;
            _httpClient = httpClient;
        }


        [BindProperty]
        public Candidate Candidate { get; set; }

        [BindProperty]
        public Criteria Criteria { get; set; }

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
                // Handle invalid model state
                // ...
                return Page();
            }

            // Serialize the Candidate and Criteria objects to JSON
            var inputModel = new AnalysisInputModel { Candidate = Candidate, Criteria = Criteria };
            var json = JsonConvert.SerializeObject(inputModel);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Make the HTTP POST request
            var response = await _httpClient.PostAsync("https://localhost:7031/api/Analysis", content);

            if (response.IsSuccessStatusCode)
            {
                // Deserialize the response if needed
                var responseJson = await response.Content.ReadAsStringAsync();
                AnalysisResult = JsonConvert.DeserializeObject<AIAnalysisResult>(responseJson);
            }
            else
            {
                // Handle error
                // ...
            }

            return Page();
        }
    }

}
