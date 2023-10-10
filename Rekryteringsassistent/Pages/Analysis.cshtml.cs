using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Rekryteringsassistent.Models;
using Rekryteringsassistent.Services;

namespace Rekryteringsassistent.Pages
{
    public class AnalysisModel : PageModel
    {
        private readonly AIAnalysisService _aiAnalysisService;

        public AnalysisModel(AIAnalysisService aiAnalysisService)
        {
            _aiAnalysisService = aiAnalysisService;
        }

        [BindProperty]
        public Candidate Candidate { get; set; }

        [BindProperty]
        public Criteria Criteria { get; set; }

        public AIAnalysisResult AnalysisResult { get; set; }

        public void OnGet()
        {
            // Initialize Candidate and Criteria if you want
            Candidate = new Candidate();
            Criteria = new Criteria();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Debug.WriteLine("Model state is not valid.");
                // Additional debug information
                foreach (var modelState in ViewData.ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        Debug.WriteLine($"Error: {error.ErrorMessage}");
                    }
                }
            }
                AnalysisResult = await _aiAnalysisService.AnalyzeCandidate(Candidate, Criteria);

            return Page();
        }
    }
}
