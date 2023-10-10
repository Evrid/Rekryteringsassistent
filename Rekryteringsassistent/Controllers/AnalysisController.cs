//using Microsoft.AspNetCore.Mvc;
//using Rekryteringsassistent.Models;
//using Rekryteringsassistent.Services;

//namespace Rekryteringsassistent.Controllers
//{
//    public class AnalysisController : Controller
//    {
//        private readonly AIAnalysisService _aiAnalysisService;

//        public AnalysisController(AIAnalysisService aiAnalysisService)
//        {
//            _aiAnalysisService = aiAnalysisService;
//        }

//        public IActionResult Index()
//        {
//            return View();
//        }

//        [HttpPost]
//        public async Task<IActionResult> Index(Candidate candidate, Criteria criteria) // Or use ViewModel that includes both Candidate and Criteria
//        {
//            if (ModelState.IsValid)
//            {
//                var analysisResult = await _aiAnalysisService.AnalyzeCandidate(candidate, criteria);

//                // Store the analysis result in ViewBag to display it
//                ViewBag.AnalysisResult = analysisResult;
//            }
//            return View(candidate);
//        }
//    }

//}
