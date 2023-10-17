using Microsoft.AspNetCore.Mvc;
using Rekryteringsassistent.Models;
using Rekryteringsassistent.Services;

[ApiController]
[Route("api/[controller]")]
public class AnalysisController : ControllerBase
{
    private readonly AIAnalysisService _aiAnalysisService;
    private readonly ILogger<AnalysisController> _logger;

    public AnalysisController(AIAnalysisService aiAnalysisService, ILogger<AnalysisController> logger)
    {
        _aiAnalysisService = aiAnalysisService;
        _logger = logger;
    }

    [HttpGet]
    public ActionResult<Candidate> Get()
    {
        // Initialize Candidate and Criteria if you want
        Candidate candidate = new Candidate();
        Criteria criteria = new Criteria();

        return Ok(new { Candidate = candidate, Criteria = criteria });
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] AnalysisInputModel inputModel)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogError("Model state is not valid.");
            return BadRequest(ModelState);
        }

        var analysisResult = await _aiAnalysisService.AnalyzeCandidate(inputModel.Candidate, inputModel.Criteria);

        return Ok(analysisResult);
    }
}

public class AnalysisInputModel
{
    public Candidate Candidate { get; set; }
    public Criteria Criteria { get; set; }
}
